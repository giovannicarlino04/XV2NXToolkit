using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;

public class X2MFile
{
    public string Type { get; set; }
    public string ModName { get; set; }
    public string ModAuthor { get; set; }
    public string ModVersion { get; set; }
    public string ModGuid { get; set; }
    public string UData { get; set; }
    public string EntryName { get; set; }

    public Dictionary<string, string> CharaNames = new Dictionary<string, string>();
    public List<SlotEntry> SlotEntries = new List<SlotEntry>();
    public List<SkillSetEntry> SkillSets = new List<SkillSetEntry>();
    public CsoEntry Cso { get; set; }
    public PscSpecEntry Psc { get; set; }

    public static X2MFile Load(string path)
    {
        var doc = XDocument.Load(path);
        var root = doc.Root;
        var x2m = new X2MFile();

        x2m.Type = root.Attribute("type")?.Value;
        x2m.ModName = root.Element("MOD_NAME")?.Attribute("value")?.Value;
        x2m.ModAuthor = root.Element("MOD_AUTHOR")?.Attribute("value")?.Value;
        x2m.ModVersion = root.Element("MOD_VERSION")?.Attribute("value")?.Value;
        x2m.ModGuid = root.Element("MOD_GUID")?.Attribute("value")?.Value;
        x2m.UData = root.Element("UDATA")?.Attribute("value")?.Value;
        x2m.EntryName = root.Element("ENTRY_NAME")?.Attribute("value")?.Value;

        foreach (var elem in root.Elements().Where(e => e.Name.LocalName.StartsWith("CHARA_NAME_")))
        {
            x2m.CharaNames[elem.Name.LocalName] = elem.Attribute("value")?.Value;
        }

        foreach (var slot in root.Elements("SlotEntry"))
        {
            x2m.SlotEntries.Add(SlotEntry.FromXElement(slot));
        }

        foreach (var skill in root.Elements("SkillSet"))
        {
            x2m.SkillSets.Add(SkillSetEntry.FromXElement(skill));
        }

        var cso = root.Element("CsoEntry");
        if (cso != null)
            x2m.Cso = CsoEntry.FromXElement(cso);

        var psc = root.Element("PscSpecEntry");
        if (psc != null)
            x2m.Psc = PscSpecEntry.FromXElement(psc);

        return x2m;
    }

    public void Save(string path)
    {
        var doc = new XDocument(new XElement("X2M",
            new XAttribute("type", Type),
            new XElement("X2M_FORMAT_VERSION", new XAttribute("value", "23.0")),
            new XElement("MOD_NAME", new XAttribute("value", ModName)),
            new XElement("MOD_AUTHOR", new XAttribute("value", ModAuthor)),
            new XElement("MOD_VERSION", new XAttribute("value", ModVersion)),
            new XElement("MOD_GUID", new XAttribute("value", ModGuid)),
            new XElement("UDATA", new XAttribute("value", UData)),
            new XElement("ENTRY_NAME", new XAttribute("value", EntryName)),

            CharaNames.Select(kv => new XElement(kv.Key, new XAttribute("value", kv.Value))),
            SlotEntries.Select(s => s.ToXElement()),
            SkillSets.Select(s => s.ToXElement()),
            Cso?.ToXElement(),
            Psc?.ToXElement()
        ));

        doc.Save(path);
    }

    public static X2MFile CreateDefault()
    {
        return new X2MFile
        {
            Type = "NEW_CHARACTER",
            ModName = "New Character",
            ModAuthor = "Author",
            ModVersion = "1.0",
            ModGuid = Guid.NewGuid().ToString(),
            UData = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
            EntryName = "XXX",
            CharaNames = new Dictionary<string, string>
            {
                { "CHARA_NAME_EN", "Default Character" }
            },
            SlotEntries = new List<SlotEntry>(),
            SkillSets = new List<SkillSetEntry>()
        };
    }
}

// SlotEntry
public class SlotEntry
{
    public int CostumeIndex;
    public string ModelPreset;
    public bool FlagGk2;
    public string Voices;
    public Dictionary<string, string> CostumeNames = new Dictionary<string, string>();

