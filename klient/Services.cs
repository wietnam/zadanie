using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Net.Sockets;
using System.Collections.Generic;
using klient;

namespace klient
{
    public static class Services
    {
        // --- Storage XML ---
        public static void ZapiszNotatkiDoPliku(string sciezka, NotesList list)
        {
            try
            {
                string dir = Path.GetDirectoryName(sciezka);
                if (string.IsNullOrEmpty(dir))
                    dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                Directory.CreateDirectory(dir);
                var ser = new XmlSerializer(typeof(NotesList));
                using (var fs = File.Create(sciezka)) ser.Serialize(fs, list);
            }
            catch (Exception ex)
            {
                // ewentualnie loguj
                throw;
            }
        }

        public static NotesList WczytajNotatkiZPliku(string sciezka)
        {
            try
            {
                if (!File.Exists(sciezka)) return new NotesList();
                var ser = new XmlSerializer(typeof(NotesList));
                using (var fs = File.OpenRead(sciezka)) return (NotesList)ser.Deserialize(fs);
            }
            catch
            {
                return new NotesList();
            }
        }


        public static void ZapiszDaneUzytkownika(string sciezka, UserEntry u)
        {
            UsersList ul = new UsersList();
            if (File.Exists(sciezka))
            {
                var ser = new XmlSerializer(typeof(UsersList));
                using (var fs = File.OpenRead(sciezka)) ul = (UsersList)ser.Deserialize(fs);
            }
            var exist = ul.users.Find(x => x.name == u.name);
            if (exist != null) { exist.password = u.password; } else ul.users.Add(u);
            var ser2 = new XmlSerializer(typeof(UsersList));
            using (var fs = File.Create(sciezka)) ser2.Serialize(fs, ul);
        }

        public static UserEntry WczytajUzytkownika(string sciezka, string name)
        {
            if (!File.Exists(sciezka)) return null;
            var ser = new XmlSerializer(typeof(UsersList));
            using (var fs = File.OpenRead(sciezka))
            {
                var ul = (UsersList)ser.Deserialize(fs);
                return ul.users.Find(x => x.name == name);
            }
        }

        // --- Crypto (XOR + rotacja + Base64) ---
        public static string ZaszyfrujTekst(string tresc, string haslo)
        {
            if (string.IsNullOrEmpty(haslo)) haslo = "empty";
            byte[] key = Encoding.UTF8.GetBytes(haslo);
            byte[] data = Encoding.UTF8.GetBytes(tresc ?? "");
            byte[] wynik = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                byte k = key[i % key.Length];
                int rot = (i % 5) - 2;
                byte xored = (byte)(data[i] ^ k);
                wynik[i] = (byte)((xored + rot) & 0xFF);
            }
            return Convert.ToBase64String(wynik);
        }

        public static string OdszyfrujTekst(string zaszyfrowaneBase64, string haslo)
        {
            if (string.IsNullOrEmpty(haslo)) haslo = "empty";
            byte[] key = Encoding.UTF8.GetBytes(haslo);
            byte[] data = Convert.FromBase64String(zaszyfrowaneBase64 ?? "");
            byte[] wynik = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                byte k = key[i % key.Length];
                int rot = (i % 5) - 2;
                byte x = (byte)((data[i] - rot) & 0xFF);
                wynik[i] = (byte)(x ^ k);
            }
            return Encoding.UTF8.GetString(wynik);
        }

        // --- 4P-Frame ---
        public static byte[] StworzRamke(byte typ, byte[] payload)
        {
            int len = payload.Length;
            byte hi = (byte)((len >> 8) & 0xFF);
            byte lo = (byte)(len & 0xFF);
            byte[] ramka = new byte[3 + len];
            ramka[0] = hi; ramka[1] = lo; ramka[2] = typ;
            Array.Copy(payload, 0, ramka, 3, len);
            return ramka;
        }

        public static void WyslijXmlDoSerwera(string xml, NetworkStream stream)
        {
            byte[] payload = Encoding.UTF8.GetBytes(xml);
            byte[] ramka = StworzRamke(0x01, payload);
            stream.Write(ramka, 0, ramka.Length);
            stream.Flush();
        }

        // --- Network helper: read frame (synchron) ---
        public static (byte typ, byte[] payload) OdbierzRamke(NetworkStream ns)
        {
            int hi = ns.ReadByte(); if (hi == -1) throw new IOException("rozłączono");
            int lo = ns.ReadByte(); if (lo == -1) throw new IOException("rozłączono");
            int len = (hi << 8) | lo;
            int typ = ns.ReadByte(); if (typ == -1) throw new IOException("rozłączono");
            byte[] payload = new byte[len];
            int read = 0;
            while (read < len)
            {
                int n = ns.Read(payload, read, len - read);
                if (n <= 0) throw new IOException("błąd odczytu payload");
                read += n;
            }
            return ((byte)typ, payload);
        }
        public static void LogKlienta(string katalog, string msg)
        {
            try
            {
                Directory.CreateDirectory(katalog);
                File.AppendAllText(Path.Combine(katalog, "client.log"),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + msg + Environment.NewLine);
            }
            catch { }
        }


    }
}
