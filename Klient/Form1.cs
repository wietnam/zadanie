using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Klient
{
    public partial class Form1 : Form
    {
        // ostatnie po³¹czenie
        TcpClient tcpClient;
        Thread recvThread;
        volatile bool receiving = false;

        // lista otrzymanych notatek (odszyfrowanych)
        List<NoteItem> notes = new List<NoteItem>();

        public Form1()
        {
            InitializeComponent();

            // podpinamy zdarzenia (nazwy metod po polsku)
            btnConnect.Click += Polacz_Click;
            btnSaveLocal.Click += ZapiszLokalnie_Click;
            btnSearch.Click += Szukaj_Click;
            lstNotes.SelectedIndexChanged += ListaNotatek_Zmieniona;
        }

        // ============================
        // Po³¹czenie do serwera (klik)
        // ============================
        private void Polacz_Click(object sender, EventArgs e)
        {
            // sprawdŸ czy ju¿ po³¹czony
            if (tcpClient != null && tcpClient.Connected)
            {
                MessageBox.Show("Already connected.");
                return;
            }

            string ip = txtServerIp.Text.Trim();
            if (!int.TryParse(txtPort.Text, out int port))
            {
                MessageBox.Show("Bad port.");
                return;
            }

            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(ip, port);
                MessageBox.Show("Connected.");

                receiving = true;
                recvThread = new Thread(() => OdbierzPetle(tcpClient.GetStream(), txtPassword.Text));
                recvThread.IsBackground = true;
                recvThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connect error: " + ex.Message);
            }
        }

        // =================================================
        // Pêtla odbieraj¹ca ramki 4P-Frame (czyta big-endian)
        // =================================================
        void OdbierzPetle(NetworkStream ns, string password)
        {
            try
            {
                while (receiving)
                {
                    int hi = ns.ReadByte();
                    if (hi == -1) break;
                    int lo = ns.ReadByte();
                    if (lo == -1) break;
                    int type = ns.ReadByte();
                    if (type == -1) break;

                    int len = (hi << 8) | lo;

                    byte[] payload = new byte[len];
                    int read = 0;
                    while (read < len)
                    {
                        int r = ns.Read(payload, read, len - read);
                        if (r <= 0) throw new Exception("Stream closed");
                        read += r;
                    }

                    if (type == 0x01)
                    {
                        string xml = Encoding.UTF8.GetString(payload);
                        PrzetworzOdebranaNotatke(xml, password);
                    }
                    else if (type == 0x02 || type == 0x03)
                    {
                        string msg = Encoding.UTF8.GetString(payload);
                        this.Invoke(new Action(() => MessageBox.Show("Server message: " + msg)));
                    }
                    else if (type == 0xFF)
                    {
                        this.Invoke(new Action(() => MessageBox.Show("End of stream")));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() => MessageBox.Show("Receive error: " + ex.Message)));
            }
            finally
            {
                tcpClient?.Close();
                tcpClient = null;
                receiving = false;
            }
        }

        // =================================================
        // Parsowanie otrzymanego XMLa, deszyfrowanie body i dodanie do listy
        // =================================================
        void PrzetworzOdebranaNotatke(string xml, string password)
        {
            try
            {
                var el = XElement.Parse(xml);
                bool encrypted = (el.Attribute("encrypted")?.Value ?? "false").ToLower() == "true";
                string title = (string)el.Element("title") ?? "";
                string created = (string)el.Element("created") ?? "";
                var tags = el.Element("tags")?.Elements("tag").Select(x => (string)x).ToArray() ?? new string[0];
                string bodyRaw = (string)el.Element("body") ?? "";

                string bodyPlain;
                if (encrypted)
                {
                    byte[] enc = Convert.FromBase64String(bodyRaw);
                    byte[] dec = Crypto.Decrypt(enc, password); // korzystamy z tego samego algorytmu co server
                    bodyPlain = Encoding.UTF8.GetString(dec);
                }
                else
                {
                    bodyPlain = bodyRaw;
                }

                var note = new NoteItem { Title = title, Created = created, Tags = tags, Body = bodyPlain };
                notes.Add(note);

                this.Invoke(new Action(() => { lstNotes.Items.Add(title); }));
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() => MessageBox.Show("Parse error: " + ex.Message)));
            }
        }

        // =================================================
        // Zapis wszystkich otrzymanych notatek lokalnie do XML (odszyfrowane)
        // =================================================
        private void ZapiszLokalnie_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "XML File|*.xml";
            dlg.FileName = "client_notes.xml";
            if (dlg.ShowDialog() != DialogResult.OK) return;

            var root = new XElement("notes");
            foreach (var n in notes)
            {
                var tagsEl = (n.Tags ?? Array.Empty<string>()).Select(t => new XElement("tag", t));
                var el = new XElement("note",
                    new XAttribute("version", 1),
                    new XAttribute("encrypted", "false"),
                    new XElement("title", n.Title),
                    new XElement("created", n.Created),
                    new XElement("tags", tagsEl),
                    new XElement("body", n.Body)
                );
                root.Add(el);
            }

            var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root);
            doc.Save(dlg.FileName);
            MessageBox.Show("Saved: " + dlg.FileName);
        }

        // =================================================
        // Prosta wyszukiwarka po tytule lub tagach (klik)
        // =================================================
        private void Szukaj_Click(object sender, EventArgs e)
        {
            string q = (txtSearch.Text ?? "").Trim().ToLower();
            lstNotes.Items.Clear();
            foreach (var n in notes)
            {
                if (string.IsNullOrEmpty(q) ||
                    (n.Title ?? "").ToLower().Contains(q) ||
                    (n.Tags != null && n.Tags.Any(t => t.ToLower().Contains(q))))
                {
                    lstNotes.Items.Add(n.Title);
                }
            }
        }

        // =================================================
        // Pokazanie zaznaczonej notatki w polu podgl¹du
        // =================================================
        private void ListaNotatek_Zmieniona(object sender, EventArgs e)
        {
            int i = lstNotes.SelectedIndex;
            if (i < 0 || i >= notes.Count) { txtNoteBody.Text = ""; return; }
            txtNoteBody.Text = notes[i].Body;
        }

        // =================================================
        // PROSTY ALGORITM DESZYFROWANIA (XOR + SHIFT)
        // =================================================
        static class Crypto
        {
            public static byte[] Decrypt(byte[] data, string password)
            {
                byte[] key = string.IsNullOrEmpty(password) ? new byte[] { 0 } : Encoding.UTF8.GetBytes(password);
                int n = key.Length;
                int shift = n % 256;
                byte[] outb = new byte[data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    byte b = (byte)((data[i] - shift) & 0xFF);
                    b = (byte)(b ^ key[i % n]);
                    outb[i] = b;
                }
                return outb;
            }
        }

        // model notatki w kliencie
        class NoteItem
        {
            public string Title;
            public string Created;
            public string[] Tags;
            public string Body;
        }
    }
}
