using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using XV2INSNX.Properties;

namespace XV2INSNX
{

    public partial class Form1 : Form
    {
        public static X2MFile file;
        public static int cmsID;
        public static string serverRoot = Application.StartupPath + @"/server";
        public static string dataPath = serverRoot + @"/data";
        public static string AppZipFile = Application.StartupPath + @"/AppZip.zip";
        public static string ServerCPKPath = Application.StartupPath + @"/server.cpk";
        public static string TempDir = Application.StartupPath + @"/temp";
        public Form1()
        {
            InitializeComponent();
        }
        private void installModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "X2M Files|*.x2m"
            };

            if (ofd.ShowDialog() != DialogResult.OK) return;

            // Estrazione archivio
            if(!Directory.Exists(TempDir))
                Directory.CreateDirectory(TempDir);
            else
            {
                Directory.Delete(TempDir, true);
                Directory.CreateDirectory(TempDir);
            }
            ZipFile.ExtractToDirectory(ofd.FileName, TempDir);
            string xmlPath = Path.Combine(TempDir, "x2m.xml");
            string xmlFile = File.ReadAllText(xmlPath);
            file = X2MFile.Load(xmlPath);

            if (file.Type != "NEW_CHARACTER")
                return;
            cmsID += 1;

            // Copia dei file nella cartella chara/
            string charaSrc = Path.Combine(TempDir, file.EntryName);
            string charaDst = Path.Combine(dataPath, "chara", file.EntryName);

            if(!Directory.Exists(charaDst))
                Directory.CreateDirectory(charaDst);
            if (Directory.Exists(charaSrc))
                MergeDirectoriesWithConfirmation(charaSrc, charaDst);

            // UI Texture
            string selDDS = Path.Combine(TempDir, "UI", "SEL.DDS");
            if (!Directory.Exists(Path.Combine(dataPath, "ui", "texture", "CHARA01")))
                Directory.CreateDirectory(Path.Combine(dataPath, "ui", "texture", "CHARA01"));
            if (File.Exists(selDDS))
            {
                string selDst = Path.Combine(dataPath, "ui", "texture", "CHARA01", $"{file.EntryName}_000.dds");
                File.Move(selDDS, selDst);
            }

            RunProcess(dataPath + @"/ui/texture/embPack.exe", Path.Combine(dataPath, "ui", "texture", "CHARA01"));

            // Aggiungi Entry CMS
            AddEntryToXmlFile(
                Path.Combine(dataPath, "system", "char_model_spec.cms"),
                xmlFile,
                "Entry",
                "char_model_spec.cms.xml",
                "CMS"
            );

            // Aggiungi Entry CSO
            AddEntryToXmlFile(
                Path.Combine(dataPath, "system", "chara_sound.cso"),
                xmlFile,
                "CsoEntry",
                "chara_sound.cso.xml",
                "CSO"
            );

            // Aggiungi Entry PSC
            string pscPath = Path.Combine(dataPath, "system", "parameter_spec_char.psc");
            string pscXml = ExtractXmlBlock(xmlFile, "PscSpecEntry");
            if (pscXml != null)
            {
                pscXml = pscXml.Replace("0xbacabaca", "0x0");

                RunProcess(dataPath + @"/system/genser.exe", pscPath);
                string content = File.ReadAllText(pscPath + ".xml");

                string closingTag = "    </Configuration>";
                string pscEntry = $"<PscEntry char_id=\"0x{cmsID:X2}\">\n{pscXml}\n</PscEntry>\n";

                // Trova tutti gli indici delle chiusure
                List<int> insertPositions = new List<int>();
                int index = 0;
                while ((index = content.IndexOf(closingTag, index)) != -1)
                {
                    insertPositions.Add(index);
                    index += closingTag.Length;
                }

                // Inserisci dal fondo
                for (int i = insertPositions.Count - 1; i >= 0; i--)
                {
                    int insertIndex = insertPositions[i];
                    content = content.Insert(insertIndex, pscEntry);
                }

                string xmlOut = pscPath + ".xml";
                File.WriteAllText(xmlOut, content);
                RunProcess(dataPath + @"/system/genser.exe", xmlOut);
            }


            // CST
            AddCSTEntry(Path.Combine(dataPath, "system", "chara_select_table.cst"), file);

            // AUR
            AddAUREntry(Path.Combine(dataPath, "system", "aura_setting.aur"), file);

