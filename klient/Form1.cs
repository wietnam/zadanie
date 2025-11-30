using System;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using klient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace klient
{
    public partial class Form1 : Form
    {
        private TcpClient klient;
        private NetworkStream ns;
        private string katalog;
        private NotesList mojeNotatki = new NotesList();

        public Form1()
        {
            InitializeComponent();
            katalog = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ZapisNotatekKLIENT");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LadowanieWartosciKlienta();
        }

        // --- GUI actions (po polsku) ---
        private void Polacz_Click(object sender, EventArgs e)
        {
            try
            {
                // utwórz klienta i po³¹cz
                klient = new TcpClient();
                klient.Connect(textBoxIP.Text.Trim(), int.Parse(textBoxPort.Text.Trim()));

                // pobierz stream **dopiero po** pomyœlnym po³¹czeniu
                ns = klient.GetStream();

                // upewnij siê, ¿e stream istnieje i mo¿na pisaæ
                if (ns == null || !ns.CanWrite)
                {
                    MessageBox.Show("Po³¹czono, ale brak dostêpnego strumienia do zapisu.");
                    return;
                }

                // wysy³amy ramkê info z nazw¹ u¿ytkownika, aby serwer móg³ zapisaæ autora
                string username = textBoxNazwa.Text.Trim();
                if (!string.IsNullOrEmpty(username))
                {
                    byte[] payload = Encoding.UTF8.GetBytes(username);
                    byte[] ramka = Services.StworzRamke(0x02, payload); // typ 0x02 = komunikat/ident
                    ns.Write(ramka, 0, ramka.Length);
                    ns.Flush();
                }

                MessageBox.Show("Po³¹czono i wys³ano login");
                // wczytaj lokalne notatki po nazwie
                LadowanieWartosciKlienta();
            }
            catch (FormatException fex)
            {
                MessageBox.Show("B³êdny port: " + fex.Message);
            }
            catch (SocketException sex)
            {
                MessageBox.Show("B³¹d po³¹czenia sieciowego: " + sex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("B³¹d wys³ania loginu: " + ex.Message);
            }
        }


        private void Rozlacz_Click(object sender, EventArgs e)
        {
            try
            {
                ns?.Close();
                klient?.Close();
                MessageBox.Show("Roz³¹czono");
            }
            catch { }
        }

        private void ZapiszNotatkiLokalnie_Click(object sender, EventArgs e)
        {
            var n = StworzNotatkeZGui();
            mojeNotatki.notes.Add(n);
            string sciezka = Path.Combine(katalog, $"{textBoxNazwa.Text}_notes.xml");
            Services.ZapiszNotatkiDoPliku(sciezka, mojeNotatki);
            PokazNotatkeWGui(n);
            // zapisz dane logowania
            Services.ZapiszDaneUzytkownika(Path.Combine(katalog, "users.xml"),
                new UserEntry { name = textBoxNazwa.Text, password = Convert.ToBase64String(Encoding.UTF8.GetBytes(textBoxHaslo.Text)) });
            MessageBox.Show("Zapisano lokalnie");
        }

        private void WyslijNotatkeNaSerwer_Click(object sender, EventArgs e)
        {
            if (ns == null || klient == null || !ns.CanWrite)
            {
                MessageBox.Show("Brak po³¹czenia z serwerem.");
                return;
            }

            try
            {
                // stwórz notatkê z GUI
                var n = StworzNotatkeZGui();
                n.encrypted = true;
                n.body = Services.ZaszyfrujTekst(n.body, textBoxHaslo.Text);

                // serializuj pojedyncze <note>
                var ser = new System.Xml.Serialization.XmlSerializer(typeof(Note));
                string xml;
                using (var sw = new System.IO.StringWriter()) { ser.Serialize(sw, n); xml = sw.ToString(); }

                // wyœlij ramkê
                Services.WyslijXmlDoSerwera(xml, ns);

                // dodaj do lokalnych notatek i zapisz je do pliku u¿ytkownika
                mojeNotatki.notes.Add(n);
                string sciezka = Path.Combine(katalog, $"{textBoxNazwa.Text}_notes.xml");
                Services.ZapiszNotatkiDoPliku(sciezka, mojeNotatki);

                PokazNotatkeWGui(n);
                MessageBox.Show("Wys³ano na serwer.");
            }
            catch (Exception ex)
            {
                // loguj lokalnie
                try { Services.LogKlienta(katalog, "B³¹d wysy³ania notatki: " + ex.Message); } catch { }
                MessageBox.Show("B³¹d wysy³ania notatki: " + ex.Message);
            }
        }


        // --- helpers ---
        private Note StworzNotatkeZGui()
        {
            return new Note
            {
                title = textBoxTitle.Text.Trim(),
                created = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                tags = new List<string>(textBoxTags.Text.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)),
                body = textBoxBody.Text ?? "",
                encrypted = false,
                version = 1
            };
        }

        private void LadowanieWartosciKlienta()
        {
            listViewNotatki.Items.Clear();
            mojeNotatki = new NotesList();
            string sciezkaUzytkownika = Path.Combine(katalog, $"{textBoxNazwa.Text}_notes.xml");
            mojeNotatki = Services.WczytajNotatkiZPliku(sciezkaUzytkownika) ?? new NotesList();
            foreach (var n in mojeNotatki.notes) PokazNotatkeWGui(n);

            var u = Services.WczytajUzytkownika(Path.Combine(katalog, "users.xml"), textBoxNazwa.Text);
            if (u != null)
            {
                try { textBoxHaslo.Text = Encoding.UTF8.GetString(Convert.FromBase64String(u.password)); } catch { }
            }
        }


        private void PokazNotatkeWGui(Note n)
        {
            var item = new ListViewItem(new string[] { n.title, n.created, string.Join(",", n.tags) });
            item.Tag = n;
            listViewNotatki.Items.Add(item);
        }
    }
}
