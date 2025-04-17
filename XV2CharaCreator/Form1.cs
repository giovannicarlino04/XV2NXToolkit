using System.IO.Compression;
using System.Xml;
using XV2CharaCreator.Properties;
using XV2Reborn;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;

namespace XVCharaCreator
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            // Dichiarazione separata dei TextBox
            this.txtCostume = new TextBox();
            this.txtCostume2 = new TextBox();
            this.txtCameraPosition = new TextBox();
            this.txtI12 = new TextBox();
            this.txtI16 = new TextBox();
            this.txtHealth = new TextBox();
            this.txtF24 = new TextBox();
            this.txtKi = new TextBox();
            this.txtKiRecharge = new TextBox();
            this.txtI36 = new TextBox();
            this.txtI40 = new TextBox();
            this.txtI44 = new TextBox();
            this.txtStamina = new TextBox();
            this.txtStaminaRechargeMove = new TextBox();
            this.txtStaminaRechargeAir = new TextBox();
            this.txtStaminaRechargeGround = new TextBox();
            this.txtStaminaDrainRate1 = new TextBox();
            this.txtStaminaDrainRate2 = new TextBox();
            this.txtF72 = new TextBox();
            this.txtBasicAtk = new TextBox();
            this.txtBasicKiAtk = new TextBox();
            this.txtStrikeAtk = new TextBox();
            this.txtSuperKiBlastAtk = new TextBox();
            this.txtBasicAtkDefense = new TextBox();
            this.txtBasicKiAtkDefense = new TextBox();
            this.txtStrikeAtkDefense = new TextBox();
            this.txtSuperKiBlastDefense = new TextBox();
            this.txtGroundSpeed = new TextBox();
            this.txtAirSpeed = new TextBox();
            this.txtBoostingSpeed = new TextBox();
            this.txtDashSpeed = new TextBox();
            this.txtF124 = new TextBox();
            this.txtReinforcementSkillDuration = new TextBox();
            this.txtF132 = new TextBox();
            this.txtRevivalHpAmount = new TextBox();
            this.txtF140 = new TextBox();
            this.txtRevivingSpeed = new TextBox();
            this.txtI148 = new TextBox();
            this.txtI152 = new TextBox();
            this.txtI156 = new TextBox();
            this.txtI160 = new TextBox();
            this.txtI164 = new TextBox();
            this.txtI168 = new TextBox();
            this.txtI172 = new TextBox();
            this.txtI176 = new TextBox();
            this.txtSuperSoul = new TextBox();
            this.txtI184 = new TextBox();
            this.txtI188 = new TextBox();
            this.txtF192 = new TextBox();
            this.txtNewI20 = new TextBox();

            // Initialize other components
            InitializeComponent();

            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            VScrollBar vbar = new VScrollBar();
            Panel scrollPanel = new Panel();

            vbar.Dock = DockStyle.Right;
            vbar.Width = 20; // Adjust width as needed
            vbar.Minimum = 0;
            vbar.Maximum = 100; // Adjust based on the total height of the content
            vbar.LargeChange = 10;
            vbar.SmallChange = 1;

            // Create a container panel to wrap the TableLayoutPanel and VScrollBar
            scrollPanel.Dock = DockStyle.Fill;
            scrollPanel.Controls.Add(tableLayoutPanel);

            tableLayoutPanel.RowCount = 46; // Adjust according to the number of rows
            tableLayoutPanel.ColumnCount = 6; // Labels in the first column, textboxes in the second column
            tableLayoutPanel.Location = new Point(0, 0);
            tableLayoutPanel.Size = new Size(1800, 1800); // Adjust size as needed

            // Etichette per ogni campo
            string[] labels = {
    "Costume", "Costume2", "Camera_Position", "I_12", "I_16", "Health", "F_24", "Ki", "Ki_Recharge",
    "I_36", "I_40", "I_44", "Stamina", "Stamina_Recharge_Move", "Stamina_Recharge_Air",
    "Stamina_Recharge_Ground", "Stamina_Drain_Rate_1", "Stamina_Drain_Rate_2", "F_72",
    "Basic_Atk", "Basic_Ki_Atk", "Strike_Atk", "Super_Ki_Blast_Atk",
    "Basic_Atk_Defense", "Basic_Ki_Atk_Defense", "Strike_Atk_Defense", "Super_Ki_Blast_Defense",
    "Ground_Speed", "Air_Speed", "Boosting_Speed", "Dash_Speed", "F_124",
    "Reinforcement_Skill_Duration", "F_132", "Revival_HP_Amount", "F_140", "Reviving_Speed",
    "I_148", "I_152", "I_156", "I_160", "I_164", "I_168", "I_172", "I_176", "Super_Soul",
    "I_184", "I_188", "F_192", "NEW_I_20"
};

            // Array di TextBox corrispondenti a ciascuna etichetta
            TextBox[] textboxes = {
    txtCostume, txtCostume2, txtCameraPosition, txtI12, txtI16, txtHealth, txtF24, txtKi, txtKiRecharge,
    txtI36, txtI40, txtI44, txtStamina, txtStaminaRechargeMove, txtStaminaRechargeAir, txtStaminaRechargeGround,
    txtStaminaDrainRate1, txtStaminaDrainRate2, txtF72, txtBasicAtk, txtBasicKiAtk, txtStrikeAtk, txtSuperKiBlastAtk,
    txtBasicAtkDefense, txtBasicKiAtkDefense, txtStrikeAtkDefense, txtSuperKiBlastDefense, txtGroundSpeed,
    txtAirSpeed, txtBoostingSpeed, txtDashSpeed, txtF124, txtReinforcementSkillDuration, txtF132, txtRevivalHpAmount,
    txtF140, txtRevivingSpeed, txtI148, txtI152, txtI156, txtI160, txtI164, txtI168, txtI172, txtI176, txtSuperSoul,
    txtI184, txtI188, txtF192, txtNewI20
};

            for (int i = 0; i < labels.Length; i++)
            {
                tableLayoutPanel.Controls.Add(new Label() { Text = labels[i] }, 0, i);
                tableLayoutPanel.Controls.Add(textboxes[i], 1, i);
            }

            // Adjust the maximum value of the scrollbar dynamically based on content size
            vbar.Maximum = tableLayoutPanel.Height - scrollPanel.ClientSize.Height;
            vbar.LargeChange = scrollPanel.ClientSize.Height;

            // ScrollBar event handler to scroll the content
            vbar.Scroll += (sender, e) =>
            {
                tableLayoutPanel.Location = new Point(0, -vbar.Value); // Move the table layout based on the scrollbar's position
            };

            tabPage6.Controls.Add(scrollPanel);
            tabPage6.Controls.Add(vbar);
        }

        private void buildxv2modFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save Mod File";
            sfd.Filter = "Xenoverse 2 Mod Files (*.xv2mod)|*.xv2mod";

            if (txtName.Text.Length > 0 && txtAuthor.Text.Length > 0
                && Directory.Exists(txtFolder.Text)
                && sfd.ShowDialog() == DialogResult.OK
                && File.Exists(textBox1.Text))
            {
                // Specify the path where you want to save the XML file
                string xmlFilePath = "./XVCharaCreatorTemp/xv2mod.xml";

                if (Directory.Exists("./XVCharaCreatorTemp"))
                    Directory.Delete("./XVCharaCreatorTemp", true);
                Directory.CreateDirectory("./XVCharaCreatorTemp");

                // Create an XmlWriterSettings instance for formatting the XML
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "    ", // Use four spaces for indentation
                };
                // Create the XmlWriter and write the XML content
                using (XmlWriter writer = XmlWriter.Create(xmlFilePath, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("XV2MOD");
                    writer.WriteAttributeString("type", "ADDED_CHARACTER");

                    WriteElementWithValue(writer, "MOD_NAME", txtName.Text);
                    WriteElementWithValue(writer, "MOD_AUTHOR", txtAuthor.Text);


                    // Let's start adding the actual character attributes (AUR, CMS, CSO, etc...)
                    WriteElementWithValue(writer, "AUR_ID", txtAuraID.Text);
                    if (cbAuraGlare.Checked)
                        WriteElementWithValue(writer, "AUR_GLARE", "1");
                    else
                        WriteElementWithValue(writer, "AUR_GLARE", "0");

                    WriteElementWithValue(writer, "CMS_BCS", txtBCS.Text);
                    WriteElementWithValue(writer, "CMS_EAN", txtEAN.Text);
                    WriteElementWithValue(writer, "CMS_FCE_EAN", txtFCEEAN.Text);
                    WriteElementWithValue(writer, "CMS_CAM_EAN", txtCAMEAN.Text);
                    WriteElementWithValue(writer, "CMS_BAC", txtBAC.Text);
                    WriteElementWithValue(writer, "CMS_BCM", txtBCM.Text);
                    WriteElementWithValue(writer, "CMS_BAI", txtBAI.Text);

                    WriteElementWithValue(writer, "CSO_1", txtCSO1.Text);
                    WriteElementWithValue(writer, "CSO_2", txtCSO2.Text);
                    WriteElementWithValue(writer, "CSO_3", txtCSO3.Text);
                    WriteElementWithValue(writer, "CSO_4", txtCSO4.Text);

                    WriteElementWithValue(writer, "CUS_SUPER_1", txtCUS1.Text);
                    WriteElementWithValue(writer, "CUS_SUPER_2", txtCUS2.Text);
                    WriteElementWithValue(writer, "CUS_SUPER_3", txtCUS3.Text);
                    WriteElementWithValue(writer, "CUS_SUPER_4", txtCUS4.Text);
                    WriteElementWithValue(writer, "CUS_ULTIMATE_1", txtCUS5.Text);
                    WriteElementWithValue(writer, "CUS_ULTIMATE_2", txtCUS6.Text);
                    WriteElementWithValue(writer, "CUS_EVASIVE", txtCUS7.Text);
                    WriteElementWithValue(writer, "CUS_KI_BLAST", txtCUS8.Text);
                    WriteElementWithValue(writer, "CUS_AWOKEN", txtCUS9.Text);

                    WriteElementWithValue(writer, "PSC_COSTUME", txtCostume.Text);            // Costume
                    WriteElementWithValue(writer, "PSC_PRESET", txtCostume2.Text);              // Costume2
                    WriteElementWithValue(writer, "PSC_CAMERA_POSITION", txtCameraPosition.Text); // Camera_Position
                    WriteElementWithValue(writer, "PSC_I_12", txtI12.Text);                   // I_12
                    WriteElementWithValue(writer, "PSC_I_16", txtI16.Text);                   // I_16 (aggiungilo!)
                    WriteElementWithValue(writer, "PSC_HEALTH", txtHealth.Text);             // Health
                    WriteElementWithValue(writer, "PSC_F_24", txtF24.Text);                   // F_24

                    WriteElementWithValue(writer, "PSC_KI", txtKi.Text);                      // Ki
                    WriteElementWithValue(writer, "PSC_KI_RECHARGE", txtKiRecharge.Text);    // Ki_Recharge
                    WriteElementWithValue(writer, "PSC_I_36", txtI36.Text);                   // I_36
                    WriteElementWithValue(writer, "PSC_I_40", txtI40.Text);                   // I_40
                    WriteElementWithValue(writer, "PSC_I_44", txtI44.Text);                   // I_44
                    WriteElementWithValue(writer, "PSC_STAMINA", txtStamina.Text);           // Stamina

                    WriteElementWithValue(writer, "PSC_STAMINA_RECHARGE_MOVE", txtStaminaRechargeMove.Text);     // Stamina_Recharge_Move
                    WriteElementWithValue(writer, "PSC_STAMINA_RECHARGE_AIR", txtStaminaRechargeAir.Text);       // Stamina_Recharge_Air
                    WriteElementWithValue(writer, "PSC_STAMINA_RECHARGE_GROUND", txtStaminaRechargeGround.Text); // Stamina_Recharge_Ground
                    WriteElementWithValue(writer, "PSC_STAMINA_DRAIN_RATE_1", txtStaminaDrainRate1.Text);         // Stamina_Drain_Rate_1
                    WriteElementWithValue(writer, "PSC_STAMINA_DRAIN_RATE_2", txtStaminaDrainRate2.Text);         // Stamina_Drain_Rate_2
                    WriteElementWithValue(writer, "PSC_F_72", txtF72.Text);                                       // F_72

                    WriteElementWithValue(writer, "PSC_BASIC_ATK", txtBasicAtk.Text);             // Basic_Atk
                    WriteElementWithValue(writer, "PSC_BASIC_KI_ATK", txtBasicKiAtk.Text);        // Basic_Ki_Atk
                    WriteElementWithValue(writer, "PSC_STRIKE_ATK", txtStrikeAtk.Text);           // Strike_Atk
                    WriteElementWithValue(writer, "PSC_SUPER_KI_BLAST_ATK", txtSuperKiBlastAtk.Text); // Super_Ki_Blast_Atk

                    WriteElementWithValue(writer, "PSC_BASIC_ATK_DEF", txtBasicAtkDefense.Text);     // Basic_Atk_Defense
                    WriteElementWithValue(writer, "PSC_BASIC_KI_DEF", txtBasicKiAtkDefense.Text);       // Basic_Ki_Atk_Defense
                    WriteElementWithValue(writer, "PSC_STRIKE_ATK_DEF", txtStrikeAtkDefense.Text);   // Strike_Atk_Defense
                    WriteElementWithValue(writer, "PSC_SUPER_KI_DEF", txtSuperKiBlastDefense.Text);       // Super_Ki_Blast_Defense

                    WriteElementWithValue(writer, "PSC_GROUND_SPEED", txtGroundSpeed.Text);     // Ground_Speed
                    WriteElementWithValue(writer, "PSC_AIR_SPEED", txtAirSpeed.Text);           // Air_Speed
                    WriteElementWithValue(writer, "PSC_BOOST_SPEED", txtBoostingSpeed.Text);       // Boosting_Speed
                    WriteElementWithValue(writer, "PSC_DASH_SPEED", txtDashSpeed.Text);         // Dash_Speed

                    WriteElementWithValue(writer, "PSC_F_124", txtF124.Text);                   // F_124
                    WriteElementWithValue(writer, "PSC_REINFORCEMENT_SKILL", txtReinforcementSkillDuration.Text); // Reinforcement_Skill_Duration
                    WriteElementWithValue(writer, "PSC_F_132", txtF132.Text);                   // F_132
                    WriteElementWithValue(writer, "PSC_REVIVAL_HP_AMOUNT", txtRevivalHpAmount.Text); // Revival_HP_Amount
                    WriteElementWithValue(writer, "PSC_F_140", txtF140.Text);                   // F_140
                    WriteElementWithValue(writer, "PSC_REVIVING_SPEED", txtRevivingSpeed.Text); // Reviving_Speed

                    WriteElementWithValue(writer, "PSC_I_148", txtI148.Text);
                    WriteElementWithValue(writer, "PSC_I_152", txtI152.Text);
                    WriteElementWithValue(writer, "PSC_I_156", txtI156.Text);
                    WriteElementWithValue(writer, "PSC_I_160", txtI160.Text);
                    WriteElementWithValue(writer, "PSC_I_164", txtI164.Text);
                    WriteElementWithValue(writer, "PSC_I_168", txtI168.Text);
                    WriteElementWithValue(writer, "PSC_I_172", txtI172.Text);
                    WriteElementWithValue(writer, "PSC_I_176", txtI176.Text);

                    WriteElementWithValue(writer, "PSC_SUPER_SOUL", txtSuperSoul.Text);                // Super_Soul / talisman
                    WriteElementWithValue(writer, "PSC_I_184", txtI184.Text);
                    WriteElementWithValue(writer, "PSC_I_188", txtI188.Text);
                    WriteElementWithValue(writer, "PSC_F_192", txtF192.Text);
                    WriteElementWithValue(writer, "PSC_NEW_I_20", txtNewI20.Text);             // NEW_I_20



                    WriteElementWithValue(writer, "MSG_CHARACTER_NAME", txtMSG1.Text);
                    WriteElementWithValue(writer, "MSG_COSTUME_NAME", txtMSG2.Text);

                    WriteElementWithValue(writer, "VOX_1", txtVOX1.Text);
                    WriteElementWithValue(writer, "VOX_2", txtVOX2.Text);

                    writer.WriteEndElement(); // Close xv2mod
                    writer.WriteEndDocument(); // Close the document
                }
                Directory.CreateDirectory("./XVCharaCreatorTemp/chara/");
                CopyDirectory(txtFolder.Text, @"./XVCharaCreatorTemp/chara/" + txtCharID.Text);
                Directory.CreateDirectory("./XVCharaCreatorTemp/ui/texture/CHARA01");
                File.Move(textBox1.Text, @"./XVCharaCreatorTemp/ui/texture/CHARA01/" + txtCharID.Text + "_000.DDS");
                Directory.CreateDirectory("./XVCharaCreatorTemp/JUNGLE");
                if (textBox2.Text.Length > 0 && Directory.GetDirectories(textBox2.Text).Length > 0 || Directory.GetFiles(textBox2.Text).Length > 0)
                    CopyDirectory(textBox2.Text, @"./XVCharaCreatorTemp/JUNGLE");
                if(File.Exists(sfd.FileName)) File.Delete(sfd.FileName);
                ZipFile.CreateFromDirectory(@"./XVCharaCreatorTemp/", sfd.FileName);
                if (File.Exists(xmlFilePath))
                    File.Delete(xmlFilePath);
                
                MessageBox.Show("Mod Created Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Directory.Delete("./XVCharaCreatorTemp", true);
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

        private void btnGenID_Click(object sender, EventArgs e)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] id = new char[3];
            string generatedID;

            for (int i = 0; i < 3; i++)
            {
                id[i] = chars[random.Next(chars.Length)];
            }

            generatedID = new string(id);


            txtCharID.Text = generatedID;
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            if (txtCharID.Text.Length == 3)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = $"Select {txtCharID.Text} Folder";
                fbd.UseDescriptionForTitle = true;

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = fbd.SelectedPath;
                    string selectedDirName = Path.GetFileName(selectedPath);

                    if (selectedDirName == txtCharID.Text && Directory.Exists(selectedPath))
                    {
                        txtFolder.Text = selectedPath;
                    }
                    else
                    {
                        MessageBox.Show($"Please select the folder matching {txtCharID.Text}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void txtAuraID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)  // Allow digits and Backspace
            {
                e.Handled = true;  // Mark the event as handled, preventing non-digit input
            }
        }

        private void txtVersion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)  // Allow digits and Backspace
            {
                e.Handled = true;  // Mark the event as handled, preventing non-digit input
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = $"Select .DDS file";
            ofd.Filter = "Direct Draw Surface files (*.DDS)|*.DDS";
            if (ofd.ShowDialog() == DialogResult.OK && File.Exists(ofd.FileName))
            {
                textBox1.Text = ofd.FileName;
            }
        }

        private void sToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
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
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Apri il file xv2mod (.xv2mod)
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open Mod File";
            ofd.Filter = "Xenoverse 2 Mod Files (*.xv2mod)|*.xv2mod";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string tempDir = "./XVCharaCreatorTemp2";
                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, true);
                Directory.CreateDirectory(tempDir);
                ZipFile.ExtractToDirectory(ofd.FileName, tempDir);
                string xmlFilePath = Path.Combine(tempDir, "xv2mod.xml");
                if (!File.Exists(xmlFilePath))
                {
                    MessageBox.Show(".xv2mod.xml not found", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (Directory.Exists(tempDir + @"/JUNGLE"))
                    textBox2.Text = tempDir + @"/JUNGLE";


                XDocument xmlDoc = XDocument.Load(xmlFilePath);
                XmlReader xmlReader = XmlReader.Create(xmlFilePath);

                string modType = xmlDoc.Descendants("XV2MOD").FirstOrDefault()?.Attribute("type")?.Value ?? "";
                if (modType != "ADDED_CHARACTER")
                {
                    throw new NotImplementedException($"Mod type {modType} not supported");
                }

                txtName.Text = xmlDoc.Descendants("MOD_NAME").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtAuthor.Text = xmlDoc.Descendants("MOD_AUTHOR").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtCharID.Text = xmlDoc.Descendants("CMS_BCS").FirstOrDefault()?.Attribute("value")?.Value ?? "";  //Too lazy to get it from filenames xD
                txtAuraID.Text = xmlDoc.Descendants("AUR_ID").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                cbAuraGlare.Checked = xmlDoc.Descendants("AUR_GLARE").FirstOrDefault()?.Attribute("value")?.Value == "1";

                txtBCS.Text = xmlDoc.Descendants("CMS_BCS").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                if (Directory.Exists(tempDir + @"/" + txtCharID.Text))
                    txtFolder.Text = tempDir + @"/" + txtCharID.Text;


                txtEAN.Text = xmlDoc.Descendants("CMS_EAN").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtFCEEAN.Text = xmlDoc.Descendants("CMS_FCE_EAN").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtCAMEAN.Text = xmlDoc.Descendants("CMS_CAM_EAN").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtBAC.Text = xmlDoc.Descendants("CMS_BAC").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtBCM.Text = xmlDoc.Descendants("CMS_BCM").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtBAI.Text = xmlDoc.Descendants("CMS_BAI").FirstOrDefault()?.Attribute("value")?.Value ?? "";

                txtCSO1.Text = xmlDoc.Descendants("CSO_1").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtCSO2.Text = xmlDoc.Descendants("CSO_2").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtCSO3.Text = xmlDoc.Descendants("CSO_3").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtCSO4.Text = xmlDoc.Descendants("CSO_4").FirstOrDefault()?.Attribute("value")?.Value ?? "";


                txtCUS1.Text = xmlDoc.Descendants("CUS_SUPER_1").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtCUS2.Text = xmlDoc.Descendants("CUS_SUPER_2").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtCUS3.Text = xmlDoc.Descendants("CUS_SUPER_3").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtCUS4.Text = xmlDoc.Descendants("CUS_SUPER_4").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtCUS5.Text = xmlDoc.Descendants("CUS_ULTIMATE_1").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtCUS6.Text = xmlDoc.Descendants("CUS_ULTIMATE_2").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtCUS7.Text = xmlDoc.Descendants("CUS_EVASIVE").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtCUS8.Text = xmlDoc.Descendants("CUS_KI_BLAST").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtCUS9.Text = xmlDoc.Descendants("CUS_AWOKEN").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                
                ReadElementAndAssignValuesPSC(xmlReader);

                txtMSG1.Text = xmlDoc.Descendants("MSG_CHARACTER_NAME").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtMSG2.Text = xmlDoc.Descendants("MSG_COSTUME_NAME").FirstOrDefault()?.Attribute("value")?.Value ?? "";

                txtVOX1.Text = xmlDoc.Descendants("VOX_1").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtVOX2.Text = xmlDoc.Descendants("VOX_2").FirstOrDefault()?.Attribute("value")?.Value ?? "";

                // Carica la directory del personaggio
                string charFolderPath = Path.Combine(tempDir, "chara", txtCharID.Text);
                if (Directory.Exists(charFolderPath))
                {
                    txtFolder.Text = charFolderPath;
                }

                // Carica l'immagine DDS
                string ddsFilePath = Path.Combine(tempDir, "ui", "texture", "CHARA01", txtCharID.Text + "_000.DDS");
                if (File.Exists(ddsFilePath))
                {
                    textBox1.Text = ddsFilePath;
                }
            }


        }

        void ReadElementAndAssignValuesPSC(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "PSC_COSTUME":
                            txtCostume.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_PRESET":
                            txtCostume2.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_CAMERA_POSITION":
                            txtCameraPosition.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_I_12":
                            txtI12.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_I_16":
                            txtI16.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_HEALTH":
                            txtHealth.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_F_24":
                            txtF24.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_KI":
                            txtKi.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_KI_RECHARGE":
                            txtKiRecharge.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_I_36":
                            txtI36.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_I_40":
                            txtI40.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_I_44":
                            txtI44.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_STAMINA":
                            txtStamina.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_STAMINA_RECHARGE_MOVE":
                            txtStaminaRechargeMove.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_STAMINA_RECHARGE_AIR":
                            txtStaminaRechargeAir.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_STAMINA_RECHARGE_GROUND":
                            txtStaminaRechargeGround.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_STAMINA_DRAIN_RATE_1":
                            txtStaminaDrainRate1.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_STAMINA_DRAIN_RATE_2":
                            txtStaminaDrainRate2.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_F_72":
                            txtF72.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_BASIC_ATK":
                            txtBasicAtk.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_BASIC_KI_ATK":
                            txtBasicKiAtk.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_STRIKE_ATK":
                            txtStrikeAtk.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_SUPER_KI_BLAST_ATK":
                            txtSuperKiBlastAtk.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_BASIC_ATK_DEF":
                            txtBasicAtkDefense.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_BASIC_KI_DEF":
                            txtBasicKiAtkDefense.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_STRIKE_ATK_DEF":
                            txtStrikeAtkDefense.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_SUPER_KI_DEF":
                            txtSuperKiBlastDefense.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_GROUND_SPEED":
                            txtGroundSpeed.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_AIR_SPEED":
                            txtAirSpeed.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_BOOST_SPEED":
                            txtBoostingSpeed.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_DASH_SPEED":
                            txtDashSpeed.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_F_124":
                            txtF124.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_REINFORCEMENT_SKILL":
                            txtReinforcementSkillDuration.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_F_132":
                            txtF132.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_REVIVAL_HP_AMOUNT":
                            txtRevivalHpAmount.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_F_140":
                            txtF140.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_REVIVING_SPEED":
                            txtRevivingSpeed.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_I_148":
                            txtI148.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_I_152":
                            txtI152.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_I_156":
                            txtI156.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_I_160":
                            txtI160.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_I_164":
                            txtI164.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_I_168":
                            txtI168.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_I_172":
                            txtI172.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_I_176":
                            txtI176.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_SUPER_SOUL":
                            txtSuperSoul.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_I_184":
                            txtI184.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_I_188":
                            txtI188.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_F_192":
                            txtF192.Text = reader.ReadElementContentAsString();
                            break;
                        case "PSC_NEW_I_20":
                            txtNewI20.Text = reader.ReadElementContentAsString();
                            break;
                    }
                }
            }
        }


        private void btnAdditionalFiles_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select data folder";
            if (fbd.ShowDialog() == DialogResult.OK)
                textBox2.Text = fbd.SelectedPath;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Directory.Exists("./XVCharaCreatorTemp"))
                Directory.Delete("./XVCharaCreatorTemp", true);
            if (Directory.Exists("./XVCharaCreatorTemp2"))
                Directory.Delete("./XVCharaCreatorTemp2", true);
        }
        private string ShowSkillSelection(List<string> skillsetDescriptions)
        {
            Form selectionForm = new Form()
            {
                Text = "Select a Skillset",
                Width = 400,
                Height = 400,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            ListBox listBox = new ListBox()
            {
                Dock = DockStyle.Top,
                Height = 300
            };
            listBox.Items.AddRange(skillsetDescriptions.ToArray());

            Button okButton = new Button()
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Dock = DockStyle.Bottom,
                Height = 40
            };

            Button cancelButton = new Button()
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Dock = DockStyle.Bottom,
                Height = 40
            };

            selectionForm.Controls.Add(listBox);
            selectionForm.Controls.Add(okButton);
            selectionForm.Controls.Add(cancelButton);

            selectionForm.AcceptButton = okButton;
            selectionForm.CancelButton = cancelButton;

            string selectedSkillset = null;

            if (selectionForm.ShowDialog() == DialogResult.OK && listBox.SelectedItem != null)
            {
                selectedSkillset = listBox.SelectedItem.ToString();
            }

            selectionForm.Dispose();
            return selectedSkillset;
        }

        private void copyValuesFromGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dataFolder = Path.Combine(Application.StartupPath, "server", "data");
            string serializerExePath = Path.Combine(dataFolder, "system", "XMLSerializer.exe");
            string cusFilePath = Path.Combine(dataFolder, "system", "custom_skill.cus");

            if(!Directory.Exists(dataFolder))
            {
                MessageBox.Show("Data folder not found! Start XV2Reborn first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(serializerExePath))
            {
                MessageBox.Show("XMLSerializer.exe not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Console.WriteLine($"Executing: {serializerExePath} {cusFilePath}");

            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                FileName = serializerExePath,
                Arguments = $"\"{cusFilePath}\"",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false
            };

            using (Process process = Process.Start(processInfo))
            {
                process.WaitForExit();
            }

            string cusXmlPath = Path.Combine(dataFolder, "system", "custom_skill.cus.xml");

            if (!File.Exists(cusXmlPath))
            {
                MessageBox.Show("Serialized XML file not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Console.WriteLine("Loading skillset data from XML...");

            XDocument cusXmlDoc = XDocument.Load(cusXmlPath);
            var skillsets = cusXmlDoc.Descendants("Skillset").ToList();

            if (!skillsets.Any())
            {
                MessageBox.Show("No skillsets found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<string> skillsetDescriptions = skillsets.Select(skillset =>
            {
                string charId = skillset.Attribute("Character_ID")?.Value ?? "0";
                string costumeIndex = skillset.Attribute("Costume_Index")?.Value ?? "0";
                return $"Character {charId} - Costume {costumeIndex}";
            }).ToList();

            string selectedDescription = ShowSkillSelection(skillsetDescriptions);

            if (string.IsNullOrEmpty(selectedDescription))
            {
                MessageBox.Show("No skillset selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Console.WriteLine($"Selected Skillset: {selectedDescription}");

            var parts = selectedDescription.Split(new[] { "Character ", " - Costume " }, StringSplitOptions.RemoveEmptyEntries);
            string selectedCharId = parts[0];
            string selectedCostumeIndex = parts[1];

            XElement selectedSkillset = skillsets.FirstOrDefault(s =>
                s.Attribute("Character_ID")?.Value == selectedCharId &&
                s.Attribute("Costume_Index")?.Value == selectedCostumeIndex
            );

            if (selectedSkillset == null)
            {
                MessageBox.Show("Skillset not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Console.WriteLine("Skillset found. Populating fields...");

            // Assegnazione diretta ai TextBox

            txtCUS1.Text = selectedSkillset.Element("SuperSkill1")?.Attribute("ID1")?.Value ?? "0";
            txtCUS2.Text = selectedSkillset.Element("SuperSkill2")?.Attribute("ID1")?.Value ?? "0";
            txtCUS3.Text = selectedSkillset.Element("SuperSkill3")?.Attribute("ID1")?.Value ?? "0";
            txtCUS4.Text = selectedSkillset.Element("SuperSkill4")?.Attribute("ID1")?.Value ?? "0";

            txtCUS5.Text = selectedSkillset.Element("UltimateSkill1")?.Attribute("ID1")?.Value ?? "0";
            txtCUS6.Text = selectedSkillset.Element("UltimateSkill2")?.Attribute("ID1")?.Value ?? "0";

            txtCUS7.Text = selectedSkillset.Element("EvasiveSkill")?.Attribute("ID1")?.Value ?? "0";
            txtCUS8.Text = selectedSkillset.Element("BlastType")?.Attribute("ID1")?.Value ?? "0";
            txtCUS9.Text = selectedSkillset.Element("AwokenSkill")?.Attribute("ID1")?.Value ?? "0";

        }

    }
}