            // CUS
            AddCUSEntry(Path.Combine(dataPath, "system", "custom_skill.cus"), file);

            // QXD
            AddQXDEntry(Path.Combine(dataPath, "quest", "CHQ", "chq_data.qxd"), file);

            // MSG
            string msgPath = Path.Combine(dataPath, "msg", $"proper_noun_character_name_{Settings.Default.language}.msg");
            MSG msgFile = MSGHandler.Read(msgPath);
            var entries = msgFile.data.ToList();

            int lastId = entries.Count;

            entries.Add(new MSGEntry
            {
                ID = lastId,
                NameID = $"chara_{file.EntryName}_000",
                Lines = new[] { file.ModName }
            });

            msgFile.data = entries.ToArray();
            MSGHandler.Write(msgFile, msgPath);

            listBox1.Items.Add(file.ModName);

            // Pulizia
            Directory.Delete(TempDir, true);
            SaveCPK();
        }

        private static void RunProcess(string exePath, string args)
        {
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = exePath,
                        Arguments = args,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    process.Start();
                    process.WaitForExit(); // Aspetta che il processo finisca
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore avviando {exePath}:\n{ex.Message}");
            }
        }

        private static void AddEntryToXmlFile(string binaryFilePath, string sourceXml, string tagName, string outputXmlName, string rootTag)
        {
            string xmlEntry = ExtractXmlBlock(sourceXml, tagName);
            if (xmlEntry == null)
            {
                Console.WriteLine($"Blocco <{tagName}> non trovato.");
                return;
            }

            // Sostituzioni dei placeholder
            xmlEntry = xmlEntry
                .Replace("123", file.EntryName)
                .Replace("0xbacabaca", $"0x{cmsID:X2}");

            RunProcess(dataPath + @"/system/genser.exe", binaryFilePath);

            string xmlPath = binaryFilePath + ".xml";
            string content = File.ReadAllText(xmlPath);
            string closingTag = $"</{rootTag}>";

            int insertIndex = content.LastIndexOf(closingTag);
            if (insertIndex == -1) return;

            string modified = content.Insert(insertIndex, "\n" + xmlEntry);
            File.WriteAllText(xmlPath, modified);

            RunProcess(dataPath + @"/system/genser.exe", xmlPath);
        }

        private static string ExtractXmlBlock(string xml, string tag)
        {
            var match = Regex.Match(xml, $@"<{tag}\b[^>]*>[\s\S]*?</{tag}>", RegexOptions.Multiline);
            return match.Success ? match.Value : null;
        }

        public static void AddCSTEntry(string filePath, X2MFile x2m)
        {

            var psi = new ProcessStartInfo
            {
                FileName = "server\\data\\system\\XV2XMLSerializer.exe",
                Arguments = "server\\data\\system\\chara_select_table.cst",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(psi);

            process.WaitForExit();
            string file = File.ReadAllText(filePath + ".xml");

            string rawVoices = x2m.SlotEntries[0].Voices.Trim().Replace(",", " ");
            string[] parts = rawVoices.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string voice1 = parts.Length > 0 ? parts[0] : "0x0";
            string voice2 = parts.Length > 1 ? parts[1] : "0x0";

            // Rimozione prefisso "0x" se presente
            voice1 = voice1.ToLower().Replace("0x", "");
            voice2 = voice2.ToLower().Replace("0x", "");

            // Conversione in decimale
            int dec1 = Convert.ToInt32(voice1, 16);
            int dec2 = Convert.ToInt32(voice2, 16);

            file = file.Replace("</CST>",
$@"  <CharaSlot>
    <CharaCostumeSlot CharaCode=""{x2m.EntryName}"" Costume=""{x2m.SlotEntries[0].CostumeIndex}"" Preset=""{x2m.SlotEntries[0].ModelPreset}"">
      <UnlockIndex value=""0"" />
      <flag_gk2 value=""0"" />
      <CssVoice1 value=""{dec1}"" />
      <CssVoice2 value=""{dec2}"" />
      <DlcFlag1 value=""DLC_Def"" />
      <DlcFlag2 value=""0"" />
      <IsCustomCostume value=""0"" />
      <CacIndex value=""-1"" />
      <var_type_after_TU9_order value=""-1"" />
      <I_40 value=""-1"" />
      <I_44 value=""0"" />
      <flag_cgk2 value=""0"" />
      <I_50 value=""0"" />
      <I_52 value=""0"" />
    </CharaCostumeSlot>
  </CharaSlot>
</CST>");

            File.WriteAllText(filePath + ".xml", file);

            psi = new ProcessStartInfo
            {
                FileName = "server\\data\\system\\XV2XMLSerializer.exe",
                Arguments = "server\\data\\system\\chara_select_table.cst.xml",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process = Process.Start(psi);
            process.WaitForExit();
        }

        public static void AddQXDEntry(string filePath, X2MFile x2m)
        {

            var psi = new ProcessStartInfo
            {
                FileName = "server\\data\\system\\XV2XMLSerializer.exe",
                Arguments = "server\\data\\quest\\CHQ\\chq_data.qxd",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(psi);

            process.WaitForExit();
            string file = File.ReadAllText(filePath + ".xml");


            file = file.Replace("  </NormalCharacters>\r\n", $"    <Character ID=\"{cmsID}\" Character=\"{x2m.EntryName}\">\r\n      <Costume_Index value=\"0\" />\r\n      <I_12 value=\"0\" />\r\n      <Level value=\"1\" />\r\n      <Health value=\"-1.0\" />\r\n      <Stamina_Armour value=\"-1.0\" />\r\n      <Ki value=\"-1.0\" />\r\n      <Stamina value=\"-1.0\" />\r\n      <Basic_Melee value=\"-1.0\" />\r\n      <Ki_Blast value=\"-1.0\" />\r\n      <Strike_Super value=\"-1.0\" />\r\n      <Ki_Super value=\"-1.0\" />\r\n      <Basic_Melee_Damage value=\"-1.0\" />\r\n      <Ki_Blast_Damage value=\"-1.0\" />\r\n      <Strike_Super_Damage value=\"-1.0\" />\r\n      <Ki_Super_Damage value=\"-1.0\" />\r\n      <F_68 value=\"-1.0\" />\r\n      <F_72 value=\"-1.0\" />\r\n      <Air_Speed value=\"-1.0\" />\r\n      <Boost_Speed value=\"-1.0\" />\r\n      <AI_Table ID=\"0\" />\r\n      <Transformation value=\"-1\" />\r\n      <Super_Soul value=\"65535\" />\r\n      <I_106 values=\"0, 0, 0, 0, 0, 0, 0\" />\r\n      <I_124 value=\"65535\" />\r\n      <I_126 value=\"0\" />\r\n      <Skills>\r\n        <Super_1 ID2=\"65535\" />\r\n        <Super_2 ID2=\"65535\" />\r\n        <Super_3 ID2=\"65535\" />\r\n        <Super_4 ID2=\"65535\" />\r\n        <Ultimate_1 ID2=\"65535\" />\r\n        <Ultimate_2 ID2=\"65535\" />\r\n        <Evasive ID2=\"65535\" />\r\n        <Blast_Type ID2=\"65535\" />\r\n        <Awoken ID2=\"65535\" />\r\n      </Skills>\r\n    </Character>\r\n  </NormalCharacters>");

            File.WriteAllText(filePath + ".xml", file);

            psi = new ProcessStartInfo
            {
                FileName = "server\\data\\system\\XV2XMLSerializer.exe",
                Arguments = "server\\data\\quest\\CHQ\\chq_data.qxd.xml",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process = Process.Start(psi);
            process.WaitForExit();
        }
        public static void AddAUREntry(string filePath, X2MFile x2m)
        {

            var psi = new ProcessStartInfo
            {
                FileName = "server\\data\\system\\XV2XMLSerializer.exe",
                Arguments = "server\\data\\system\\aura_setting.aur",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(psi);

            process.WaitForExit();
            string file = File.ReadAllText(filePath + ".xml");


            file = file.Replace("  </CharacterAuras>\r\n</AUR>", $"    <CharacterAura Chara_ID=\"{cmsID}\" Costume=\"0\" Aura_ID=\"0\" Glare=\"true\" />\r\n  </CharacterAuras>\r\n</AUR>");

            File.WriteAllText(filePath + ".xml", file);

            psi = new ProcessStartInfo
            {
                FileName = "server\\data\\system\\XV2XMLSerializer.exe",
                Arguments = "server\\data\\system\\aura_setting.aur.xml",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process = Process.Start(psi);
            process.WaitForExit();
        }

        public static void AddCUSEntry(string filePath, X2MFile x2m)
        {

            var psi = new ProcessStartInfo
            {
                FileName = "server\\data\\system\\XV2XMLSerializer.exe",
                Arguments = "server\\data\\system\\custom_skill.cus",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(psi);

            process.WaitForExit();
            string file = File.ReadAllText(filePath + ".xml");

            string[] values = x2m.SkillSets[0].Skills.Split(',')
                                   .Select(v => v.Trim())
                                   .ToArray();


            file = file.Replace("  </Skillsets>", $"    <Skillset Character_ID=\"{cmsID}\" Costume_Index=\"0\" Model_Preset=\"0\">\r\n      <SuperSkill1 ID1=\"{values[0]}\" />\r\n      <SuperSkill2 ID1=\"{values[1]}\" />\r\n      <SuperSkill3 ID1=\"{values[2]}\" />\r\n      <SuperSkill4 ID1=\"{values[3]}\" />\r\n      <UltimateSkill1 ID1=\"{values[4]}\" />\r\n      <UltimateSkill2 ID1=\"{values[5]}\" />\r\n      <EvasiveSkill ID1=\"{values[6]}\" />\r\n      <BlastType ID1=\"{values[7]}\" />\r\n      <AwokenSkill ID1=\"{values[8]}\" />\r\n    </Skillset>\r\n  </Skillsets>");

            File.WriteAllText(filePath + ".xml", file);

            psi = new ProcessStartInfo
            {
                FileName = "server\\data\\system\\XV2XMLSerializer.exe",
                Arguments = "server\\data\\system\\custom_skill.cus.xml",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process = Process.Start(psi);
            process.WaitForExit();
        }
        public static void MergeDirectoriesWithConfirmation(string sourceDir, string destDir)
        {
            if (!Directory.Exists(sourceDir) || !Directory.Exists(destDir))
            {
                throw new DirectoryNotFoundException("Source or destination directory does not exist.");
            }

            string[] sourceSubDirs = Directory.GetDirectories(sourceDir);

            foreach (string sourceFile in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(sourceFile);
                string destFile = Path.Combine(destDir, fileName);

                if (File.Exists(destFile))
                {
                    var result = MessageBox.Show($"A file with the name '{fileName}' already exists. Do you want to replace it?", "File Replace Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        File.Copy(sourceFile, destFile, true); // Replace the existing file.
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        return; // Cancel the entire operation.
                    }
                }
                else
                {
                    File.Copy(sourceFile, destFile);
                }
            }

            foreach (string sourceSubDir in sourceSubDirs)
            {
                string dirName = Path.GetFileName(sourceSubDir);
                string destSubDir = Path.Combine(destDir, dirName);
                if (!Directory.Exists(destSubDir))
                {
                    Directory.CreateDirectory(destSubDir);
                }
                MergeDirectoriesWithConfirmation(sourceSubDir, destSubDir);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Settings.Default.modList.Contains("test.DELETEME12345"))
                Settings.Default.modList.Clear();
            listBox1.Items.Clear();
            foreach(string item in Settings.Default.modList)
            {
                listBox1.Items.Add(item);
            }
            if(!Directory.Exists(serverRoot) && !Directory.Exists(Application.StartupPath + @"/CPKTools"))
                ZipFile.ExtractToDirectory(AppZipFile, Application.StartupPath);
            cmsID = 300 + Settings.Default.modList.Count;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(Directory.Exists(TempDir))
                Directory.Delete(TempDir, true);
            Settings.Default.modList.Clear();
            foreach (string item in listBox1.Items)
            {
                Settings.Default.modList.Add(item);
            }
            Settings.Default.Save();
        }

        private void clearInstallationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Settings.Default.modList.Clear();
            Settings.Default.Reset();
            Settings.Default.Save();
            if (Directory.Exists(serverRoot))
            {
                Directory.Delete(serverRoot, true);
            }
            if (Directory.Exists(Application.StartupPath + @"/CPKTools"))
            {
                Directory.Delete(Application.StartupPath + @"/CPKTools", true);
            }
            this.Close();
        }

        private void changeLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.ShowDialog();
        }
        private void SaveCPK()
        {
            if (File.Exists(Application.StartupPath + @"/server.cpk"))
                File.Delete(Application.StartupPath + @"/server.cpk");
            var psi = new ProcessStartInfo
            {
                FileName = "CPKTools\\cpkmakec.exe",
                Arguments = "\"server\" \"server.cpk\" -align=512 -mode=FILENAME -code=UTF-8 -nodatetime",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(psi);

            process.WaitForExit();
        }
        private void produceCPKFinalizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCPK();
        }
    }
}
