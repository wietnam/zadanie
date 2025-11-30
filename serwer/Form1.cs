using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using serwer;

namespace serwer
{
    public partial class Form1 : Form
    {
        private TcpListener listener;
        private bool running = false;
        private string katalog;

        public Form1()
        {
            InitializeComponent();
            katalog = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ZapisNotatekSERWER");
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void StartSerwera_Click(object sender, EventArgs e)
        {
            if (running) return;
            int port = int.Parse(textBoxPort.Text.Trim());
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            running = true;
            Task.Run(() => AcceptLoop());
            MessageBox.Show("Serwer uruchomiony");
        }

        private void StopSerwera_Click(object sender, EventArgs e)
        {
            running = false;
            listener?.Stop();
            MessageBox.Show("Serwer zatrzymany");
        }

        private async Task AcceptLoop()
        {
            while (running)
            {
                try
                {
                    var klient = await listener.AcceptTcpClientAsync();
                    Task.Run(() => OdbierajStream(klient));
                }
                catch { if (!running) break; }
            }
        }

        private void Log(string msg)
        {
            try
            {
                string katalogLog = Path.Combine(katalog, "logs");
                Directory.CreateDirectory(katalogLog);
                File.AppendAllText(Path.Combine(katalogLog, "server.log"),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + msg + Environment.NewLine);
            }
            catch { }
        }

        private void OdbierajStream(TcpClient klient)
        {
            string klientInfo = klient.Client.RemoteEndPoint?.ToString() ?? "unknown";
            string username = null; // tu trzymamy nazwê u¿ytkownika z 0x02
            Log($"NOWE_PO£¥CZENIE: {klientInfo}");
            using (klient)
            {
                var ns = klient.GetStream();
                try
                {
                    while (running)
                    {
                        try
                        {
                            var ramka = Services.OdbierzRamke(ns);
                            if (ramka.typ == 0x02)
                            {
                                username = Encoding.UTF8.GetString(ramka.payload);
                                Log($"Ustalono username dla {klientInfo}: {username}");
                                this.Invoke(new Action(() =>
                                {
                                    listViewSerwer.Items.Add(new ListViewItem(new string[] { $"<login> {username}", klientInfo, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }));
                                }));
                                continue;
                            }
                            else if (ramka.typ == 0x01)
                            {
                                string xml = Encoding.UTF8.GetString(ramka.payload);
                                string tytul = Services.ParsujTytulZXml(xml);
                                string autor = string.IsNullOrEmpty(username) ? klientInfo : username;
                                var entry = new ServerNoteWrapper { author = autor, noteXml = xml };
                                string sciezka = Path.Combine(katalog, "server_notes.xml");
                                // zabezpieczenie: lock przy zapisie pliku
                                lock (typeof(Services))
                                {
                                    Services.ZapiszNotatkeNaSerwerze(sciezka, entry);
                                }
                                this.Invoke(new Action(() =>
                                {
                                    var item = new ListViewItem(new string[] { tytul, autor, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                                    listViewSerwer.Items.Add(item);
                                }));
                                continue;
                            }
                            else if (ramka.typ == 0xFF) break;
                        }
                        catch (IOException ioEx)
                        {
                            Log($"IO error z {klientInfo}: {ioEx.Message}");
                            break;
                        }
                        catch (Exception ex)
                        {
                            Log($"B³¹d w pêtli odczytu od {klientInfo}: {ex.Message}");
                            break;
                        }
                    }
                }
                finally
                {
                    Log($"ZAMKNIÊCIE_PO£¥CZENIA: {klientInfo}");
                }
            }
        }


    }
}
