using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace klient
{
    [XmlRoot("note")]
    public class Note
    {
        [XmlAttribute("version")]
        public int version { get; set; } = 1;

        [XmlAttribute("encrypted")]
        public bool encrypted { get; set; }

        [XmlElement("title")]
        public string title { get; set; }

        [XmlElement("created")]
        public string created { get; set; } // "RRRR-MM-DD HH:MM:SS"

        [XmlArray("tags")]
        [XmlArrayItem("tag")]
        public List<string> tags { get; set; } = new List<string>();

        [XmlElement("body")]
        public string body { get; set; } // base64 zaszyfrowane
    }

    [XmlRoot("notes")]
    public class NotesList
    {
        [XmlElement("note")]
        public List<Note> notes { get; set; } = new List<Note>();
    }

    [XmlRoot("users")]
    public class UsersList
    {
        [XmlElement("user")]
        public List<UserEntry> users { get; set; } = new List<UserEntry>();
    }

    public class UserEntry
    {
        [XmlElement("name")]
        public string name { get; set; }
        [XmlElement("password")]
        public string password { get; set; } // zaszyfrowane lub base64
    }
}