    public static SlotEntry FromXElement(XElement e)
    {
        var entry = new SlotEntry
        {
            CostumeIndex = int.Parse(e.Attribute("costume_index")?.Value ?? "0"),
            ModelPreset = e.Element("MODEL_PRESET")?.Attribute("value")?.Value,
            FlagGk2 = bool.Parse(e.Element("FLAG_GK2")?.Attribute("value")?.Value ?? "false"),
            Voices = e.Element("VOICES_ID_LIST")?.Attribute("value")?.Value
        };

        foreach (var cn in e.Elements().Where(x => x.Name.LocalName.StartsWith("COSTUME_NAME_")))
            entry.CostumeNames[cn.Name.LocalName] = cn.Attribute("value")?.Value;

        return entry;
    }

    public XElement ToXElement()
    {
        return new XElement("SlotEntry", new XAttribute("costume_index", CostumeIndex),
            new XElement("MODEL_PRESET", new XAttribute("value", ModelPreset)),
            new XElement("FLAG_GK2", new XAttribute("value", FlagGk2.ToString().ToLower())),
            new XElement("VOICES_ID_LIST", new XAttribute("value", Voices)),
            CostumeNames.Select(kv => new XElement(kv.Key, new XAttribute("value", kv.Value)))
        );
    }
}

// SkillSetEntry
public class SkillSetEntry
{
    public string CharId;
    public string CostumeId;
    public string Skills;
    public int ModelPreset;

    public static SkillSetEntry FromXElement(XElement e) => new SkillSetEntry
    {
        CharId = e.Element("CHAR_ID")?.Attribute("value")?.Value,
        CostumeId = e.Element("COSTUME_ID")?.Attribute("value")?.Value,
        Skills = e.Element("SKILLS")?.Attribute("value")?.Value,
        ModelPreset = int.Parse(e.Element("MODEL_PRESET")?.Attribute("value").Value ?? "0")
    };

    public XElement ToXElement()
    {
        return new XElement("SkillSet",
            new XElement("CHAR_ID", new XAttribute("value", CharId)),
            new XElement("COSTUME_ID", new XAttribute("value", CostumeId)),
            new XElement("SKILLS", new XAttribute("value", Skills)),
            new XElement("MODEL_PRESET", new XAttribute("value", ModelPreset))
        );
    }
}

// CsoEntry
public class CsoEntry
{
    public string CharId, CostumeId, SE, VOX, AMK, SKILLS;

    public static CsoEntry FromXElement(XElement e) => new CsoEntry
    {
        CharId = e.Element("CHAR_ID")?.Attribute("value")?.Value,
        CostumeId = e.Element("COSTUME_ID")?.Attribute("value")?.Value,
        SE = e.Element("SE")?.Attribute("value")?.Value,
        VOX = e.Element("VOX")?.Attribute("value")?.Value,
        AMK = e.Element("AMK")?.Attribute("value")?.Value,
        SKILLS = e.Element("SKILLS")?.Attribute("value")?.Value
    };

    public XElement ToXElement() => new XElement("CsoEntry",
        new XElement("CHAR_ID", new XAttribute("value", CharId)),
        new XElement("COSTUME_ID", new XAttribute("value", CostumeId)),
        new XElement("SE", new XAttribute("value", SE)),
        new XElement("VOX", new XAttribute("value", VOX)),
        new XElement("AMK", new XAttribute("value", AMK)),
        new XElement("SKILLS", new XAttribute("value", SKILLS))
    );
}

// PscSpecEntry
public class PscSpecEntry
{
    public string CharId;
    public string CostumeId;
    public Dictionary<string, string> Entries = new Dictionary<string, string>();

    public static PscSpecEntry FromXElement(XElement e)
    {
        var entry = new PscSpecEntry
        {
            CharId = e.Element("CHAR_ID")?.Attribute("value")?.Value,
            CostumeId = e.Element("COSTUME_ID")?.Attribute("value")?.Value,
            Entries = e.Elements("Entry")
                .Where(x => x.Attribute("id") != null)
                .ToDictionary(
                    x => x.Attribute("name")?.Value ?? x.Attribute("id")?.Value,
                    x => x.Value
                )
        };

        return entry;
    }

    public XElement ToXElement()
    {
        var elements = new List<XElement>
        {
            new XElement("CHAR_ID", new XAttribute("value", CharId)),
            new XElement("COSTUME_ID", new XAttribute("value", CostumeId))
        };

        foreach (var kv in Entries)
        {
            elements.Add(new XElement("Entry",
                new XAttribute("id", kv.Key),
                new XAttribute("name", kv.Key),
                kv.Value));
        }

        return new XElement("PscSpecEntry", elements);
    }
}
