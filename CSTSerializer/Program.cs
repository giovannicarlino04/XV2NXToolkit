using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CSTSerializer
{
    [Serializable]
    public class CharacterEntry
    {
        [XmlAttribute("CostumeID")]
        public int CostumeID { get; set; }

        [XmlElement("Name")]
        public string? Name { get; set; }

        [XmlElement("PaddingHex")]
        public string PaddingHex { get; set; } = "";
    }

    [Serializable]
    [XmlRoot("CSTFile")]
    public class CSTFile
    {
        [XmlElement("Header")]
        public string HeaderHex { get; set; } = "";

        [XmlArray("Characters")]
        [XmlArrayItem("CharacterEntry")]
        public List<CharacterEntry> Characters { get; set; } = new();
    }

    class Program
    {
        const int HEADER_SIZE = 16;
        const int ENTRY_SIZE = 52;
        const int NAME_SIZE = 4;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  CSTSerializer file.cst  -> converts to file.xml");
                Console.WriteLine("  CSTSerializer file.xml  -> converts to file.cst");
                return;
            }

            string input = args[0];

            if (input.EndsWith(".cst", StringComparison.OrdinalIgnoreCase))
            {
                string xmlPath = Path.ChangeExtension(input, ".xml");

                var (entries, header) = ReadCst(input);
                var cstFile = new CSTFile
                {
                    Characters = entries,
                    HeaderHex = BitConverter.ToString(header).Replace("-", " ")
                };

                SerializeXml(cstFile, xmlPath);
                Console.WriteLine($"Parsed {entries.Count} entries and header from .cst and wrote to {xmlPath}");
            }
            else if (input.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            {
                string binPath = Path.ChangeExtension(input, ".cst");

                var cstFile = DeserializeXml(input);
                var headerBytes = ParseHexString(cstFile.HeaderHex, HEADER_SIZE);
                WriteCst(cstFile.Characters, binPath, headerBytes);

                Console.WriteLine($"Loaded {cstFile.Characters.Count} entries from .xml and wrote to {binPath}");
            }
            else
            {
                Console.WriteLine("Unsupported file type. Only .cst and .xml files are supported.");
            }
        }

        static (List<CharacterEntry> entries, byte[] header) ReadCst(string path)
        {
            var result = new List<CharacterEntry>();
            var bytes = File.ReadAllBytes(path);

            if (bytes.Length < HEADER_SIZE)
                throw new Exception("Invalid CST file: too short");

            byte[] header = bytes[..HEADER_SIZE];

            for (int i = HEADER_SIZE; i + ENTRY_SIZE <= bytes.Length; i += ENTRY_SIZE)
            {
                using var ms = new MemoryStream(bytes, i, ENTRY_SIZE);
                using var br = new BinaryReader(ms, Encoding.UTF8);

                int costumeId = br.ReadInt32();
                byte[] nameBytes = br.ReadBytes(NAME_SIZE);
                string name = Encoding.UTF8.GetString(nameBytes).Split('\0')[0];

                byte[] paddingBytes = br.ReadBytes(ENTRY_SIZE - 4 - NAME_SIZE);
                string paddingHex = BitConverter.ToString(paddingBytes).Replace("-", " ");

                result.Add(new CharacterEntry
                {
                    CostumeID = costumeId,
                    Name = CleanInvalidXmlChars(name),
                    PaddingHex = paddingHex
                });
            }

            return (result, header);
        }

        static void WriteCst(List<CharacterEntry> entries, string path, byte[] header)
        {
            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            using var bw = new BinaryWriter(fs, Encoding.UTF8);

            if (header.Length != HEADER_SIZE)
                throw new Exception("Invalid header size");

            bw.Write(header);

            foreach (var entry in entries)
            {
                bw.Write(entry.CostumeID);

                byte[] nameBytes = Encoding.UTF8.GetBytes(entry.Name ?? string.Empty);
                if (nameBytes.Length >= NAME_SIZE)
                    Array.Resize(ref nameBytes, NAME_SIZE - 1);

                Array.Resize(ref nameBytes, NAME_SIZE);
                nameBytes[^1] = 0;

                bw.Write(nameBytes);

                byte[] paddingBytes = ParseHexString(entry.PaddingHex, ENTRY_SIZE - 4 - NAME_SIZE);
                bw.Write(paddingBytes);
            }
        }

        static void SerializeXml(CSTFile data, string path)
        {
            var serializer = new XmlSerializer(typeof(CSTFile));
            using var writer = new StreamWriter(path, false, Encoding.UTF8);
            serializer.Serialize(writer, data);
        }

        static CSTFile DeserializeXml(string path)
        {
            var serializer = new XmlSerializer(typeof(CSTFile));
            using var reader = new StreamReader(path, Encoding.UTF8);
            return (CSTFile)serializer.Deserialize(reader)!;
        }

        static byte[] ParseHexString(string hex, int expectedLength)
        {
            var parts = hex.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != expectedLength)
                throw new Exception($"Hex string must contain exactly {expectedLength} bytes");

            byte[] result = new byte[expectedLength];
            for (int i = 0; i < expectedLength; i++)
                result[i] = Convert.ToByte(parts[i], 16);

            return result;
        }

        static string CleanInvalidXmlChars(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            StringBuilder sb = new();
            foreach (char ch in text)
            {
                if (XmlConvert.IsXmlChar(ch))
                    sb.Append(ch);
            }
            return sb.ToString();
        }
    }
}
