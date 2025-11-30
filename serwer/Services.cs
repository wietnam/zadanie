using System;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace serwer
{
    [XmlRoot("notes")]
    public class ServerNotes
    {
        [XmlElement("note")]
        public List<ServerNoteWrapper> notes { get; set; } = new List<ServerNoteWrapper>();
    }

    public class ServerNoteWrapper
    {
        [XmlElement("author")]
        public string author { get; set; }
        [XmlElement("note")]
        public string noteXml { get; set; } // full <note> XML as string
    }

    public static class Services
    {
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

        public static (byte typ, byte[] payload) OdbierzRamke(NetworkStream ns)
        {
            // ustaw krótki timeout, żeby detectować dead sockets
            ns.ReadTimeout = 15000; // 15s - dopasuj jeśli potrzebujesz
            int hi = ns.ReadByte();
            if (hi == -1) throw new IOException("rozłączono (hi)");
            int lo = ns.ReadByte();
            if (lo == -1) throw new IOException("rozłączono (lo)");
            int len = (hi << 8) | lo;
            int typ = ns.ReadByte();
            if (typ == -1) throw new IOException("rozłączono (typ)");

            if (len < 0) throw new IOException("nieprawidłowa długość payload");
            byte[] payload = new byte[len];
            int read = 0;
            while (read < len)
            {
                int n = ns.Read(payload, read, len - read);
                if (n <= 0) throw new IOException("błąd odczytu payload (n<=0)");
                read += n;
            }
            return ((byte)typ, payload);
        }


        public static void ZapiszNotatkeNaSerwerze(string sciezka, ServerNoteWrapper entry)
        {
            ServerNotes sn = new ServerNotes();
            if (File.Exists(sciezka))
            {
                var ser = new XmlSerializer(typeof(ServerNotes));
                using (var fs = File.OpenRead(sciezka)) sn = (ServerNotes)ser.Deserialize(fs);
            }
            sn.notes.Add(entry);
            var ser2 = new XmlSerializer(typeof(ServerNotes));
            Directory.CreateDirectory(Path.GetDirectoryName(sciezka));
            using (var fs = File.Create(sciezka)) ser2.Serialize(fs, sn);
        }

        // Szybkie parsowanie tytułu z note xml (proste)
        public static string ParsujTytulZXml(string xml)
        {
            try
            {
                int a = xml.IndexOf("<title>");
                int b = xml.IndexOf("</title>");
                if (a >= 0 && b > a) return xml.Substring(a + 7, b - (a + 7));
            }
            catch { }
            return "(brak tytułu)";
        }
    }
}
