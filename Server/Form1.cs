using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Server
{
    public partial class Form1 : Form
    {
        // ====== POLA SERWERA ======
        TcpListener listener;           // nas³uch TCP
        TcpClient client;               // ostatni pod³¹czony klient
        Thread listenThread;            // w¹tek akceptuj¹cy klientów
        bool listening = false;         // czy serwer nas³uchuje

        // Lista notatek przechowywana w pamiêci
        List<Notatka> notes = new List<Notatka>();

        public Form1()
        {
            InitializeComponent();

            // podpinanie przycisków
            btnStart.Click += BtnStart_Click;
            btnAddNote.Click += BtnAddNote_Click;
            btnSaveXml.Click += BtnSaveXml_Click;
            btnSend.Click += BtnSend_Click;
        }

        // =================================================
        // 1. NAS£UCH NA PORCIE TCP PODANYM PRZEZ U¯YTKOWNIKA
        // =================================================
        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (listening)
            {
                MessageBox.Show("Serwer ju¿ nas³uchuje.");
                return;
            }

            if (!int.TryParse(txtPort.Text, out int port))
            {
                MessageBox.Show("Nieprawid³owy port.");
                return;
            }

            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                listening = true;

                listenThread = new Thread(AcceptLoop);
                listenThread.IsBackground = true;
                listenThread.Start();

                MessageBox.Show("Serwer nas³uchuje na porcie: " + port);
            }
            catch (Exception ex)
            {
                MessageBox.Show("B³¹d uruchamiania nas³uchu: " + ex.Message);
            }
        }

        // pêtla akceptuj¹ca nowych klientów TCP
        void AcceptLoop()
        {
            try
            {
                while (listening)
                {
                    var c = listener.AcceptTcpClient();
                    client = c;

                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show("Po³¹czono klienta: " + c.Client.RemoteEndPoint);
                    }));
                }
            }
            catch
            {
                // ignorujemy b³êdy zamykania listenera
            }
        }

        // ============================
        // DODAWANIE NOTATKI Z GUI
        // ============================
        private void BtnAddNote_Click(object sender, EventArgs e)
        {
            var n = new Notatka();
            n.Tytul = txtTitle.Text;
            n.Tresc = txtBody.Text;
            n.Tagi = (txtTags.Text ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            n.Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            n.Wersja = 1;

            notes.Add(n);
            RefreshList();
        }

        void RefreshList()
        {
            lstNotes.Items.Clear();
            foreach (var n in notes)
                lstNotes.Items.Add(n.Tytul);
        }

        // =========================================================
        // 3. ZAPIS WSZYSTKICH NOTATEK DO PLIKU XML (FORMAT ZADANY)
        // =========================================================
        private void BtnSaveXml_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "Plik XML|*.xml";
            dlg.FileName = "notatki.xml";
            if (dlg.ShowDialog() != DialogResult.OK) return;

            var root = new XElement("notes");

            foreach (var n in notes)
            {
                var tagsElements = (n.Tagi ?? Array.Empty<string>())
                    .Select(t => new XElement("tag", t));

                var el = new XElement("note",
                    new XAttribute("version", n.Wersja),
                    new XAttribute("encrypted", "false"),
                    new XElement("title", n.Tytul),
                    new XElement("created", n.Data),
                    new XElement("tags", tagsElements),
                    new XElement("body", n.Tresc)
                );

                root.Add(el);
            }

            var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root);
            doc.Save(dlg.FileName);

            MessageBox.Show("Zapisano notatki do: " + dlg.FileName);
        }

        // =========================================================
        // 2 i 4. STRUMIENIOWE WYSY£ANIE TCP + PROTOKÓ£ 4P-FRAME
        //
        // 4P-Frame:
        // [LEN_HI][LEN_LO][TYPE][PAYLOAD...]
        //
        // LEN = d³ugoœæ PAYLOAD (2 bajty, big-endian)
        // TYPE = 0x01 (notatka), 0xFF (koniec strumienia)
        // =========================================================
        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (client == null || !client.Connected)
            {
                MessageBox.Show("Brak po³¹czonego klienta.");
                return;
            }

            var ns = client.GetStream();

            try
            {
                foreach (var n in notes)
                {
                    string xml = BuildNoteXml(n);
                    byte[] payload = Encoding.UTF8.GetBytes(xml);

                    byte[] frame = MakeFrame(payload, 0x01);

                    ns.Write(frame, 0, frame.Length);
                    ns.Flush();
                }

                byte[] endFrame = MakeFrame(Array.Empty<byte>(), 0xFF);
                ns.Write(endFrame, 0, endFrame.Length);
                ns.Flush();

                MessageBox.Show("Wys³ano wszystkie notatki (strumieñ 4P-Frame).");
            }
            catch (Exception ex)
            {
                MessageBox.Show("B³¹d podczas wysy³ania: " + ex.Message);
            }
        }

        // budowanie XML pojedynczej notatki
        string BuildNoteXml(Notatka n)
        {
            var tags = "";

            foreach (var t in n.Tagi ?? Array.Empty<string>())
                tags += $"<tag>{System.Security.SecurityElement.Escape(t)}</tag>";

            return
                $"<note version=\"{n.Wersja}\" encrypted=\"false\">" +
                $"<title>{System.Security.SecurityElement.Escape(n.Tytul)}</title>" +
                $"<created>{n.Data}</created>" +
                $"<tags>{tags}</tags>" +
                $"<body>{System.Security.SecurityElement.Escape(n.Tresc)}</body>" +
                $"</note>";
        }

        // tworzy ramkê 4P-Frame zgodnie z wymaganiem
        byte[] MakeFrame(byte[] payload, byte type)
        {
            if (payload == null) payload = Array.Empty<byte>();

            int len = payload.Length;
            byte hi = (byte)((len >> 8) & 0xFF);
            byte lo = (byte)(len & 0xFF);

            byte[] frame = new byte[3 + len];
            frame[0] = hi;
            frame[1] = lo;
            frame[2] = type;

            Buffer.BlockCopy(payload, 0, frame, 3, len);

            return frame;
        }

        // klasa przechowuj¹ca dane notatki
        class Notatka
        {
            public string Tytul;
            public string Tresc;
            public string Data;
            public string[] Tagi;
            public int Wersja;
        }
    }
}
