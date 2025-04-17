using FreeImageAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Security;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using XV2Reborn.Properties;
using XV2Reborn.XV;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace XV2Reborn
{

    public partial class Form1 : Form
    {
        string language = "";

        public Form1()
        {
            InitializeComponent();

            ContextMenuStrip contextMenu = new ContextMenuStrip();

            ToolStripMenuItem uninstallMenuItem = new ToolStripMenuItem("Uninstall Mod");

            contextMenu.Items.Add(uninstallMenuItem);

            lvMods.ContextMenuStrip = contextMenu;

            uninstallMenuItem.Click += (sender, e) =>
            {
                if (lvMods.SelectedItems.Count > 0)
                {
                    ListViewItem selectedItem = lvMods.SelectedItems[0];

                    uninstallMod(sender, e);
                }
                else
                {
                    MessageBox.Show("No mod selected for uninstallation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            lvMods.MouseUp += (sender, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    ListViewItem itemUnderCursor = lvMods.GetItemAt(e.X, e.Y);

                    if (itemUnderCursor != null)
                    {
                        lvMods.SelectedItems.Clear();
                        itemUnderCursor.Selected = true; 
                        contextMenu.Show(lvMods, e.Location);
                    }
                }
            };

        }

        public void Form1_Load(object sender, EventArgs e)
        {
            

            if (Settings.Default.language.Length == 0)
            {
                Form2 frm = new Form2();
                frm.ShowDialog();
                language = Settings.Default.language;
            }
            else
            {
                language = Settings.Default.language;
            }

            if (Properties.Settings.Default.datafolder.Length == 0)
            {
                Settings.Default.datafolder = System.Windows.Forms.Application.StartupPath + @"/server/data";
                Settings.Default.Save();

                var myAssembly = Assembly.GetExecutingAssembly();
                var myStream = myAssembly.GetManifestResourceStream("XV2Reborn.ZipFile_Blobs.AppZip.zip");
                ZipArchive archive = new ZipArchive(myStream);
                archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\..\..\"));

            }
            else
            {
                if (!Directory.Exists(Properties.Settings.Default.datafolder))
                    MessageBox.Show("Data Folder not Found, Please Clear Installation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (Properties.Settings.Default.modlist.Contains("System.Object"))
            {
                Properties.Settings.Default.modlist.Clear();
            }

            if (Properties.Settings.Default.addonmodlist.Contains("System.Object"))
            {
                Properties.Settings.Default.addonmodlist.Clear();
            }

            loadLvItems();
            Clean();
        }
        private void Clean()
        {
            if (File.Exists(Properties.Settings.Default.datafolder + "//modinfo.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + "//modinfo.xml");
            }

            if (Directory.Exists(Properties.Settings.Default.datafolder + "//temp"))
            {
                Directory.Delete(Properties.Settings.Default.datafolder + "//temp", true);
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml.bak"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml.bak");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml.bak"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml.bak");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml.bak"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml.bak");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml.bak"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml.bak");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml.bak"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml.bak");
            }
            /*
            if (File.Exists(Properties.Settings.Default.datafolder + @"\quest\TMQ\tmq_data.qxd.bak"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\quest\TMQ\tmq_data.qxd.bak");
            }
            */

            if (Directory.Exists("./XV2RebornTemp"))
                Directory.Delete("./XV2RebornTemp", true);
            if (File.Exists(Settings.Default.datafolder + "\\x2m.xml"))
                File.Delete(Settings.Default.datafolder + "\\x2m.xml");

            RemoveEmptyDirectories(Settings.Default.datafolder);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Clean();
        }



        private void clearInstallationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear installation?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (Directory.Exists(Properties.Settings.Default.datafolder + "/../"))
                    Directory.Delete(Properties.Settings.Default.datafolder + "/../", true);

                if (Directory.Exists("./XV2RebornTemp"))
                    Directory.Delete("./XV2RebornTemp", true);

                if (Directory.Exists("./CPKTools"))
                    Directory.Delete("./CPKTools", true);



                Properties.Settings.Default.Reset();
                MessageBox.Show("Installation cleared, XV2Reborn will now close", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                return;
            }
        }

        private void saveLvItems()
        {
            Properties.Settings.Default.modlist = new StringCollection();
            Properties.Settings.Default.modlist.AddRange((from i in this.lvMods.Items.Cast<ListViewItem>()
                                                          select string.Join("|", from si in i.SubItems.Cast<ListViewItem.ListViewSubItem>()
                                                                                  select si.Text)).ToArray());
            Properties.Settings.Default.Save();
            label1.Text = "Installed Mods: " + lvMods.Items.Count.ToString();
        }

        private void loadLvItems()
        {
            if (Properties.Settings.Default.modlist == null)
            {
                Properties.Settings.Default.modlist = new StringCollection();
            }

            this.lvMods.Items.AddRange((from i in Properties.Settings.Default.modlist.Cast<string>()
                                        select new ListViewItem(i.Split('|'))).ToArray());

            label1.Text = "Installed Mods: " + lvMods.Items.Count.ToString();
        }
        private List<string> EnumerateFiles(string directory)
        {
            List<string> files = new List<string>();
            foreach (string file in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
            {
                files.Add(file);
            }
            return files;
        }

        private void installmod(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Install Mod";
            ofd.Filter = "Xenoverse Mod Files (*.xv2mod)|*.xv2mod";
            ofd.Multiselect = true;
            ofd.CheckFileExists = true;

            string modtype = "";
            string modname = "";
            string modauthor = "";
            string modversion = "";
            int AUR_ID = 0;
            int AUR_GLARE = 0;
            string CMS_BCS = "";
            string CMS_EAN = "";
            string CMS_FCE_EAN = "";
            string CMS_CAM_EAN = "";
            string CMS_BAC = "";
            string CMS_BCM = "";
            string CMS_BAI = "";
            string CSO_1 = "";
            string CSO_2 = "";
            string CSO_3 = "";
            string CSO_4 = "";
            string CUS_SUPER_1 = "";
            string CUS_SUPER_2 = "";
            string CUS_SUPER_3 = "";
            string CUS_SUPER_4 = "";
            string CUS_ULTIMATE_1 = "";
            string CUS_ULTIMATE_2 = "";
            string CUS_EVASIVE = "";
            string CUS_KI_BLAST = "";
            string CUS_AWOKEN = "";
            string PSC_COSTUME = "";
            string PSC_PRESET = "";
            string PSC_CAMERA_POSITION = "";
            string PSC_I_12 = "";
            string PSC_I_16 = "";
            string PSC_HEALTH = "";
            string PSC_F_24 = "";
            string PSC_KI = "";
            string PSC_KI_RECHARGE = "";
            string PSC_I_36 = "";
            string PSC_I_40 = "";
            string PSC_I_44 = "";
            string PSC_STAMINA = "";
            string PSC_STAMINA_RECHARGE_MOVE = "";
            string PSC_STAMINA_RECHARGE_AIR = "";
            string PSC_STAMINA_RECHARGE_GROUND = "";
            string PSC_STAMINA_DRAIN_RATE_1 = "";
            string PSC_STAMINA_DRAIN_RATE_2 = "";
            string PSC_F_72 = "";
            string PSC_BASIC_ATK = "";
            string PSC_BASIC_KI_ATK = "";
            string PSC_STRIKE_ATK = "";
            string PSC_SUPER_KI_BLAST_ATK = "";
            string PSC_BASIC_ATK_DEF = "";
            string PSC_BASIC_KI_DEF = "";
            string PSC_STRIKE_ATK_DEF = "";
            string PSC_SUPER_KI_DEF = "";
            string PSC_GROUND_SPEED = "";
            string PSC_AIR_SPEED = "";
            string PSC_BOOST_SPEED = "";
            string PSC_DASH_SPEED = "";
            string PSC_F_124 = "";
            string PSC_REINFORCEMENT_SKILL = "";
            string PSC_F_132 = "";
            string PSC_REVIVAL_HP_AMOUNT = "";
            string PSC_F_140 = "";
            string PSC_REVIVING_SPEED = "";
            string PSC_I_148 = "";
            string PSC_I_152 = "";
            string PSC_I_156 = "";
            string PSC_I_160 = "";
            string PSC_I_164 = "";
            string PSC_I_168 = "";
            string PSC_I_172 = "";
            string PSC_I_176 = "";
            string PSC_SUPER_SOUL = "";
            string PSC_I_184 = "";
            string PSC_I_188 = "";
            string PSC_F_192 = "";
            string PSC_NEW_I_20 = "";


            string MSG_CHARACTER_NAME = "";
            string MSG_COSTUME_NAME = "";
            short VOX_1 = -1;
            short VOX_2 = -1;
            string SKILL_ShortName = "";
            string SKILL_ID1 = "";
            string SKILL_ID2 = "";
            string SKILL_I_04 = "";
            string SKILL_Race_Lock = "";
            string SKILL_Type = "";
            string SKILL_FilesLoaded = "";
            string SKILL_PartSet = "";
            string SKILL_I_18 = "";
            string SKILL_EAN = "";
            string SKILL_CAM_EAN = "";
            string SKILL_EEPK = "";
            string SKILL_ACB_SE = "";
            string SKILL_ACB_VOX = "";
            string SKILL_AFTER_BAC = "";
            string SKILL_AFTER_BCM = "";
            string SKILL_I_48 = "";
            string SKILL_I_50 = "";
            string SKILL_I_52 = "";
            string SKILL_I_54 = "";
            string SKILL_PUP = "";
            string SKILL_CUS_Aura = "";
            string SKILL_TransformCharaSwap = "";
            string SKILL_Skillset_Change = "";
            string SKILL_Num_Of_Transforms = "";
            string SKILL_I_66 = "";

            // The existing values
            string SKILL_TYPE = "";
            string MSG_SKILL_NAME = "";
            string MSG_SKILL_DESC = "";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in ofd.FileNames)
                {
                    string xmlfile = "./XV2RebornTemp" + @"/xv2mod.xml";
                    int CharID = 300 + Settings.Default.modlist.Count;

                    if (File.Exists(Settings.Default.datafolder + @"/xv2mod.xml"))
                        File.Delete(Settings.Default.datafolder + @"/xv2mod.xml");
                    if (File.Exists(xmlfile))
                        File.Delete(xmlfile);
                    ZipFile.ExtractToDirectory(file, "./XV2RebornTemp");

                    if (!File.Exists(xmlfile))
                    {
                        MessageBox.Show("xv2mod.xml file not found.",
                        "Invalid mod file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }


                    string xmlData = File.ReadAllText(xmlfile);

                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlData)))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {

                                if (reader.Name == "XV2MOD")
                                {
                                    if (reader.GetAttribute("type").Length == 0)
                                    {
                                        MessageBox.Show("Invalid xmlreader attribute.",
                                        "Invalid mod file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                    modtype = reader.GetAttribute("type");

                                }

                                if (reader.Name == "XV2MOD")
                                {
                                    if (reader.GetAttribute("type").Length == 0)
                                    {
                                        MessageBox.Show("Invalid xmlreader attribute.", "Invalid mod file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    modtype = reader.GetAttribute("type");
                                }
                                if (reader.Name == "MOD_NAME")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        modname = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "MOD_AUTHOR")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        modauthor = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "MOD_VERSION")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        modversion = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "AUR_ID")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        bool parseSuccess = Int32.TryParse(reader.GetAttribute("value").Trim(), out AUR_ID);
                                        if (!parseSuccess)
                                        {
                                            // Gestisci il caso in cui la conversione non riesce, ad esempio, fornisci un valore predefinito o mostra un messaggio di errore.
                                            MessageBox.Show("AUR_ID value not recognized", "Error", MessageBoxButtons.OK);
                                            return;
                                        }
                                    }
                                }
                                if (reader.Name == "AUR_GLARE")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        bool parseSuccess = Int32.TryParse(reader.GetAttribute("value").Trim(), out AUR_GLARE);
                                        if (!parseSuccess)
                                        {
                                            // Gestisci il caso in cui la conversione non riesce, ad esempio, fornisci un valore predefinito o mostra un messaggio di errore.
                                            MessageBox.Show("AUR_GLARE value not recognized", "Error", MessageBoxButtons.OK);
                                            return;
                                        }
                                    }
                                }
                                if (reader.Name == "CMS_BCS")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_BCS = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CMS_EAN")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_EAN = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CMS_FCE_EAN")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_FCE_EAN = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CMS_CAM_EAN")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_CAM_EAN = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CMS_BAC")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_BAC = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CMS_BCM")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_BCM = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CMS_BAI")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_BAI = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CSO_1")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CSO_1 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CSO_2")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CSO_2 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CSO_3")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CSO_3 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CSO_4")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CSO_4 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_SUPER_1")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_SUPER_1 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_SUPER_2")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_SUPER_2 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_SUPER_3")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_SUPER_3 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_SUPER_4")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_SUPER_4 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_ULTIMATE_1")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_ULTIMATE_1 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_ULTIMATE_2")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_ULTIMATE_2 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_EVASIVE")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_EVASIVE = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_KI_BLAST")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_KI_BLAST = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_AWOKEN")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_AWOKEN = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_COSTUME" && reader.HasAttributes)
                                {
                                    PSC_COSTUME = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_PRESET" && reader.HasAttributes)
                                {
                                    PSC_PRESET = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_CAMERA_POSITION" && reader.HasAttributes)
                                {
                                    PSC_CAMERA_POSITION = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_I_12" && reader.HasAttributes)
                                {
                                    PSC_I_12 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_I_16" && reader.HasAttributes)
                                {
                                    PSC_I_16 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_HEALTH" && reader.HasAttributes)
                                {
                                    PSC_HEALTH = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_F_24" && reader.HasAttributes)
                                {
                                    PSC_F_24 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_KI" && reader.HasAttributes)
                                {
                                    PSC_KI = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_KI_RECHARGE" && reader.HasAttributes)
                                {
                                    PSC_KI_RECHARGE = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_I_36" && reader.HasAttributes)
                                {
                                    PSC_I_36 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_I_40" && reader.HasAttributes)
                                {
                                    PSC_I_40 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_I_44" && reader.HasAttributes)
                                {
                                    PSC_I_44 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_STAMINA" && reader.HasAttributes)
                                {
                                    PSC_STAMINA = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_STAMINA_RECHARGE_MOVE" && reader.HasAttributes)
                                {
                                    PSC_STAMINA_RECHARGE_MOVE = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_STAMINA_RECHARGE_AIR" && reader.HasAttributes)
                                {
                                    PSC_STAMINA_RECHARGE_AIR = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_STAMINA_RECHARGE_GROUND" && reader.HasAttributes)
                                {
                                    PSC_STAMINA_RECHARGE_GROUND = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_STAMINA_DRAIN_RATE_1" && reader.HasAttributes)
                                {
                                    PSC_STAMINA_DRAIN_RATE_1 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_STAMINA_DRAIN_RATE_2" && reader.HasAttributes)
                                {
                                    PSC_STAMINA_DRAIN_RATE_2 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_F_72" && reader.HasAttributes)
                                {
                                    PSC_F_72 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_BASIC_ATK" && reader.HasAttributes)
                                {
                                    PSC_BASIC_ATK = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_BASIC_KI_ATK" && reader.HasAttributes)
                                {
                                    PSC_BASIC_KI_ATK = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_STRIKE_ATK" && reader.HasAttributes)
                                {
                                    PSC_STRIKE_ATK = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_SUPER_KI_BLAST_ATK" && reader.HasAttributes)
                                {
                                    PSC_SUPER_KI_BLAST_ATK = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_BASIC_ATK_DEF" && reader.HasAttributes)
                                {
                                    PSC_BASIC_ATK_DEF = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_BASIC_KI_DEF" && reader.HasAttributes)
                                {
                                    PSC_BASIC_KI_DEF = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_STRIKE_ATK_DEF" && reader.HasAttributes)
                                {
                                    PSC_STRIKE_ATK_DEF = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_SUPER_KI_DEF" && reader.HasAttributes)
                                {
                                    PSC_SUPER_KI_DEF = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_GROUND_SPEED" && reader.HasAttributes)
                                {
                                    PSC_GROUND_SPEED = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_AIR_SPEED" && reader.HasAttributes)
                                {
                                    PSC_AIR_SPEED = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_BOOST_SPEED" && reader.HasAttributes)
                                {
                                    PSC_BOOST_SPEED = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_DASH_SPEED" && reader.HasAttributes)
                                {
                                    PSC_DASH_SPEED = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_F_124" && reader.HasAttributes)
                                {
                                    PSC_F_124 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_REINFORCEMENT_SKILL" && reader.HasAttributes)
                                {
                                    PSC_REINFORCEMENT_SKILL = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_F_132" && reader.HasAttributes)
                                {
                                    PSC_F_132 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_REVIVAL_HP_AMOUNT" && reader.HasAttributes)
                                {
                                    PSC_REVIVAL_HP_AMOUNT = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_F_140" && reader.HasAttributes)
                                {
                                    PSC_F_140 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_REVIVING_SPEED" && reader.HasAttributes)
                                {
                                    PSC_REVIVING_SPEED = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_I_148" && reader.HasAttributes)
                                {
                                    PSC_I_148 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_I_152" && reader.HasAttributes)
                                {
                                    PSC_I_152 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_I_156" && reader.HasAttributes)
                                {
                                    PSC_I_156 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_I_160" && reader.HasAttributes)
                                {
                                    PSC_I_160 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_I_164" && reader.HasAttributes)
                                {
                                    PSC_I_164 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_I_168" && reader.HasAttributes)
                                {
                                    PSC_I_168 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_I_172" && reader.HasAttributes)
                                {
                                    PSC_I_172 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_I_176" && reader.HasAttributes)
                                {
                                    PSC_I_176 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_SUPER_SOUL" && reader.HasAttributes)
                                {
                                    PSC_SUPER_SOUL = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_I_184" && reader.HasAttributes)
                                {
                                    PSC_I_184 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_I_188" && reader.HasAttributes)
                                {
                                    PSC_I_188 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_F_192" && reader.HasAttributes)
                                {
                                    PSC_F_192 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "PSC_NEW_I_20" && reader.HasAttributes)
                                {
                                    PSC_NEW_I_20 = reader.GetAttribute("value").Trim();
                                }
                                if (reader.Name == "MSG_CHARACTER_NAME")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        MSG_CHARACTER_NAME = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "MSG_COSTUME_NAME")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        MSG_COSTUME_NAME = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "VOX_1")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        bool parseSuccess = Int16.TryParse(reader.GetAttribute("value").Trim(), out VOX_1);
                                        if (!parseSuccess)
                                        {
                                            MessageBox.Show("VOX_1 value not recognized", "Error", MessageBoxButtons.OK);
                                            return;
                                        }
                                    }
                                }
                                if (reader.Name == "VOX_2")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        bool parseSuccess = Int16.TryParse(reader.GetAttribute("value").Trim(), out VOX_2);
                                        if (!parseSuccess)
                                        {
                                            MessageBox.Show("VOX_2 value not recognized", "Error", MessageBoxButtons.OK);
                                            return;
                                        }
                                    }
                                }

                                if(reader.Name == "MSG_SKILL_NAME")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        MSG_SKILL_NAME = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "MSG_SKILL_DESC")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        MSG_SKILL_DESC = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "SKILL_TYPE")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_TYPE = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "ShortName")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_ShortName = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "ID1")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_ID1 = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "ID2")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_ID2 = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "I_04")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_I_04 = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "Race_Lock")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_Race_Lock = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "Type")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_Type = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "FilesLoaded")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_FilesLoaded = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "PartSet")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_PartSet = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "I_18")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_I_18 = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "EAN")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_EAN = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "CAM_EAN")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_CAM_EAN = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "EEPK")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_EEPK = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "ACB_SE")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_ACB_SE = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "ACB_VOX")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_ACB_VOX = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "AFTER_BAC")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_AFTER_BAC = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "AFTER_BCM")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_AFTER_BCM = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "I_48")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_I_48 = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "I_50")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_I_50 = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "I_52")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_I_52 = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "I_54")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_I_54 = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "PUP")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_PUP = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "CUS_Aura")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_CUS_Aura = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "TransformCharaSwap")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_TransformCharaSwap = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "Skillset_Change")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_Skillset_Change = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "Num_Of_Transforms")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_Num_Of_Transforms = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "I_66")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_I_66 = reader.GetAttribute("value").Trim();
                                    }
                                }
                            }
                        }
                        if (File.Exists(xmlfile))
                            File.Delete(xmlfile);

                    }
                    if (modtype == "REPLACER")
                    {
                        List<string> installedFiles = EnumerateFiles("./XV2RebornTemp");
                        List<string> final = new List<string>();

                        foreach (string installedFile in installedFiles)
                        {
                            // Correctly replace the path and add it to the list
                            string modifiedFile = installedFile.Replace("./XV2RebornTemp", Properties.Settings.Default.datafolder);
                            final.Add(modifiedFile);
                        }

                        // Ensure the target directory exists
                        string installedDir = Path.Combine(Settings.Default.datafolder, "installed");
                        if (!Directory.Exists(installedDir))
                        {
                            Directory.CreateDirectory(installedDir);
                        }

                        // Write the modified paths to a file
                        string filePath = Path.Combine(installedDir, $"{modtype}_{modname}.txt");
                        File.WriteAllLines(filePath, final);

                        DirectoryUtils.MergeDirectoriesWithConfirmation("./XV2RebornTemp", Settings.Default.datafolder);

                        Clean();


                        string[] row = { modname, modauthor, "Replacer" };
                        ListViewItem lvi = new ListViewItem(row);
                        lvMods.Items.Add(lvi);
                        saveLvItems();
                    }
                    else if (modtype == "ADDED_CHARACTER")
                    {
                        List<string> installedFiles = EnumerateFiles("./XV2RebornTemp");
                        List<string> final = new List<string>();

                        foreach (string installedFile in installedFiles)
                        {
                            string modifiedFile = installedFile.Replace("./XV2RebornTemp", Properties.Settings.Default.datafolder);
                            final.Add(modifiedFile);
                        }

                        string installedDir = Path.Combine(Settings.Default.datafolder, "installed");
                        if (!Directory.Exists(installedDir))
                        {
                            Directory.CreateDirectory(installedDir);
                        }

                        string filePath = Path.Combine(Settings.Default.datafolder, Settings.Default.datafolder + $"/installed/{modtype}_{modname}_{CMS_BCS}_{CharID}.txt");
                        File.WriteAllLines(filePath, final);

                       
                        if(Directory.Exists("./XV2RebornTemp/JUNGLE"))
                            DirectoryUtils.MergeDirectoriesWithConfirmation("./XV2RebornTemp/JUNGLE", Settings.Default.datafolder);
                        DirectoryUtils.MergeDirectoriesWithConfirmation("./XV2RebornTemp", Settings.Default.datafolder);

                        Clean();
                        
                       
                        // CSO
                        CSO cso = new CSO();
                        cso.Load(Settings.Default.datafolder + @"/system/chara_sound.cso");
                        CSO_Data characterData = new CSO_Data
                        {
                            Char_ID = CharID,           // Sostituisci con l'ID del personaggio desiderato
                            Costume_ID = 0,      // Sostituisci con l'ID del costume desiderato
                            Paths = new string[4]  // Aggiungi i percorsi desiderati
                            {
                                    CSO_1,
                                    CSO_2,
                                    CSO_3,
                                    CSO_4
                            }
                        };
                        cso.AddCharacter(characterData);

                        // CUS

                        Process p = new Process();
                        ProcessStartInfo info = new ProcessStartInfo();
                        info.FileName = "cmd.exe";
                        info.CreateNoWindow = true;
                        info.WindowStyle = ProcessWindowStyle.Hidden;
                        info.RedirectStandardInput = true;
                        info.UseShellExecute = false;
                        p.StartInfo = info;
                        p.Start();
                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"XMLSerializer.exe custom_skill.cus");
                            }
                        }
                        p.WaitForExit();

                        string cuspath = Settings.Default.datafolder + @"\system\custom_skill.cus.xml";
                        string text4 = File.ReadAllText(cuspath);

                        text4 = text4.Replace("  </Skillsets>", "    <Skillset Character_ID=\"" + CharID + $"\" Costume_Index=\"0\" Model_Preset=\"0\">\r\n      <SuperSkill1 ID1=\"{CUS_SUPER_1}\" />\r\n      <SuperSkill2 ID1=\"{CUS_SUPER_2}\" />\r\n      <SuperSkill3 ID1=\"{CUS_SUPER_3}\" />\r\n      <SuperSkill4 ID1=\"{CUS_SUPER_4}\" />\r\n      <UltimateSkill1 ID1=\"{CUS_ULTIMATE_1}\" />\r\n      <UltimateSkill2 ID1=\"{CUS_ULTIMATE_2}\" />\r\n      <EvasiveSkill ID1=\"{CUS_EVASIVE}\" />\r\n      <BlastType ID1=\"{CUS_KI_BLAST}\" />\r\n      <AwokenSkill ID1=\"{CUS_AWOKEN}\" />\r\n    </Skillset>\r\n  </Skillsets>");
                        File.WriteAllText(cuspath, text4);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"XMLSerializer.exe " + quote + Settings.Default.datafolder + @"\system\custom_skill.cus.xml" + quote);
                            }
                        }

                        p.WaitForExit();

                        ///
                        // AUR
                        info.FileName = "cmd.exe";
                        info.CreateNoWindow = true;
                        info.WindowStyle = ProcessWindowStyle.Hidden;
                        info.RedirectStandardInput = true;
                        info.UseShellExecute = false;
                        p.StartInfo = info;

                        p.Start();
                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"XMLSerializer.exe aura_setting.aur");
                            }
                        }
                        p.WaitForExit();

                        string aurpath = Settings.Default.datafolder + @"\system\aura_setting.aur.xml";
                        string text5 = File.ReadAllText(aurpath);
                        string glare;
                        if (AUR_GLARE == 1)
                        {
                            glare = "True";
                        }
                        else
                        {
                            glare = "False";
                        }
                        text5 = text5.Replace("  </CharacterAuras>", "    <CharacterAura Chara_ID=\"" + CharID + $"\" Costume=\"0\" Aura_ID=\"{AUR_ID}\" Glare=\"{glare}\" />\r\n  </CharacterAuras>");
                        File.WriteAllText(aurpath, text5);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"XMLSerializer.exe " + quote + Settings.Default.datafolder + @"\system\aura_setting.aur.xml" + quote);
                            }
                        }

                        p.WaitForExit();


                        //////

                        // PSC
                        p.Start();
                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"XMLSerializer.exe parameter_spec_char.psc");
                            }
                        }
                        p.WaitForExit();

                        string pscpath = Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml";
                        string text6 = File.ReadAllText(pscpath);

                        text6 = text6.Replace("  </Configuration>\r\n</PSC>",
     "    <PSC_Entry Chara_ID=\"" + CharID + "\">\r\n" +
     "      <PscSpecEntry Costume=\"" + PSC_COSTUME + "\" Costume2=\"" + PSC_PRESET + "\">\r\n" +
     "        <Camera_Position value=\"" + PSC_CAMERA_POSITION + "\" />\r\n" +
     "        <I_12 value=\"" + PSC_I_12 + "\" />\r\n" +
     "        <I_16 value=\"5\" />\r\n" +
     "        <Health value=\"" + PSC_HEALTH + "\" />\r\n" +
     "        <F_24 value=\"" + PSC_F_24 + "\" />\r\n" +
     "        <Ki value=\"" + PSC_KI + "\" />\r\n" +
     "        <Ki_Recharge value=\"" + PSC_KI_RECHARGE + "\" />\r\n" +
     "        <I_36 value=\"" + PSC_I_36 + "\" />\r\n" +
     "        <I_40 value=\"" + PSC_I_36 + "\" />\r\n" +
     "        <I_44 value=\"" + PSC_I_40 + "\" />\r\n" +
     "        <Stamina value=\"" + PSC_STAMINA + "\" />\r\n" +
     "        <Stamina_Recharge_Move value=\"" + PSC_STAMINA_RECHARGE_MOVE + "\" />\r\n" +
     "        <Stamina_Recharge_Air value=\"" + PSC_STAMINA_RECHARGE_AIR + "\" />\r\n" +
     "        <Stamina_Recharge_Ground value=\"" + PSC_STAMINA_RECHARGE_GROUND + "\" />\r\n" +
     "        <Stamina_Drain_Rate_1 value=\"" + PSC_STAMINA_DRAIN_RATE_1 + "\" />\r\n" +
     "        <Stamina_Drain_Rate_2 value=\"" + PSC_STAMINA_DRAIN_RATE_2 + "\" />\r\n" +
     "        <F_72 value=\"" + PSC_F_72 + "\" />\r\n" +
     "        <Basic_Atk value=\"" + PSC_BASIC_ATK + "\" />\r\n" +
     "        <Basic_Ki_Atk value=\"" + PSC_BASIC_KI_ATK + "\" />\r\n" +
     "        <Strike_Atk value=\"" + PSC_STRIKE_ATK + "\" />\r\n" +
     "        <Super_Ki_Blast_Atk value=\"" + PSC_SUPER_KI_BLAST_ATK + "\" />\r\n" +
     "        <Basic_Atk_Defense value=\"" + PSC_BASIC_ATK_DEF + "\" />\r\n" +
     "        <Basic_Ki_Atk_Defense value=\"" + PSC_BASIC_KI_DEF + "\" />\r\n" +
     "        <Strike_Atk_Defense value=\"" + PSC_STRIKE_ATK_DEF + "\" />\r\n" +
     "        <Super_Ki_Blast_Defense value=\"" + PSC_SUPER_KI_DEF + "\" />\r\n" +
     "        <Ground_Speed value=\"" + PSC_GROUND_SPEED + "\" />\r\n" +
     "        <Air_Speed value=\"" + PSC_AIR_SPEED + "\" />\r\n" +
     "        <Boosting_Speed value=\"" + PSC_BOOST_SPEED + "\" />\r\n" +
     "        <Dash_Speed value=\"" + PSC_DASH_SPEED + "\" />\r\n" +
     "        <F_124 value=\"" + PSC_F_124 + "\" />\r\n" +
     "        <Reinforcement_Skill_Duration value=\"" + PSC_REINFORCEMENT_SKILL + "\" />\r\n" +
     "        <F_132 value=\"" + PSC_F_132 + "\" />\r\n" +
     "        <Revival_HP_Amount value=\"" + PSC_REVIVAL_HP_AMOUNT + "\" />\r\n" +
     "        <F_140 value=\"" + PSC_F_140 + "\" />\r\n" +
     "        <Reviving_Speed value=\"" + PSC_REVIVING_SPEED + "\" />\r\n" +
     "        <I_148 value=\"" + PSC_I_148 + "\" />\r\n" +
     "        <I_152 value=\"" + PSC_I_152 + "\" />\r\n" +
     "        <I_156 value=\"" + PSC_I_156 + "\" />\r\n" +
     "        <I_160 value=\"" + PSC_I_160 + "\" />\r\n" +
     "        <I_164 value=\"" + PSC_I_164 + "\" />\r\n" +
     "        <I_168 value=\"" + PSC_I_168 + "\" />\r\n" +
     "        <I_172 value=\"" + PSC_I_172 + "\" />\r\n" +
     "        <I_176 value=\"" + PSC_I_176 + "\" />\r\n" +
     "        <Super_Soul talisman=\"" + PSC_SUPER_SOUL + "\" />\r\n" +
     "        <I_184 value=\"" + PSC_I_184 + "\" />\r\n" +
     "        <I_188 value=\"" + PSC_I_188 + "\" />\r\n" +
     "        <F_192 value=\"" + PSC_F_192 + "\" />\r\n" +
     "        <NEW_I_20 value=\"" + PSC_NEW_I_20 + "\" />\r\n" +
     "      </PscSpecEntry>\r\n" +
     "    </PSC_Entry>\r\n" +
     "  </Configuration>\r\n</PSC>");

                        File.WriteAllText(pscpath, text6);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"XMLSerializer.exe " + quote + Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml" + quote);
                            }
                        }

                        p.WaitForExit();
                        //////


                        //CMS
                        p.Start();
                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"XMLSerializer.exe char_model_spec.cms");
                            }
                        }
                        p.WaitForExit();

                        string cmspath = Settings.Default.datafolder + @"\system\char_model_spec.cms.xml";
                        string text7 = File.ReadAllText(cmspath);

                        text7 = text7.Replace("</CMS>", $"  <Entry ID=\"{CharID}\" ShortName=\"{CMS_BCS}\">\r\n    <I_08 value=\"0x0\" />\r\n    <I_16 value=\"0x4\" />\r\n    <LoadCamDist value=\"0\" />\r\n    <I_22 value=\"0xc801\" />\r\n    <I_24 value=\"0xffff\" />\r\n    <I_26 value=\"0x400\" />\r\n    <I_28 value=\"0x0\" />\r\n    <BCS value=\"{CMS_BCS}\" />\r\n    <EAN value=\"{CMS_EAN}\" />\r\n    <FCE_EAN value=\"{CMS_FCE_EAN}\" />\r\n    <FCE value=\"\" />\r\n    <CAM_EAN value=\"{CMS_CAM_EAN}\" />\r\n    <BAC value=\"{CMS_BAC}\" />\r\n    <BCM value=\"{CMS_BCM}\" />\r\n    <BAI value=\"{CMS_BAI}\" />\r\n    <BDM value=\"{CMS_BCM}\" />\r\n  </Entry>\r\n</CMS>");
                        File.WriteAllText(cmspath, text7);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"XMLSerializer.exe " + quote + Settings.Default.datafolder + @"\system\char_model_spec.cms.xml" + quote);
                            }
                        }

                        p.WaitForExit();

                        //CST
                        var psi = new ProcessStartInfo
                        {
                            FileName = "server\\data\\system\\XMLSerializer.exe",
                            Arguments = "server\\data\\system\\chara_select_table.cst",
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        var process = Process.Start(psi);

                        process.WaitForExit();
                        filePath = "server\\data\\system\\chara_select_table.cst";
                        string fileCST = File.ReadAllText(filePath + ".xml");

                        fileCST = fileCST.Replace("</CST>",
            $@"  <CharaSlot>
    <CharaCostumeSlot CharaCode=""{CMS_BCS}"" Costume=""{PSC_COSTUME}"" Preset=""{PSC_PRESET}"">
      <UnlockIndex value=""0"" />
      <flag_gk2 value=""0"" />
      <CssVoice1 value=""{VOX_1.ToString().Replace("-1", "65535")}"" />
      <CssVoice2 value=""{VOX_2.ToString().Replace("-1", "65535")}"" />
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

                        File.WriteAllText(filePath + ".xml", fileCST);

                        psi = new ProcessStartInfo
                        {
                            FileName = "server\\data\\system\\XMLSerializer.exe",
                            Arguments = "server\\data\\system\\chara_select_table.cst.xml",
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        process = Process.Start(psi);
                        process.WaitForExit();

                        string msgPath = Path.Combine(Settings.Default.datafolder, "msg", $"proper_noun_character_name_{Settings.Default.language}.msg");
                        MSG msgFile = MSGStream.Read(msgPath);
                        var entries = msgFile.data.ToList();

                        int lastId = entries.Count;

                        entries.Add(new MSGEntry
                        {
                            ID = lastId,
                            NameID = $"chara_{CMS_BCS}_000",
                            Lines = new[] { MSG_CHARACTER_NAME }
                        });

                        msgFile.data = entries.ToArray();
                        MSGStream.Write(msgFile, msgPath);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\ui\texture");
                                sw.WriteLine(@"embpack.exe CHARA01");
                            }
                        }
                        // QXD
                        psi = new ProcessStartInfo
                        {
                            FileName = "server\\data\\system\\XMLSerializer.exe",
                            Arguments = "server\\data\\quest\\CHQ\\chq_data.qxd",
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        process = Process.Start(psi);

                        process.WaitForExit();
                        string file99 = File.ReadAllText("server\\data\\quest\\CHQ\\chq_data.qxd" + ".xml");


                        file99 = file99.Replace("  </NormalCharacters>\r\n", $"    <Character ID=\"{CharID}\" Character=\"{CMS_BCS}\">\r\n      <Costume_Index value=\"0\" />\r\n      <I_12 value=\"0\" />\r\n      <Level value=\"1\" />\r\n      <Health value=\"-1.0\" />\r\n      <Stamina_Armour value=\"-1.0\" />\r\n      <Ki value=\"-1.0\" />\r\n      <Stamina value=\"-1.0\" />\r\n      <Basic_Melee value=\"-1.0\" />\r\n      <Ki_Blast value=\"-1.0\" />\r\n      <Strike_Super value=\"-1.0\" />\r\n      <Ki_Super value=\"-1.0\" />\r\n      <Basic_Melee_Damage value=\"-1.0\" />\r\n      <Ki_Blast_Damage value=\"-1.0\" />\r\n      <Strike_Super_Damage value=\"-1.0\" />\r\n      <Ki_Super_Damage value=\"-1.0\" />\r\n      <F_68 value=\"-1.0\" />\r\n      <F_72 value=\"-1.0\" />\r\n      <Air_Speed value=\"-1.0\" />\r\n      <Boost_Speed value=\"-1.0\" />\r\n      <AI_Table ID=\"0\" />\r\n      <Transformation value=\"-1\" />\r\n      <Super_Soul value=\"65535\" />\r\n      <I_106 values=\"0, 0, 0, 0, 0, 0, 0\" />\r\n      <I_124 value=\"65535\" />\r\n      <I_126 value=\"0\" />\r\n      <Skills>\r\n        <Super_1 ID2=\"65535\" />\r\n        <Super_2 ID2=\"65535\" />\r\n        <Super_3 ID2=\"65535\" />\r\n        <Super_4 ID2=\"65535\" />\r\n        <Ultimate_1 ID2=\"65535\" />\r\n        <Ultimate_2 ID2=\"65535\" />\r\n        <Evasive ID2=\"65535\" />\r\n        <Blast_Type ID2=\"65535\" />\r\n        <Awoken ID2=\"65535\" />\r\n      </Skills>\r\n    </Character>\r\n  </NormalCharacters>");

                        File.WriteAllText("server\\data\\quest\\CHQ\\chq_data.qxd" + ".xml", file99 );

                        psi = new ProcessStartInfo
                        {
                            FileName = "server\\data\\system\\XMLSerializer.exe",
                            Arguments = "server\\data\\quest\\CHQ\\chq_data.qxd.xml",
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        process = Process.Start(psi);
                        process.WaitForExit();

                        string[] row = { modname, modauthor, "Added Character" };
                        ListViewItem lvi = new ListViewItem(row);
                        lvMods.Items.Add(lvi);
                        saveLvItems();
                    }
                    else if(modtype == "ADDED_SKILL")
                    {

                        List<string> installedFiles = EnumerateFiles("./XV2RebornTemp");
                        List<string> final = new List<string>();

                        foreach (string installedFile in installedFiles)
                        {
                            string modifiedFile = installedFile.Replace("./XV2RebornTemp", Properties.Settings.Default.datafolder);
                            final.Add(modifiedFile);
                        }

                        string installedDir = Path.Combine(Settings.Default.datafolder, "installed");
                        if (!Directory.Exists(installedDir))
                        {
                            Directory.CreateDirectory(installedDir);
                        }
                        //062_GHS_MSK

                        string filePath = Path.Combine(Settings.Default.datafolder, Settings.Default.datafolder + $"/installed/{modtype}_{modname}_{SKILL_TYPE}_{SKILL_ID1.PadLeft(3, '0')}_{SKILL_ID2.PadLeft(3, '0')}_{CharID}_{SKILL_ShortName}.txt");
                        File.WriteAllLines(filePath, final);


                        if (Directory.Exists("./XV2RebornTemp/JUNGLE"))
                            DirectoryUtils.MergeDirectoriesWithConfirmation("./XV2RebornTemp/JUNGLE", Settings.Default.datafolder);
                        DirectoryUtils.MergeDirectoriesWithConfirmation("./XV2RebornTemp", Settings.Default.datafolder);

                        Clean();

                        //CUS

                        // CUS
                        Process p = new Process();
                        ProcessStartInfo info = new ProcessStartInfo();
                        info.FileName = "cmd.exe";
                        info.CreateNoWindow = true;
                        info.WindowStyle = ProcessWindowStyle.Hidden;
                        info.RedirectStandardInput = true;
                        info.UseShellExecute = false;
                        p.StartInfo = info;
                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"XMLSerializer.exe custom_skill.cus");
                            }
                        }

                        p.WaitForExit();

                        string cuspath = Settings.Default.datafolder + @"\system\custom_skill.cus.xml";
                        string text4 = File.ReadAllText(cuspath);

                        switch (SKILL_TYPE)
                        {
                            case "SPA":
                                text4 = text4.Replace("\r\n  </SuperSkills>", $"<Skill ShortName=\"{SKILL_ShortName}\" ID1=\"{SKILL_ID1}\" ID2=\"{SKILL_ID2}\"><I_04 value=\"{SKILL_I_04}\" /><Race_Lock value=\"{SKILL_Race_Lock}\" /><Type value=\"{SKILL_Type}\" /><FilesLoaded Flags=\"{SKILL_FilesLoaded}\" /><PartSet value=\"{SKILL_PartSet}\" /><I_18 value=\"{SKILL_I_18}\" /><EAN Path=\"{SKILL_EAN}\" /><CAM_EAN Path=\"{SKILL_CAM_EAN}\" /><EEPK Path=\"{SKILL_EEPK}\" /><ACB_SE Path=\"{SKILL_ACB_SE}\" /><ACB_VOX Path=\"{SKILL_ACB_VOX}\" /><AFTER_BAC Path=\"{SKILL_AFTER_BAC}\" /><AFTER_BCM Path=\"{SKILL_AFTER_BCM}\" /><I_48 value=\"{SKILL_I_48}\" /><I_50 value=\"{SKILL_I_50}\" /><I_52 value=\"{SKILL_I_52}\" /><I_54 value=\"{SKILL_I_54}\" /><PUP ID=\"{SKILL_PUP}\" /><CUS_Aura value=\"{SKILL_CUS_Aura}\" /><TransformCharaSwap Chara_ID=\"{SKILL_TransformCharaSwap}\" /><Skillset_Change ModelPreset=\"{SKILL_Skillset_Change}\" /><Num_Of_Transforms value=\"{SKILL_Num_Of_Transforms}\" /><I_66 value=\"{SKILL_I_66}\" /></Skill></SuperSkills>");
                                break;
                            case "ULT":
                                text4 = text4.Replace("\r\n  </UltimateSkills>", $"<Skill ShortName=\"{SKILL_ShortName}\" ID1=\"{SKILL_ID1}\" ID2=\"{SKILL_ID2}\"><I_04 value=\"{SKILL_I_04}\" /><Race_Lock value=\"{SKILL_Race_Lock}\" /><Type value=\"{SKILL_Type}\" /><FilesLoaded Flags=\"{SKILL_FilesLoaded}\" /><PartSet value=\"{SKILL_PartSet}\" /><I_18 value=\"{SKILL_I_18}\" /><EAN Path=\"{SKILL_EAN}\" /><CAM_EAN Path=\"{SKILL_CAM_EAN}\" /><EEPK Path=\"{SKILL_EEPK}\" /><ACB_SE Path=\"{SKILL_ACB_SE}\" /><ACB_VOX Path=\"{SKILL_ACB_VOX}\" /><AFTER_BAC Path=\"{SKILL_AFTER_BAC}\" /><AFTER_BCM Path=\"{SKILL_AFTER_BCM}\" /><I_48 value=\"{SKILL_I_48}\" /><I_50 value=\"{SKILL_I_50}\" /><I_52 value=\"{SKILL_I_52}\" /><I_54 value=\"{SKILL_I_54}\" /><PUP ID=\"{SKILL_PUP}\" /><CUS_Aura value=\"{SKILL_CUS_Aura}\" /><TransformCharaSwap Chara_ID=\"{SKILL_TransformCharaSwap}\" /><Skillset_Change ModelPreset=\"{SKILL_Skillset_Change}\" /><Num_Of_Transforms value=\"{SKILL_Num_Of_Transforms}\" /><I_66 value=\"{SKILL_I_66}\" /></Skill></UltimateSkills>");
                                break;
                            case "ESC":
                                text4 = text4.Replace("\r\n  </EvasiveSkills>", $"<Skill ShortName=\"{SKILL_ShortName}\" ID1=\"{SKILL_ID1}\" ID2=\"{SKILL_ID2}\"><I_04 value=\"{SKILL_I_04}\" /><Race_Lock value=\"{SKILL_Race_Lock}\" /><Type value=\"{SKILL_Type}\" /><FilesLoaded Flags=\"{SKILL_FilesLoaded}\" /><PartSet value=\"{SKILL_PartSet}\" /><I_18 value=\"{SKILL_I_18}\" /><EAN Path=\"{SKILL_EAN}\" /><CAM_EAN Path=\"{SKILL_CAM_EAN}\" /><EEPK Path=\"{SKILL_EEPK}\" /><ACB_SE Path=\"{SKILL_ACB_SE}\" /><ACB_VOX Path=\"{SKILL_ACB_VOX}\" /><AFTER_BAC Path=\"{SKILL_AFTER_BAC}\" /><AFTER_BCM Path=\"{SKILL_AFTER_BCM}\" /><I_48 value=\"{SKILL_I_48}\" /><I_50 value=\"{SKILL_I_50}\" /><I_52 value=\"{SKILL_I_52}\" /><I_54 value=\"{SKILL_I_54}\" /><PUP ID=\"{SKILL_PUP}\" /><CUS_Aura value=\"{SKILL_CUS_Aura}\" /><TransformCharaSwap Chara_ID=\"{SKILL_TransformCharaSwap}\" /><Skillset_Change ModelPreset=\"{SKILL_Skillset_Change}\" /><Num_Of_Transforms value=\"{SKILL_Num_Of_Transforms}\" /><I_66 value=\"{SKILL_I_66}\" /></Skill></EvasiveSkills>");
                                break;
                        }

                        File.WriteAllText(cuspath, text4);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"XMLSerializer.exe " + quote + Settings.Default.datafolder + @"\system\custom_skill.cus.xml" + quote);
                            }
                        }

                        p.WaitForExit();

                        //MSG

                        MSG MSGfile;
                        MSG MSGfile2;
                        MSGfile = MSGStream.Read(Settings.Default.datafolder + @"/MSG/proper_noun_skill_" + "spa" + "_name_" + language + ".MSG");
                        MSGfile2 = MSGStream.Read(Settings.Default.datafolder + @"/MSG/proper_noun_skill_" + "spa" + "_info_" + language + ".MSG");
                        string MSGfileSkName = "";
                        string MSGfileSkInfo = "";
                        string sktypeMSG = "";

                        switch (SKILL_TYPE)
                        {
                            case "SPA":
                                sktypeMSG = "spe_skill_";
                                MSGfileSkName = Settings.Default.datafolder + @"/MSG/proper_noun_skill_" + "spa" + "_name_" + language + ".MSG";
                                MSGfileSkInfo = Settings.Default.datafolder + @"/MSG/proper_noun_skill_" + "spa" + "_info_" + language + ".MSG";
                                MSGfile = MSGStream.Read(Settings.Default.datafolder + @"/MSG/proper_noun_skill_" + "spa" + "_name_" + language + ".MSG");
                                MSGfile2 = MSGStream.Read(Settings.Default.datafolder + @"/MSG/proper_noun_skill_" + "spa" + "_info_" + language + ".MSG");
                                break;
                            case "ULT":
                                sktypeMSG = "ult_";

                                MSGfileSkName = Settings.Default.datafolder + @"/MSG/proper_noun_skill_" + "ult" + "_name_" + language + ".MSG";
                                MSGfileSkInfo = Settings.Default.datafolder + @"/MSG/proper_noun_skill_" + "ult" + "_info_" + language + ".MSG";
                                MSGfile = MSGStream.Read(Settings.Default.datafolder + @"/MSG/proper_noun_skill_" + "ult" + "_name_" + language + ".MSG");
                                MSGfile2 = MSGStream.Read(Settings.Default.datafolder + @"/MSG/proper_noun_skill_" + "ult" + "_info_" + language + ".MSG");
                                break;
                            case "ESC":
                                sktypeMSG = "avoid_skill_";

                                MSGfileSkName = Settings.Default.datafolder + @"/MSG/proper_noun_skill_" + "esc" + "_name_" + language + ".MSG";
                                MSGfileSkInfo = Settings.Default.datafolder + @"/MSG/proper_noun_skill_" + "esc" + "_info_" + language + ".MSG";
                                MSGfile = MSGStream.Read(Settings.Default.datafolder + @"/MSG/proper_noun_skill_" + "esc" + "_name_" + language + ".MSG");
                                MSGfile2 = MSGStream.Read(Settings.Default.datafolder + @"/MSG/proper_noun_skill_" + "esc" + "_info_" + language + ".MSG");
                                break;

                        }
                        MSGEntry[] expand = new MSGEntry[MSGfile.data.Length + 1];
                        Array.Copy(MSGfile.data, expand, MSGfile.data.Length);
                        expand[expand.Length - 1] = new MSGEntry(); // <-- FIX HERE
                        expand[expand.Length - 1].ID = MSGfile.data.Length;
                        expand[expand.Length - 1].Lines = new string[] { MSG_SKILL_NAME };
                        expand[expand.Length - 1].NameID = sktypeMSG + SKILL_ID1.PadLeft(3, '0');
                        MSGfile.data = expand;
                        MSGStream.Write(MSGfile, MSGfileSkName);

                        // And repeat for MSGfile2:
                        expand = new MSGEntry[MSGfile2.data.Length + 1];
                        Array.Copy(MSGfile2.data, expand, MSGfile2.data.Length);
                        expand[expand.Length - 1] = new MSGEntry(); // <-- FIX HERE TOO
                        expand[expand.Length - 1].ID = MSGfile2.data.Length;
                        expand[expand.Length - 1].Lines = new string[] { MSG_SKILL_DESC };
                        expand[expand.Length - 1].NameID = sktypeMSG + SKILL_ID1.PadLeft(3, '0');
                        MSGfile2.data = expand;
                        MSGStream.Write(MSGfile2, MSGfileSkName);

                        Clean();
                        string[] row = { modname, modauthor, "Added Skill" };
                        ListViewItem lvi = new ListViewItem(row);
                        lvMods.Items.Add(lvi);
                        saveLvItems();
                    }
                }
            }
            Clean();
        }
        private void uninstallMod(object sender, EventArgs e)
        {
            if (lvMods.SelectedItems.Count == 0)
            {
                MessageBox.Show("No mod selected for uninstallation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string serializerPath = Settings.Default.datafolder + "/system/XMLSerializer.exe";

            ListViewItem selectedItem = lvMods.SelectedItems[0];
            string modtype = selectedItem.SubItems[2].Text;
            string modname = selectedItem.SubItems[0].Text;
            switch (modtype)
            {
                case "Replacer":
                    modtype = "REPLACER";
                    break;
                case "Added Character":
                    modtype = "ADDED_CHARACTER";
                    break;
                case "Added Skill":
                    modtype = "ADDED_SKILL";
                    break;
                default:
                    throw new NotImplementedException();
            }
            string modFilePath = Path.Combine(Settings.Default.datafolder, "installed", $"{modtype}_{modname}.txt");

            if (modtype == "REPLACER")
            {
                if (File.Exists(modFilePath))
                {
                    string[] installedFiles = File.ReadAllLines(modFilePath);
                    foreach (string file in installedFiles)
                    {
                        if (File.Exists(file))
                        {
                            File.Delete(file);
                            Console.WriteLine($"{file} - File deleted");
                        }
                        else if (Directory.Exists(file))
                        {
                            Directory.Delete(file, true);
                            Console.WriteLine($"{file} - Directory deleted");
                        }
                    }
                    File.Delete(modFilePath);
                }
            }
            else if (modtype == "ADDED_CHARACTER")
            {
                string modFilePathPattern = Path.Combine(Settings.Default.datafolder, "installed", $"{modtype}_{modname}_*_*.txt");

                // Use Directory.GetFiles with the pattern to find matching files
                string[] matchingFiles = Directory.GetFiles(Settings.Default.datafolder + "/installed", $"{modtype}_{modname}_*_*.txt");

                // If you need to work with the first matching file, for example
                modFilePath = matchingFiles.Length > 0 ? matchingFiles[0] : null; // Handle the case if no file is found

                if (modFilePath != null)
                {
                    Console.WriteLine("Found file: " + modFilePath);
                }
                else
                {
                    Console.WriteLine("No matching file found.");
                }
                string fileName = Path.GetFileName(modFilePath);
                // Updated pattern to allow spaces, parentheses, and other special characters
                string pattern = @"^(\w+)_(.*?)_(\w+)_(\d+)\.txt$";

                // Match the file name against the pattern
                Match match = Regex.Match(fileName, pattern);
                string CMS_BCS = "";
                int CMS_ID = 0;
                if (match.Success)
                {
                    CMS_BCS = match.Groups[3].Value;
                    if(int.TryParse(match.Groups[4].Value, out CMS_ID))
                        Console.WriteLine($"Found CMS_BCS: {CMS_BCS}, CMS_ID: {CMS_ID}");
                }
                else
                {
                    Console.WriteLine("No matching file format found.");
                }

                if (File.Exists(modFilePath))
                {
                    string[] installedFiles = File.ReadAllLines(modFilePath);
                    foreach (string f in installedFiles)
                    {
                        if (File.Exists(f))
                        {
                            File.Delete(f);
                            Console.WriteLine($"{f} - File deleted");
                        }
                        else if (Directory.Exists(f))
                        {
                            Directory.Delete(f, true);
                            Console.WriteLine($"{f} - Directory deleted");
                        }
                    }
                    File.Delete(modFilePath);
                }

                // Remove Character from CMS
                //TODO

                // Remove Character from CSO
                CSO cso = new CSO();
                cso.Load(Settings.Default.datafolder + @"/system/chara_sound.cso");
                for (int i = 0; i < cso.Data.Count(); i++)
                {
                    if (cso.Data[i].Char_ID == CMS_ID)
                        cso.RemoveCharacter(cso.Data[i]);
                }
                // Remove Character from CUS
                RunCommand($"\"{serializerPath}\" \"{Settings.Default.datafolder + @"/system/custom_skill.cus"}\"");

                string cuspath = Settings.Default.datafolder + @"/system/custom_skill.cus.xml";
                if (File.Exists(cuspath))
                {
                    // Read the content of the CUS XML file
                    string text4 = File.ReadAllText(cuspath);

                    // Define the regex pattern to remove the skillset
                    string skillsetToRemove = $"    <Skillset Character_ID=\"{CMS_ID}\" Costume_Index=\"0\" Model_Preset=\"0\">.*?</Skillset>";

                    // Perform the regex replacement
                    text4 = Regex.Replace(text4, skillsetToRemove, "", RegexOptions.Singleline);

                    // Write the updated content back to the file
                    File.WriteAllText(cuspath, text4);
                }

                // Re-serialize the CUS file
                RunCommand($"\"{serializerPath}\" \"{cuspath}\"");

                // Remove Character from AUR
                RunCommand($"\"{serializerPath}\" \"{Settings.Default.datafolder + @"/system/aura_setting.aur"}\"");

                string aurpath = Settings.Default.datafolder + @"/system/aura_setting.aur.xml";
                if (File.Exists(aurpath))
                {
                    // Read the content of the AUR XML file
                    string text5 = File.ReadAllText(aurpath);

                    // Define the regex pattern to remove the aura entry
                    string auraToRemove = $"    <CharacterAura Chara_ID=\"{CMS_ID}\" Costume=\"0\".*?/>\r\n";

                    // Perform the regex replacement
                    text5 = Regex.Replace(text5, auraToRemove, "");

                    // Write the updated content back to the file
                    File.WriteAllText(aurpath, text5);
                }

                // Re-serialize the AUR file
                RunCommand($"\"{serializerPath}\" \"{Settings.Default.datafolder + @"/system/aura_setting.aur.xml"}\"");

                // Remove Character from PSC
                RunCommand($"\"{serializerPath}\" \"{Settings.Default.datafolder + @"/system/parameter_spec_char.psc"}\"");

                string pscpath = Settings.Default.datafolder + @"/system/parameter_spec_char.psc.xml";
                if (File.Exists(pscpath))
                {
                    // Read the content of the PSC XML file
                    string text6 = File.ReadAllText(pscpath);

                    // Define the regex pattern to remove the PSC entry
                    string pscToRemove = $"    <PSC_Entry Chara_ID=\"{CMS_ID}\">.*?</PSC_Entry>";

                    // Perform the regex replacement
                    text6 = Regex.Replace(text6, pscToRemove, "", RegexOptions.Singleline);

                    // Write the updated content back to the file
                    File.WriteAllText(pscpath, text6);
                }
                RunCommand($"\"{serializerPath}\" \"{Settings.Default.datafolder + @"/system/parameter_spec_char.psc.xml"}\"");

                string charalist = Settings.Default.datafolder + @"/XVP_SLOTS.xs";
                if (File.Exists(charalist))
                {
                    string s = File.ReadAllText(charalist);
                    Console.WriteLine("CMS_BCS value: " + CMS_BCS);
                    string escapedCMS_BCS = Regex.Escape(CMS_BCS);
                    Console.WriteLine("Escaped CMS_BCS: " + escapedCMS_BCS);
                    pattern = @"\{\[\s*" + escapedCMS_BCS + @"\s*,\s*0\s*,\s*0\s*,\s*0\s*,\s*(-?\d{1,3})\s*,\s*(-?\d{1,3})\s*\]\}";
                    Console.WriteLine("Regex pattern: " + pattern);
                    string modifiedContent = Regex.Replace(s, pattern, "");

                    Console.WriteLine("Modified content: " + modifiedContent);
                    File.WriteAllText(charalist, modifiedContent);
                }
                else
                {
                    Console.WriteLine("File does not exist.");
                }
                // Remove from MSG
                string MSGPath = Settings.Default.datafolder + @"/MSG/proper_noun_character_name_" + language + ".MSG";
                if (File.Exists(MSGPath))
                {
                    MSG MSGfile = MSGStream.Read(MSGPath);
                    List<MSGEntry> updatedData = MSGfile.data.Where(d => !d.NameID.Contains(CMS_BCS)).ToList();
                    MSGfile.data = updatedData.ToArray();
                    MSGStream.Write(MSGfile, MSGPath);
                }

            }
            else if (modtype == "ADDED_SKILL")
            {
                string modFilePathPattern = Path.Combine(Settings.Default.datafolder, "installed", $"{modtype}_{modname}_*_*_*.txt");

                // Use Directory.GetFiles with the pattern to find matching files
                string[] matchingFiles = Directory.GetFiles(Settings.Default.datafolder + "/installed", $"{modtype}_{modname}_*_*_*_*.txt");

                // If you need to work with the first matching file, for example
                modFilePath = matchingFiles.Length > 0 ? matchingFiles[0] : null; // Handle the case if no file is found

                if (modFilePath != null)
                {
                    Console.WriteLine("Found file: " + modFilePath);
                }
                else
                {
                    Console.WriteLine("No matching file found.");
                }
                // Check if the file exists
                if (File.Exists(modFilePath))
                {
                    // Get the list of files installed
                    string[] installedFiles = File.ReadAllLines(modFilePath);

                    // Delete the installed files
                    foreach (string file in installedFiles)
                    {
                        if (File.Exists(file))
                        {
                            File.Delete(file);
                            Console.WriteLine($"{file} - File deleted");
                        }
                        else if (Directory.Exists(file))
                        {
                            Directory.Delete(file, true);
                            Console.WriteLine($"{file} - Directory deleted");
                        }
                    }

                    // Delete the mod installation data file
                    File.Delete(modFilePath);
                }

                // Remove the directories created during installation
                if (Directory.Exists("./XV2RebornTemp/JUNGLE"))
                    Directory.Delete("./XV2RebornTemp/JUNGLE", true);
                if (Directory.Exists("./XV2RebornTemp"))
                    Directory.Delete("./XV2RebornTemp", true);

                string fileName = Path.GetFileName(modFilePath);
                
                string pattern = @"^(\w+)_(\w+)_(\w+)_(\d{3})_(\w+)_(\w+)_(.*?)\.txt$";

                // Match the file name against the pattern
                Match match = Regex.Match(fileName, pattern);


                string SKILL_TYPE = "";
                string SKILL_ID1 = "";
                string SKILL_ID2 = "";
                string CharID = "";
                string SKILL_ShortName = "";

                if (match.Success)
                {
                    // Extract the values from the matching groups
                    SKILL_TYPE = match.Groups[3].Value;         // The third group corresponds to SKILL_TYPE
                    SKILL_ID1 = match.Groups[4].Value;          // The fourth group corresponds to SKILL_ID1 (padded with zeros)
                    SKILL_ID2 = match.Groups[5].Value;          // The fourth group corresponds to SKILL_ID1 (padded with zeros)
                    CharID = match.Groups[6].Value;             // The fifth group corresponds to CharID
                    SKILL_ShortName = match.Groups[7].Value;    // The sixth group corresponds to SKILL_ShortName

                    Console.WriteLine($"SKILL_TYPE: {SKILL_TYPE}");
                    Console.WriteLine($"SKILL_ID1: {SKILL_ID1}");
                    Console.WriteLine($"SKILL_ID2: {SKILL_ID2}");
                    Console.WriteLine($"CharID: {CharID}");
                    Console.WriteLine($"SKILL_ShortName: {SKILL_ShortName}");

                    // Example of constructing the file path
                    string filePath = Path.Combine(Settings.Default.datafolder, $"{Settings.Default.datafolder}/installed/{modtype}_{modname}_{SKILL_TYPE}_{SKILL_ID1.PadLeft(3, '0')}_{SKILL_ID2.PadLeft(3, '0')}_{CharID}_{SKILL_ShortName}.txt");
                    Console.WriteLine($"File path: {filePath}");
                }
                else
                {
                    Console.WriteLine("No matching file format found.");
                }
                // Run the first command (assuming it's required for some processing)
                RunCommand($"\"{serializerPath}\" \"{Settings.Default.datafolder + @"/system/custom_skill.cus"}\"");

                // Remove the custom skill from the XML file
                string cuspath = Settings.Default.datafolder + @"\system\custom_skill.cus.xml";
                if (File.Exists(cuspath))
                {
                    // Read the content of the XML file
                    string xmlContent = File.ReadAllText(cuspath);

                    // Define the pattern to match the skill to remove (accounting for the exact padding)
                    string patternCUS = $@"<Skill ShortName=""{SKILL_ShortName}"" ID1=""{SKILL_ID1.PadLeft(3, '0')}"" ID2=""{SKILL_ID2.PadLeft(3, '0')}"">.*?</Skill>";

                    // Remove the skill from the XML content (using the single-line option to handle multiline)
                    string updatedXmlContent = Regex.Replace(xmlContent, patternCUS, string.Empty, RegexOptions.Singleline);

                    // Write the updated XML back to the file
                    File.WriteAllText(cuspath, updatedXmlContent);

                    // Finally, serialize the updated XML (if this step is needed)
                    RunCommand($"\"{serializerPath}\" \"{Settings.Default.datafolder + @"/system/custom_skill.cus.xml"}\"");
                }


                // Remove skill data from the MSG files
                string MSGPathName = Settings.Default.datafolder + @"/MSG/proper_noun_skill_" + SKILL_TYPE.ToLower() + "_name_" + language + ".MSG";
                string MSGPathInfo = Settings.Default.datafolder + @"/MSG/proper_noun_skill_" + SKILL_TYPE.ToLower() + "_info_" + language + ".MSG";

                if (File.Exists(MSGPathName))
                {
                    MSG MSGfile = MSGStream.Read(MSGPathName);
                    List<MSGEntry> updatedNameData = MSGfile.data.Where(d => !d.NameID.Contains(SKILL_ID1.PadLeft(3, '0'))).ToList();
                    MSGfile.data = updatedNameData.ToArray();
                    MSGStream.Write(MSGfile, MSGPathName);
                }

                if (File.Exists(MSGPathInfo))
                {
                    MSG MSGfile2 = MSGStream.Read(MSGPathInfo);
                    List<MSGEntry> updatedInfoData = MSGfile2.data.Where(d => !d.NameID.Contains(SKILL_ID1.PadLeft(3, '0'))).ToList();
                    MSGfile2.data = updatedInfoData.ToArray();
                    MSGStream.Write(MSGfile2, MSGPathInfo);
                }

                // Remove the mod from the ListView and save the updated list
                lvMods.Items.Remove(selectedItem);
                saveLvItems();
            }


            lvMods.Items.Remove(selectedItem);
            saveLvItems();
            MessageBox.Show("Mod successfully uninstalled.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Clean();
        }

        public static void RemoveEmptyDirectories(string path)
        {
            if (Directory.Exists(path))
            {
                foreach (var subdirectory in Directory.GetDirectories(path))
                {
                    RemoveEmptyDirectories(subdirectory); 
                }

                if (Directory.GetFileSystemEntries(path).Length == 0)
                {
                    try
                    {
                        Directory.Delete(path); // Remove the empty directory
                        Console.WriteLine($"Removed empty directory: {path}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error removing directory {path}: {ex.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Directory not found: {path}");
            }
        }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void RunCommand(string command)
        {
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardInput = true,
                UseShellExecute = false
            };

            p.StartInfo = info;
            p.Start();

            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine(command);
                }
            }

            p.WaitForExit();
        }
        public static void CopyDirectory(string sourceDir, string destDir, bool recursive = true)
        {
            if (string.IsNullOrEmpty(sourceDir))
                throw new ArgumentNullException(nameof(sourceDir), "Source directory cannot be null or empty.");

            if (!Directory.Exists(sourceDir))
                throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");

            if (Directory.Exists(destDir))
                Directory.Delete(destDir, recursive);

            Directory.CreateDirectory(destDir);

            var files = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories);
            if (files.Length == 0)
                throw new FileNotFoundException($"No files found in directory: {sourceDir}");

            foreach (string file in files)
            {
                string destFile = file.Replace(sourceDir, destDir);
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));
                File.Copy(file, destFile, true);
            }
        }
        private void WriteElementWithValue(XmlWriter writer, string elementName, string value)
        {
            if (!string.IsNullOrEmpty(value))  // Check if value is not empty or null
            {
                // Remove the surrounding quotes if present
                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    value = value.Substring(1, value.Length - 2);  // Remove the surrounding quotes
                }

                writer.WriteStartElement(elementName);
                writer.WriteAttributeString("value", value);  // Add value as an attribute
                writer.WriteEndElement();
            }
            else
            {
                string tempVal = "\"\"";
                // Remove the surrounding quotes if present
                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    value = tempVal.Substring(1, value.Length - 2);  // Remove the surrounding quotes
                }

                writer.WriteStartElement(elementName);
                writer.WriteAttributeString("value", value);  // Add value as an attribute
                writer.WriteEndElement();
            }
        }
        public static void MoveDirectory(string sourceDir, string destDir, bool recursive)  //Just to keep it the same
        {
            if (!Directory.Exists(sourceDir))
            {
                throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");
            }

            if (Directory.Exists(destDir))
            {
                Directory.Delete(destDir, recursive);
            }

            Directory.CreateDirectory(destDir);

            foreach (string file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                string destFile = file.Replace(sourceDir, destDir);
                Directory.CreateDirectory(Path.GetDirectoryName(destFile)); 
                File.Move(file, destFile);
            }

            Directory.Delete(sourceDir, recursive);
        }
        private string ShowInputDialog(string prompt, string defaultValue = "")
        {
            using (InputForm inputForm = new InputForm(prompt, defaultValue))
            {
                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    return inputForm.UserInput;
                }
            }

            return string.Empty;
        }

        private void produceCPKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CPK Files|*.cpk";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Title = "Save CPK";
            saveFileDialog.OverwritePrompt = true;

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;
            else
            {
                if (File.Exists(saveFileDialog.FileName))
                    File.Delete(saveFileDialog.FileName);
                var psi = new ProcessStartInfo
                {
                    FileName = "CPKTools\\cpkmakec.exe",
                    Arguments = $"\"server\" \"{saveFileDialog.FileName}\" -align=512 -mode=FILENAME -code=UTF-8 -nodatetime",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                var process = Process.Start(psi);

                process.WaitForExit();
            }

        }

        private void convertX2MToXV2MODToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Convert X2M Mod";
            ofd.Filter = "Xenoverse 2 Mod Files (*.x2m)|*.x2m";
            ofd.Multiselect = false;  //Important
            ofd.CheckFileExists = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in ofd.FileNames)
                {
                    string tempFolder = "./XV2RebornTemp";
                    if(Directory.Exists(tempFolder))
                        Directory.Delete(tempFolder, true);
                    Directory.CreateDirectory(tempFolder);
                    ZipFile.ExtractToDirectory(ofd.FileName, tempFolder);

                    var x2mfolder = Directory.GetDirectories(tempFolder).FirstOrDefault(d => Regex.IsMatch(Path.GetFileName(d), "^[A-Z0-9]{3}$"));
                   
                    string embpackPath = Path.Combine(Settings.Default.datafolder, @"ui\texture", "embpack.exe");

                    string finalPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{Path.GetFileNameWithoutExtension(ofd.FileName)}";
                    MoveDirectory(tempFolder, finalPath, true);
                    // Helper method to safely get attribute value
                    string GetAttributeValue(XmlNode node, string attributeName)
                    {
                        var attribute = node?.Attributes[attributeName];
                        return attribute?.Value ?? "N/A"; // Return "N/A" if the attribute doesn't exist
                    }
                    var xmlContent = File.ReadAllText(finalPath + @"/x2m.xml");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);
                    // Basic Information
                    string x2mType = GetAttributeValue(doc.SelectSingleNode("//X2M"), "type");

                    if (x2mType == "NEW_CHARACTER")
                    {
                        string formatVersion = GetAttributeValue(doc.SelectSingleNode("//X2M_FORMAT_VERSION"), "value");
                        string modName = GetAttributeValue(doc.SelectSingleNode("//MOD_NAME"), "value");
                        string modAuthor = GetAttributeValue(doc.SelectSingleNode("//MOD_AUTHOR"), "value");
                        string modVersion = GetAttributeValue(doc.SelectSingleNode("//MOD_VERSION"), "value");
                        string modGuid = GetAttributeValue(doc.SelectSingleNode("//MOD_GUID"), "value");
                        string uData = GetAttributeValue(doc.SelectSingleNode("//UDATA"), "value");
                        string entryName = GetAttributeValue(doc.SelectSingleNode("//ENTRY_NAME"), "value");
                        string charaNameEn = GetAttributeValue(doc.SelectSingleNode("//CHARA_NAME_EN"), "value");

                        // SlotEntry
                        string slotCostumeIndex = GetAttributeValue(doc.SelectSingleNode("//SlotEntry"), "costume_index");
                        string modelPreset = GetAttributeValue(doc.SelectSingleNode("//SlotEntry/MODEL_PRESET"), "value");
                        string flagGK2 = GetAttributeValue(doc.SelectSingleNode("//SlotEntry/FLAG_GK2"), "value");
                        string voicesIdList = GetAttributeValue(doc.SelectSingleNode("//SlotEntry/VOICES_ID_LIST"), "value");
                        string costumeNameEn = GetAttributeValue(doc.SelectSingleNode("//SlotEntry/COSTUME_NAME_EN"), "value");

                        // Entry
                        string entryId = GetAttributeValue(doc.SelectSingleNode("//Entry"), "id");
                        string entryNameId = GetAttributeValue(doc.SelectSingleNode("//Entry"), "name");

                        string u10 = GetAttributeValue(doc.SelectSingleNode("//Entry/U_10"), "value");
                        string loadCamDist = GetAttributeValue(doc.SelectSingleNode("//Entry/LOAD_CAM_DIST"), "value");
                        string u16 = GetAttributeValue(doc.SelectSingleNode("//Entry/U_16"), "value");
                        string u18 = GetAttributeValue(doc.SelectSingleNode("//Entry/U_18"), "value");
                        string u1A = GetAttributeValue(doc.SelectSingleNode("//Entry/U_1A"), "value");
                        string character = GetAttributeValue(doc.SelectSingleNode("//Entry/CHARACTER"), "value");
                        string ean = GetAttributeValue(doc.SelectSingleNode("//Entry/EAN"), "value");
                        string fceEan = GetAttributeValue(doc.SelectSingleNode("//Entry/FCE_EAN"), "value");
                        string fce = GetAttributeValue(doc.SelectSingleNode("//Entry/FCE"), "value");
                        string camEan = GetAttributeValue(doc.SelectSingleNode("//Entry/CAM_EAN"), "value");
                        string bac = GetAttributeValue(doc.SelectSingleNode("//Entry/BAC"), "value");
                        string bcm = GetAttributeValue(doc.SelectSingleNode("//Entry/BCM"), "value");
                        string ai = GetAttributeValue(doc.SelectSingleNode("//Entry/AI"), "value");
                        string str50 = GetAttributeValue(doc.SelectSingleNode("//Entry/STR_50"), "value");

                        // SkillSet
                        string skillSetCharId = GetAttributeValue(doc.SelectSingleNode("//SkillSet/CHAR_ID"), "value");
                        string skillSetCostumeId = GetAttributeValue(doc.SelectSingleNode("//SkillSet/COSTUME_ID"), "value");
                        string skillsValue = GetAttributeValue(doc.SelectSingleNode("//SkillSet/SKILLS"), "value");

                        // Split the SKILLS string by commas
                        string[] skills = skillsValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        // Output the parsed skills
                        Console.WriteLine("Skills: ");
                        foreach (var skille in skills)
                        {
                            Console.WriteLine(skille.Trim()); // Trim any extra whitespace
                        }
                        string skillSetModelPreset = GetAttributeValue(doc.SelectSingleNode("//SkillSet/MODEL_PRESET"), "value");

                        // CsoEntry
                        string csoCharId = GetAttributeValue(doc.SelectSingleNode("//CsoEntry/CHAR_ID"), "value");
                        string csoCostumeId = GetAttributeValue(doc.SelectSingleNode("//CsoEntry/COSTUME_ID"), "value");
                        string se = GetAttributeValue(doc.SelectSingleNode("//CsoEntry/SE"), "value");
                        string vox = GetAttributeValue(doc.SelectSingleNode("//CsoEntry/VOX"), "value");
                        string amk = GetAttributeValue(doc.SelectSingleNode("//CsoEntry/AMK"), "value");
                        string csoSkills = GetAttributeValue(doc.SelectSingleNode("//CsoEntry/SKILLS"), "value");

                        // PscSpecEntry
                        string costumeId = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/COSTUME_ID"), "value");
                        string costumeId2 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/COSTUME_ID2"), "value");
                        string cameraPosition = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/CAMERA_POSITION"), "value");
                        string u0C = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/U_0C"), "value");
                        string u10_2 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/U_10"), "value");
                        string health = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/HEALTH"), "value");
                        string f18 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/F_18"), "value");
                        string ki = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/KI"), "value");
                        string kiRecharge = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/KI_RECHARGE"), "value");
                        string u24 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/U_24"), "value");
                        string u28 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/U_28"), "value");
                        string u2C = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/U_2C"), "value");
                        string stamina = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/STAMINA"), "value");
                        string staminaRechargeMove = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/STAMINA_RECHARGE_MOVE"), "value");
                        string staminaRechargeAir = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/STAMINA_RECHARGE_AIR"), "value");
                        string staminaRechargeGround = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/STAMINA_RECHARGE_GROUND"), "value");
                        string staminaDrainRate1 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/STAMINA_DRAIN_RATE1"), "value");
                        string staminaDrainRate2 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/STAMINA_DRAIN_RATE2"), "value");
                        string f48 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/F_48"), "value");
                        string basicAttack = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/BASIC_ATTACK"), "value");
                        string basicKiAttack = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/BASIC_KI_ATTACK"), "value");
                        string strikeAttack = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/STRIKE_ATTACK"), "value");
                        string kiBlastSuper = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/KI_BLAST_SUPER"), "value");
                        string basicPhysDefense = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/BASIC_PHYS_DEFENSE"), "value");
                        string basicKiDefense = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/BASIC_KI_DEFENSE"), "value");
                        string strikeAtkDefense = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/STRIKE_ATK_DEFENSE"), "value");
                        string superKiBlastDefense = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/SUPER_KI_BLAST_DEFENSE"), "value");
                        string groundSpeed = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/GROUND_SPEED"), "value");
                        string airSpeed = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/AIR_SPEED"), "value");
                        string boostingSpeed = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/BOOSTING_SPEED"), "value");
                        string dashDistance = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/DASH_DISTANCE"), "value");
                        string f7C = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/F_7C"), "value");
                        string reinfSkillDuration = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/REINF_SKILL_DURATION"), "value");
                        string f84 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/F_84"), "value");
                        string revivalHpAmount = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/REVIVAL_HP_AMOUNT"), "value");
                        string f8C = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/F_8C"), "value");
                        string revivingSpeed = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/REVIVING_SPEED"), "value");
                        string u98 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/U_98"), "value");
                        string talisman = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/TALISMAN"), "value");
                        string uB8 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/U_B8"), "value");
                        string uBC = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/U_BC"), "value");
                        string fC0 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/F_C0"), "value");

                        // CharacLink
                        string characLinkIdCharac = GetAttributeValue(doc.SelectSingleNode("//CharacLink"), "idCharac");
                        string characLinkIdCostume = GetAttributeValue(doc.SelectSingleNode("//CharacLink"), "idCostume");
                        string characLinkIdAura = GetAttributeValue(doc.SelectSingleNode("//CharacLink"), "idAura");
                        string characLinkGlare = GetAttributeValue(doc.SelectSingleNode("//CharacLink"), "glare");

                        // SevEntryHL
                        string sevEntryHlCostumeId = GetAttributeValue(doc.SelectSingleNode("//SevEntryHL"), "costume_id");
                        string sevEntryHlCopyChar = GetAttributeValue(doc.SelectSingleNode("//SevEntryHL"), "copy_char");
                        string sevEntryHlCopyCostume = GetAttributeValue(doc.SelectSingleNode("//SevEntryHL"), "copy_costume");

                        // CmlEntry
                        string cmlEntryCharId = GetAttributeValue(doc.SelectSingleNode("//CmlEntry"), "char_id");
                        string cmlEntryCostumeId = GetAttributeValue(doc.SelectSingleNode("//CmlEntry"), "costume_id");
                        string cmlEntryU04 = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/U_04"), "value");
                        string cmlEntryCssPos = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/CSS_POS"), "value");
                        string cmlEntryCssRot = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/CSS_ROT"), "value");
                        string cmlEntryF0C = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/F_0C"), "value");
                        string cmlEntryF10 = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/F_10"), "value");
                        string cmlEntryF14 = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/F_14"), "value");
                        string cmlEntryF18 = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/F_18"), "value");
                        string cmlEntryF1C = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/F_1C"), "value");
                        string cmlEntryF20 = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/F_20"), "value");
                        string cmlEntryF24 = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/F_24"), "value");
                        string cmlEntryF28 = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/F_28"), "value");
                        string cmlEntryF2C = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/F_2C"), "value");

                        File.Delete(finalPath + @"/x2m.xml");
                        // Create an XmlWriterSettings instance for formatting the XML
                        XmlWriterSettings settings = new XmlWriterSettings
                        {
                            Indent = true,
                            IndentChars = "    ", // Use four spaces for indentation
                        };

                        // Create the XmlWriter and write the XML content
                        string xmlFilePath = finalPath + @"/xv2mod.xml";
                        using (XmlWriter writer = XmlWriter.Create(xmlFilePath, settings))
                        {
                            writer.WriteStartDocument();
                            writer.WriteStartElement("XV2MOD");
                            writer.WriteAttributeString("type", "ADDED_CHARACTER");

                            WriteElementWithValue(writer, "MOD_NAME", modName);
                            WriteElementWithValue(writer, "MOD_AUTHOR", modAuthor);


                            if (Convert.ToInt32(characLinkIdAura, 16) > 0)
                                WriteElementWithValue(writer, "AUR_ID", Convert.ToInt32(characLinkIdAura, 16).ToString());
                            else
                                WriteElementWithValue(writer, "AUR_ID", "0");  //Fallback

                            if (characLinkGlare == "true")
                                WriteElementWithValue(writer, "AUR_GLARE", "1");
                            else
                                WriteElementWithValue(writer, "AUR_GLARE", "0");

                            WriteElementWithValue(writer, "CMS_BCS", character);
                            WriteElementWithValue(writer, "CMS_EAN", ean);
                            WriteElementWithValue(writer, "CMS_FCE_EAN", fceEan);
                            WriteElementWithValue(writer, "CMS_CAM_EAN", camEan);
                            WriteElementWithValue(writer, "CMS_BAC", bac);
                            WriteElementWithValue(writer, "CMS_BCM", bcm);
                            WriteElementWithValue(writer, "CMS_BAI", ai);

                            WriteElementWithValue(writer, "CSO_1", se);
                            WriteElementWithValue(writer, "CSO_2", vox);
                            WriteElementWithValue(writer, "CSO_3", amk);
                            WriteElementWithValue(writer, "CSO_4", csoSkills);

                            WriteElementWithValue(writer, "CUS_SUPER_1", skills[0]);
                            WriteElementWithValue(writer, "CUS_SUPER_2", skills[1]);
                            WriteElementWithValue(writer, "CUS_SUPER_3", skills[2]);
                            WriteElementWithValue(writer, "CUS_SUPER_4", skills[3]);
                            WriteElementWithValue(writer, "CUS_ULTIMATE_1", skills[4]);
                            WriteElementWithValue(writer, "CUS_ULTIMATE_2", skills[5]);
                            WriteElementWithValue(writer, "CUS_EVASIVE", skills[6]);
                            WriteElementWithValue(writer, "CUS_KI_BLAST", skills[7]);
                            WriteElementWithValue(writer, "CUS_AWOKEN", skills[8]);



                            WriteElementWithValue(writer, "PSC_COSTUME", "0");                      // Costume
                            WriteElementWithValue(writer, "PSC_PRESET", "0");                     // Costume2
                            WriteElementWithValue(writer, "PSC_CAMERA_POSITION", Convert.ToInt32(cameraPosition, 16).ToString());         // Camera_Position
                            WriteElementWithValue(writer, "PSC_I_12", Convert.ToInt32(u0C, 16).ToString());                               // I_12
                            WriteElementWithValue(writer, "PSC_I_16", Convert.ToInt32(u10_2, 16).ToString());                               // I_16
                            WriteElementWithValue(writer, "PSC_HEALTH", health);                          // Health
                            WriteElementWithValue(writer, "PSC_F_24", f18);                               // F_24

                            WriteElementWithValue(writer, "PSC_KI", ki);                                  // Ki
                            WriteElementWithValue(writer, "PSC_KI_RECHARGE", kiRecharge);                // Ki_Recharge
                            WriteElementWithValue(writer, "PSC_I_36", Convert.ToInt32(u24, 16).ToString());                               // I_36
                            WriteElementWithValue(writer, "PSC_I_40", Convert.ToInt32(u28, 16).ToString());                               // I_40
                            WriteElementWithValue(writer, "PSC_I_44", Convert.ToInt32(u2C, 16).ToString());                               // I_44
                            WriteElementWithValue(writer, "PSC_STAMINA", stamina);                        // Stamina

                            WriteElementWithValue(writer, "PSC_STAMINA_RECHARGE_MOVE", staminaRechargeMove); // Stamina_Recharge_Move
                            WriteElementWithValue(writer, "PSC_STAMINA_RECHARGE_AIR", staminaRechargeAir);   // Stamina_Recharge_Air
                            WriteElementWithValue(writer, "PSC_STAMINA_RECHARGE_GROUND", staminaRechargeGround); // Stamina_Recharge_Ground
                            WriteElementWithValue(writer, "PSC_STAMINA_DRAIN_RATE_1", staminaDrainRate1);   // Stamina_Drain_Rate_1
                            WriteElementWithValue(writer, "PSC_STAMINA_DRAIN_RATE_2", staminaDrainRate2);   // Stamina_Drain_Rate_2
                            WriteElementWithValue(writer, "PSC_F_72", f48);                                   // F_72

                            WriteElementWithValue(writer, "PSC_BASIC_ATK", basicAttack);                       // Basic_Atk
                            WriteElementWithValue(writer, "PSC_BASIC_KI_ATK", basicKiAttack);                  // Basic_Ki_Atk
                            WriteElementWithValue(writer, "PSC_STRIKE_ATK", strikeAttack);                     // Strike_Atk
                            WriteElementWithValue(writer, "PSC_SUPER_KI_BLAST_ATK", kiBlastSuper);       // Super_Ki_Blast_Atk

                            WriteElementWithValue(writer, "PSC_BASIC_ATK_DEF", basicPhysDefense);             // Basic_Atk_Defense
                            WriteElementWithValue(writer, "PSC_BASIC_KI_DEF", basicKiDefense);           // Basic_Ki_Atk_Defense
                            WriteElementWithValue(writer, "PSC_STRIKE_ATK_DEF", strikeAtkDefense);          // Strike_Atk_Defense
                            WriteElementWithValue(writer, "PSC_SUPER_KI_DEF", superKiBlastDefense);         // Super_Ki_Blast_Defense

                            WriteElementWithValue(writer, "PSC_GROUND_SPEED", groundSpeed);                 // Ground_Speed
                            WriteElementWithValue(writer, "PSC_AIR_SPEED", airSpeed);                       // Air_Speed
                            WriteElementWithValue(writer, "PSC_BOOST_SPEED", boostingSpeed);                // Boosting_Speed
                            WriteElementWithValue(writer, "PSC_DASH_SPEED", dashDistance);                     // Dash_Speed

                            WriteElementWithValue(writer, "PSC_F_124", f7C);                               // F_124
                            WriteElementWithValue(writer, "PSC_REINFORCEMENT_SKILL", reinfSkillDuration); // Reinforcement_Skill_Duration
                            WriteElementWithValue(writer, "PSC_F_132", f84);                               // F_132
                            WriteElementWithValue(writer, "PSC_REVIVAL_HP_AMOUNT", revivalHpAmount);       // Revival_HP_Amount
                            WriteElementWithValue(writer, "PSC_F_140", f8C);                               // F_140
                            WriteElementWithValue(writer, "PSC_REVIVING_SPEED", revivingSpeed);            // Reviving_Speed

                            WriteElementWithValue(writer, "PSC_I_148", "0");                               // I_148
                            WriteElementWithValue(writer, "PSC_I_152", "0");                               // I_152
                            WriteElementWithValue(writer, "PSC_I_156", "0");                               // I_156
                            WriteElementWithValue(writer, "PSC_I_160", "0");                               // I_160
                            WriteElementWithValue(writer, "PSC_I_164", "0");                               // I_164
                            WriteElementWithValue(writer, "PSC_I_168", "0");                               // I_168
                            WriteElementWithValue(writer, "PSC_I_172", "0");                               // I_172
                            WriteElementWithValue(writer, "PSC_I_176", "0");                               // I_176

                            WriteElementWithValue(writer, "PSC_SUPER_SOUL", Convert.ToInt32(talisman, 16).ToString());                     // Super_Soul / talisman
                            WriteElementWithValue(writer, "PSC_I_184", "0");                               // I_184
                            WriteElementWithValue(writer, "PSC_I_188", "0");                               // I_188
                            WriteElementWithValue(writer, "PSC_F_192", "0");                               // F_192
                            WriteElementWithValue(writer, "PSC_NEW_I_20", "0");                          // NEW_I_20


                            WriteElementWithValue(writer, "MSG_CHARACTER_NAME", charaNameEn);
                            WriteElementWithValue(writer, "MSG_COSTUME_NAME", costumeNameEn);

                            WriteElementWithValue(writer, "VOX_1", "-1");
                            WriteElementWithValue(writer, "VOX_2", "-1");

                            writer.WriteEndElement(); // Close XV2MOD
                            writer.WriteEndDocument(); // Close the document
                        }
                        Directory.CreateDirectory(finalPath + $"/ui/texture/CHARA01/");
                        File.Move(finalPath + @"/UI/SEL.DDS", finalPath + $"/ui/texture/CHARA01/{character}_000.DDS");
                        MoveDirectory(finalPath + $"/{character}", finalPath + $"/chara/{character}", true);

                        if (File.Exists(finalPath + ".xv2mod"))
                            File.Delete(finalPath + ".xv2mod");
                        ZipFile.CreateFromDirectory(finalPath, finalPath + ".xv2mod");
                    }
                    else
                    {
                        throw new NotImplementedException("Not yet implemented");
                    }
                    MessageBox.Show($"X2M Converted successfully, you can find the converted file in \"{finalPath + ".xv2mod"}\"", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clean();
                    Directory.Delete(finalPath, true);
                }

            }


    }
}
}
