﻿using System.Windows.Forms;

namespace XVCharaCreator
{
    partial class Form1
    {

        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            buildxv2modFileToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            sToolStripMenuItem = new ToolStripMenuItem();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            button2 = new Button();
            button1 = new Button();
            textBox2 = new TextBox();
            label30 = new Label();
            textBox1 = new TextBox();
            label27 = new Label();
            btnGenID = new Button();
            txtCharID = new TextBox();
            btnFolder = new Button();
            txtName = new TextBox();
            txtAuthor = new TextBox();
            txtFolder = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label5 = new Label();
            label1 = new Label();
            tabPage2 = new TabPage();
            txtAuraID = new TextBox();
            cbAuraGlare = new CheckBox();
            label6 = new Label();
            tabPage3 = new TabPage();
            txtBAI = new TextBox();
            label13 = new Label();
            txtBCM = new TextBox();
            label12 = new Label();
            txtBAC = new TextBox();
            label11 = new Label();
            txtCAMEAN = new TextBox();
            label10 = new Label();
            txtFCEEAN = new TextBox();
            label9 = new Label();
            txtEAN = new TextBox();
            label8 = new Label();
            txtBCS = new TextBox();
            label7 = new Label();
            tabPage4 = new TabPage();
            txtCSO4 = new TextBox();
            label14 = new Label();
            txtCSO3 = new TextBox();
            label15 = new Label();
            txtCSO2 = new TextBox();
            label16 = new Label();
            txtCSO1 = new TextBox();
            label17 = new Label();
            tabPage5 = new TabPage();
            txtCUS9 = new TextBox();
            txtCUS8 = new TextBox();
            txtCUS7 = new TextBox();
            txtCUS6 = new TextBox();
            txtCUS5 = new TextBox();
            txtCUS4 = new TextBox();
            txtCUS3 = new TextBox();
            label31 = new Label();
            txtCUS2 = new TextBox();
            label4 = new Label();
            txtCUS1 = new TextBox();
            label23 = new Label();
            label22 = new Label();
            label21 = new Label();
            label24 = new Label();
            label20 = new Label();
            label19 = new Label();
            label18 = new Label();
            tabPage6 = new TabPage();
            tabPage7 = new TabPage();
            label26 = new Label();
            label25 = new Label();
            txtMSG2 = new TextBox();
            txtMSG1 = new TextBox();
            tabPage8 = new TabPage();
            txtVOX2 = new TextBox();
            txtVOX1 = new TextBox();
            label29 = new Label();
            label28 = new Label();
            menuStrip2 = new MenuStrip();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            copyValuesFromGameToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            tabPage5.SuspendLayout();
            tabPage7.SuspendLayout();
            tabPage8.SuspendLayout();
            menuStrip2.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(706, 24);
            menuStrip1.TabIndex = 8;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, buildxv2modFileToolStripMenuItem, toolStripSeparator1, sToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(180, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // buildxv2modFileToolStripMenuItem
            // 
            buildxv2modFileToolStripMenuItem.Name = "buildxv2modFileToolStripMenuItem";
            buildxv2modFileToolStripMenuItem.Size = new Size(180, 22);
            buildxv2modFileToolStripMenuItem.Text = "Build xv2mod File";
            buildxv2modFileToolStripMenuItem.Click += buildxv2modFileToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(177, 6);
            // 
            // sToolStripMenuItem
            // 
            sToolStripMenuItem.Name = "sToolStripMenuItem";
            sToolStripMenuItem.Size = new Size(180, 22);
            sToolStripMenuItem.Text = "Exit";
            sToolStripMenuItem.Click += sToolStripMenuItem_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Controls.Add(tabPage6);
            tabControl1.Controls.Add(tabPage7);
            tabControl1.Controls.Add(tabPage8);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 24);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(706, 678);
            tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = SystemColors.Control;
            tabPage1.Controls.Add(button2);
            tabPage1.Controls.Add(button1);
            tabPage1.Controls.Add(textBox2);
            tabPage1.Controls.Add(label30);
            tabPage1.Controls.Add(textBox1);
            tabPage1.Controls.Add(label27);
            tabPage1.Controls.Add(btnGenID);
            tabPage1.Controls.Add(txtCharID);
            tabPage1.Controls.Add(btnFolder);
            tabPage1.Controls.Add(txtName);
            tabPage1.Controls.Add(txtAuthor);
            tabPage1.Controls.Add(txtFolder);
            tabPage1.Controls.Add(label3);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(label5);
            tabPage1.Controls.Add(label1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(698, 650);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Mod Info";
            // 
            // button2
            // 
            button2.Location = new Point(318, 229);
            button2.Name = "button2";
            button2.Size = new Size(33, 23);
            button2.TabIndex = 30;
            button2.Text = "...";
            button2.UseVisualStyleBackColor = true;
            button2.Click += btnAdditionalFiles_Click;
            // 
            // button1
            // 
            button1.Location = new Point(317, 189);
            button1.Name = "button1";
            button1.Size = new Size(33, 23);
            button1.TabIndex = 30;
            button1.Text = "...";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(142, 229);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(170, 23);
            textBox2.TabIndex = 29;
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(29, 233);
            label30.Name = "label30";
            label30.Size = new Size(92, 15);
            label30.TabIndex = 28;
            label30.Text = "Additional Data:";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(141, 189);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(170, 23);
            textBox1.TabIndex = 29;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(28, 193);
            label27.Name = "label27";
            label27.Size = new Size(103, 15);
            label27.TabIndex = 28;
            label27.Text = "Character Portrait:";
            // 
            // btnGenID
            // 
            btnGenID.Location = new Point(213, 110);
            btnGenID.Name = "btnGenID";
            btnGenID.Size = new Size(75, 23);
            btnGenID.TabIndex = 27;
            btnGenID.Text = "Generate";
            btnGenID.UseVisualStyleBackColor = true;
            btnGenID.Click += btnGenID_Click;
            // 
            // txtCharID
            // 
            txtCharID.CharacterCasing = CharacterCasing.Upper;
            txtCharID.Location = new Point(141, 110);
            txtCharID.MaxLength = 3;
            txtCharID.Name = "txtCharID";
            txtCharID.Size = new Size(66, 23);
            txtCharID.TabIndex = 26;
            // 
            // btnFolder
            // 
            btnFolder.Location = new Point(317, 150);
            btnFolder.Name = "btnFolder";
            btnFolder.Size = new Size(33, 23);
            btnFolder.TabIndex = 25;
            btnFolder.Text = "...";
            btnFolder.UseVisualStyleBackColor = true;
            btnFolder.Click += btnFolder_Click;
            // 
            // txtName
            // 
            txtName.Location = new Point(141, 34);
            txtName.Name = "txtName";
            txtName.Size = new Size(170, 23);
            txtName.TabIndex = 21;
            // 
            // txtAuthor
            // 
            txtAuthor.Location = new Point(141, 70);
            txtAuthor.Name = "txtAuthor";
            txtAuthor.Size = new Size(170, 23);
            txtAuthor.TabIndex = 23;
            // 
            // txtFolder
            // 
            txtFolder.Location = new Point(141, 150);
            txtFolder.Name = "txtFolder";
            txtFolder.Size = new Size(170, 23);
            txtFolder.TabIndex = 24;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(27, 37);
            label3.Name = "label3";
            label3.Size = new Size(70, 15);
            label3.TabIndex = 16;
            label3.Text = "Mod Name:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 73);
            label2.Name = "label2";
            label2.Size = new Size(75, 15);
            label2.TabIndex = 18;
            label2.Text = "Mod Author:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(56, 113);
            label5.Name = "label5";
            label5.Size = new Size(49, 15);
            label5.TabIndex = 19;
            label5.Text = "Char ID:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(28, 154);
            label1.Name = "label1";
            label1.Size = new Size(97, 15);
            label1.TabIndex = 20;
            label1.Text = "Character Folder:";
            // 
            // tabPage2
            // 
            tabPage2.BackColor = SystemColors.Control;
            tabPage2.Controls.Add(txtAuraID);
            tabPage2.Controls.Add(cbAuraGlare);
            tabPage2.Controls.Add(label6);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(698, 650);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "AUR";
            // 
            // txtAuraID
            // 
            txtAuraID.Location = new Point(99, 28);
            txtAuraID.Name = "txtAuraID";
            txtAuraID.Size = new Size(100, 23);
            txtAuraID.TabIndex = 3;
            txtAuraID.KeyPress += txtAuraID_KeyPress;
            // 
            // cbAuraGlare
            // 
            cbAuraGlare.AutoSize = true;
            cbAuraGlare.Location = new Point(205, 32);
            cbAuraGlare.Name = "cbAuraGlare";
            cbAuraGlare.Size = new Size(53, 19);
            cbAuraGlare.TabIndex = 2;
            cbAuraGlare.Text = "Glare";
            cbAuraGlare.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(44, 33);
            label6.Name = "label6";
            label6.Size = new Size(49, 15);
            label6.TabIndex = 1;
            label6.Text = "Aura ID:";
            // 
            // tabPage3
            // 
            tabPage3.BackColor = SystemColors.Control;
            tabPage3.Controls.Add(txtBAI);
            tabPage3.Controls.Add(label13);
            tabPage3.Controls.Add(txtBCM);
            tabPage3.Controls.Add(label12);
            tabPage3.Controls.Add(txtBAC);
            tabPage3.Controls.Add(label11);
            tabPage3.Controls.Add(txtCAMEAN);
            tabPage3.Controls.Add(label10);
            tabPage3.Controls.Add(txtFCEEAN);
            tabPage3.Controls.Add(label9);
            tabPage3.Controls.Add(txtEAN);
            tabPage3.Controls.Add(label8);
            tabPage3.Controls.Add(txtBCS);
            tabPage3.Controls.Add(label7);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(698, 650);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "CMS";
            // 
            // txtBAI
            // 
            txtBAI.Location = new Point(111, 213);
            txtBAI.Name = "txtBAI";
            txtBAI.Size = new Size(218, 23);
            txtBAI.TabIndex = 5;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(39, 216);
            label13.Name = "label13";
            label13.Size = new Size(28, 15);
            label13.TabIndex = 4;
            label13.Text = "BAI:";
            // 
            // txtBCM
            // 
            txtBCM.Location = new Point(111, 184);
            txtBCM.Name = "txtBCM";
            txtBCM.Size = new Size(218, 23);
            txtBCM.TabIndex = 5;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(39, 187);
            label12.Name = "label12";
            label12.Size = new Size(36, 15);
            label12.TabIndex = 4;
            label12.Text = "BCM:";
            // 
            // txtBAC
            // 
            txtBAC.Location = new Point(111, 155);
            txtBAC.Name = "txtBAC";
            txtBAC.Size = new Size(218, 23);
            txtBAC.TabIndex = 5;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(39, 158);
            label11.Name = "label11";
            label11.Size = new Size(33, 15);
            label11.TabIndex = 4;
            label11.Text = "BAC:";
            // 
            // txtCAMEAN
            // 
            txtCAMEAN.Location = new Point(111, 126);
            txtCAMEAN.Name = "txtCAMEAN";
            txtCAMEAN.Size = new Size(218, 23);
            txtCAMEAN.TabIndex = 5;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(39, 129);
            label10.Name = "label10";
            label10.Size = new Size(65, 15);
            label10.TabIndex = 4;
            label10.Text = "CAM_EAN:";
            // 
            // txtFCEEAN
            // 
            txtFCEEAN.Location = new Point(111, 97);
            txtFCEEAN.Name = "txtFCEEAN";
            txtFCEEAN.Size = new Size(218, 23);
            txtFCEEAN.TabIndex = 5;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(39, 100);
            label9.Name = "label9";
            label9.Size = new Size(58, 15);
            label9.TabIndex = 4;
            label9.Text = "FCE_EAN:";
            // 
            // txtEAN
            // 
            txtEAN.Location = new Point(111, 68);
            txtEAN.Name = "txtEAN";
            txtEAN.Size = new Size(218, 23);
            txtEAN.TabIndex = 5;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(39, 71);
            label8.Name = "label8";
            label8.Size = new Size(33, 15);
            label8.TabIndex = 4;
            label8.Text = "EAN:";
            // 
            // txtBCS
            // 
            txtBCS.Location = new Point(111, 39);
            txtBCS.Name = "txtBCS";
            txtBCS.Size = new Size(218, 23);
            txtBCS.TabIndex = 5;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(39, 42);
            label7.Name = "label7";
            label7.Size = new Size(31, 15);
            label7.TabIndex = 4;
            label7.Text = "BCS:";
            // 
            // tabPage4
            // 
            tabPage4.BackColor = SystemColors.Control;
            tabPage4.Controls.Add(txtCSO4);
            tabPage4.Controls.Add(label14);
            tabPage4.Controls.Add(txtCSO3);
            tabPage4.Controls.Add(label15);
            tabPage4.Controls.Add(txtCSO2);
            tabPage4.Controls.Add(label16);
            tabPage4.Controls.Add(txtCSO1);
            tabPage4.Controls.Add(label17);
            tabPage4.Location = new Point(4, 24);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(698, 650);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "CSO";
            // 
            // txtCSO4
            // 
            txtCSO4.Location = new Point(101, 138);
            txtCSO4.Name = "txtCSO4";
            txtCSO4.Size = new Size(218, 23);
            txtCSO4.TabIndex = 10;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(70, 141);
            label14.Name = "label14";
            label14.Size = new Size(16, 15);
            label14.TabIndex = 6;
            label14.Text = "4:";
            // 
            // txtCSO3
            // 
            txtCSO3.Location = new Point(101, 109);
            txtCSO3.Name = "txtCSO3";
            txtCSO3.Size = new Size(218, 23);
            txtCSO3.TabIndex = 11;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(70, 112);
            label15.Name = "label15";
            label15.Size = new Size(16, 15);
            label15.TabIndex = 7;
            label15.Text = "3:";
            // 
            // txtCSO2
            // 
            txtCSO2.Location = new Point(101, 80);
            txtCSO2.Name = "txtCSO2";
            txtCSO2.Size = new Size(218, 23);
            txtCSO2.TabIndex = 12;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(70, 83);
            label16.Name = "label16";
            label16.Size = new Size(16, 15);
            label16.TabIndex = 8;
            label16.Text = "2:";
            // 
            // txtCSO1
            // 
            txtCSO1.Location = new Point(101, 51);
            txtCSO1.Name = "txtCSO1";
            txtCSO1.Size = new Size(218, 23);
            txtCSO1.TabIndex = 13;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(70, 54);
            label17.Name = "label17";
            label17.Size = new Size(16, 15);
            label17.TabIndex = 9;
            label17.Text = "1:";
            // 
            // tabPage5
            // 
            tabPage5.BackColor = SystemColors.Control;
            tabPage5.Controls.Add(menuStrip2);
            tabPage5.Controls.Add(txtCUS9);
            tabPage5.Controls.Add(txtCUS8);
            tabPage5.Controls.Add(txtCUS7);
            tabPage5.Controls.Add(txtCUS6);
            tabPage5.Controls.Add(txtCUS5);
            tabPage5.Controls.Add(txtCUS4);
            tabPage5.Controls.Add(txtCUS3);
            tabPage5.Controls.Add(label31);
            tabPage5.Controls.Add(txtCUS2);
            tabPage5.Controls.Add(label4);
            tabPage5.Controls.Add(txtCUS1);
            tabPage5.Controls.Add(label23);
            tabPage5.Controls.Add(label22);
            tabPage5.Controls.Add(label21);
            tabPage5.Controls.Add(label24);
            tabPage5.Controls.Add(label20);
            tabPage5.Controls.Add(label19);
            tabPage5.Controls.Add(label18);
            tabPage5.Location = new Point(4, 24);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(698, 650);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "CUS";
            // 
            // txtCUS9
            // 
            txtCUS9.Location = new Point(143, 272);
            txtCUS9.Name = "txtCUS9";
            txtCUS9.Size = new Size(479, 23);
            txtCUS9.TabIndex = 2;
            // 
            // txtCUS8
            // 
            txtCUS8.Location = new Point(143, 243);
            txtCUS8.Name = "txtCUS8";
            txtCUS8.Size = new Size(479, 23);
            txtCUS8.TabIndex = 2;
            // 
            // txtCUS7
            // 
            txtCUS7.Location = new Point(143, 214);
            txtCUS7.Name = "txtCUS7";
            txtCUS7.Size = new Size(479, 23);
            txtCUS7.TabIndex = 2;
            // 
            // txtCUS6
            // 
            txtCUS6.Location = new Point(143, 188);
            txtCUS6.Name = "txtCUS6";
            txtCUS6.Size = new Size(479, 23);
            txtCUS6.TabIndex = 2;
            // 
            // txtCUS5
            // 
            txtCUS5.Location = new Point(143, 159);
            txtCUS5.Name = "txtCUS5";
            txtCUS5.Size = new Size(479, 23);
            txtCUS5.TabIndex = 2;
            // 
            // txtCUS4
            // 
            txtCUS4.Location = new Point(143, 130);
            txtCUS4.Name = "txtCUS4";
            txtCUS4.Size = new Size(479, 23);
            txtCUS4.TabIndex = 2;
            // 
            // txtCUS3
            // 
            txtCUS3.Location = new Point(143, 100);
            txtCUS3.Name = "txtCUS3";
            txtCUS3.Size = new Size(479, 23);
            txtCUS3.TabIndex = 2;
            // 
            // label31
            // 
            label31.AutoSize = true;
            label31.Location = new Point(73, 275);
            label31.Name = "label31";
            label31.Size = new Size(53, 15);
            label31.TabIndex = 1;
            label31.Text = "Awoken:";
            // 
            // txtCUS2
            // 
            txtCUS2.Location = new Point(143, 71);
            txtCUS2.Name = "txtCUS2";
            txtCUS2.Size = new Size(479, 23);
            txtCUS2.TabIndex = 2;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(73, 246);
            label4.Name = "label4";
            label4.Size = new Size(48, 15);
            label4.TabIndex = 1;
            label4.Text = "Ki Blast:";
            // 
            // txtCUS1
            // 
            txtCUS1.Location = new Point(143, 42);
            txtCUS1.Name = "txtCUS1";
            txtCUS1.Size = new Size(479, 23);
            txtCUS1.TabIndex = 2;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(73, 217);
            label23.Name = "label23";
            label23.Size = new Size(48, 15);
            label23.TabIndex = 1;
            label23.Text = "Evasive:";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(58, 191);
            label22.Name = "label22";
            label22.Size = new Size(64, 15);
            label22.TabIndex = 1;
            label22.Text = "Ultimate 2:";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(58, 162);
            label21.Name = "label21";
            label21.Size = new Size(64, 15);
            label21.TabIndex = 1;
            label21.Text = "Ultimate 1:";
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(78, 133);
            label24.Name = "label24";
            label24.Size = new Size(49, 15);
            label24.TabIndex = 1;
            label24.Text = "Super 4:";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(78, 103);
            label20.Name = "label20";
            label20.Size = new Size(49, 15);
            label20.TabIndex = 1;
            label20.Text = "Super 3:";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(78, 74);
            label19.Name = "label19";
            label19.Size = new Size(49, 15);
            label19.TabIndex = 1;
            label19.Text = "Super 2:";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(78, 45);
            label18.Name = "label18";
            label18.Size = new Size(49, 15);
            label18.TabIndex = 1;
            label18.Text = "Super 1:";
            // 
            // tabPage6
            // 
            tabPage6.Location = new Point(4, 24);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(3);
            tabPage6.Size = new Size(698, 650);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "PSC";
            // 
            // tabPage7
            // 
            tabPage7.BackColor = SystemColors.Control;
            tabPage7.Controls.Add(label26);
            tabPage7.Controls.Add(label25);
            tabPage7.Controls.Add(txtMSG2);
            tabPage7.Controls.Add(txtMSG1);
            tabPage7.Location = new Point(4, 24);
            tabPage7.Name = "tabPage7";
            tabPage7.Padding = new Padding(3);
            tabPage7.Size = new Size(698, 650);
            tabPage7.TabIndex = 6;
            tabPage7.Text = "MSG";
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(37, 70);
            label26.Name = "label26";
            label26.Size = new Size(93, 15);
            label26.TabIndex = 1;
            label26.Text = "Costume Name:";
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(37, 41);
            label25.Name = "label25";
            label25.Size = new Size(96, 15);
            label25.TabIndex = 1;
            label25.Text = "Character Name:";
            // 
            // txtMSG2
            // 
            txtMSG2.Location = new Point(139, 67);
            txtMSG2.Name = "txtMSG2";
            txtMSG2.Size = new Size(248, 23);
            txtMSG2.TabIndex = 0;
            // 
            // txtMSG1
            // 
            txtMSG1.Location = new Point(139, 38);
            txtMSG1.Name = "txtMSG1";
            txtMSG1.Size = new Size(248, 23);
            txtMSG1.TabIndex = 0;
            // 
            // tabPage8
            // 
            tabPage8.BackColor = SystemColors.Control;
            tabPage8.Controls.Add(txtVOX2);
            tabPage8.Controls.Add(txtVOX1);
            tabPage8.Controls.Add(label29);
            tabPage8.Controls.Add(label28);
            tabPage8.Location = new Point(4, 24);
            tabPage8.Name = "tabPage8";
            tabPage8.Padding = new Padding(3);
            tabPage8.Size = new Size(698, 650);
            tabPage8.TabIndex = 7;
            tabPage8.Text = "VOX";
            // 
            // txtVOX2
            // 
            txtVOX2.Location = new Point(99, 51);
            txtVOX2.Name = "txtVOX2";
            txtVOX2.Size = new Size(47, 23);
            txtVOX2.TabIndex = 1;
            // 
            // txtVOX1
            // 
            txtVOX1.Location = new Point(46, 51);
            txtVOX1.Name = "txtVOX1";
            txtVOX1.Size = new Size(47, 23);
            txtVOX1.TabIndex = 1;
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new Point(99, 33);
            label29.Name = "label29";
            label29.Size = new Size(47, 15);
            label29.TabIndex = 0;
            label29.Text = "Voice 2:";
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(46, 33);
            label28.Name = "label28";
            label28.Size = new Size(47, 15);
            label28.TabIndex = 0;
            label28.Text = "Voice 1:";
            // 
            // menuStrip2
            // 
            menuStrip2.Items.AddRange(new ToolStripItem[] { toolsToolStripMenuItem });
            menuStrip2.Location = new Point(3, 3);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Size = new Size(692, 24);
            menuStrip2.TabIndex = 9;
            menuStrip2.Text = "menuStrip2";
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { copyValuesFromGameToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(46, 20);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // copyValuesFromGameToolStripMenuItem
            // 
            copyValuesFromGameToolStripMenuItem.Name = "copyValuesFromGameToolStripMenuItem";
            copyValuesFromGameToolStripMenuItem.Size = new Size(200, 22);
            copyValuesFromGameToolStripMenuItem.Text = "Copy values from game";
            copyValuesFromGameToolStripMenuItem.Click += copyValuesFromGameToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(706, 702);
            Controls.Add(tabControl1);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            Text = "XVCharaCreator";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            tabPage5.ResumeLayout(false);
            tabPage5.PerformLayout();
            tabPage7.ResumeLayout(false);
            tabPage7.PerformLayout();
            tabPage8.ResumeLayout(false);
            tabPage8.PerformLayout();
            menuStrip2.ResumeLayout(false);
            menuStrip2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem buildxv2modFileToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem sToolStripMenuItem;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button btnGenID;
        private TextBox txtCharID;
        private Button btnFolder;
        private TextBox txtName;
        private TextBox txtAuthor;
        private TextBox txtFolder;
        private Label label3;
        private Label label2;
        private Label label5;
        private Label label1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TabPage tabPage6;
        private TabPage tabPage7;
        private CheckBox cbAuraGlare;
        private Label label6;
        private TextBox txtAuraID;
        private TextBox txtBAC;
        private Label label11;
        private TextBox txtCAMEAN;
        private Label label10;
        private TextBox txtFCEEAN;
        private Label label9;
        private TextBox txtEAN;
        private Label label8;
        private TextBox txtBCS;
        private Label label7;
        private TextBox txtBAI;
        private Label label13;
        private TextBox txtBCM;
        private Label label12;
        private TextBox txtCSO4;
        private Label label14;
        private TextBox txtCSO3;
        private Label label15;
        private TextBox txtCSO2;
        private Label label16;
        private TextBox txtCSO1;
        private Label label17;
        private Label label20;
        private Label label19;
        private Label label18;
        private Label label23;
        private Label label22;
        private Label label21;
        private Label label24;
        private Label label25;
        private TextBox txtMSG2;
        private TextBox txtMSG1;
        private Label label26;
        private Button button1;
        private TextBox textBox1;
        private Label label27;
        private TabPage tabPage8;
        private Label label28;
        private TextBox txtVOX2;
        private TextBox txtVOX1;
        private Label label29;
        private ToolStripMenuItem openToolStripMenuItem;
        private Button button2;
        private TextBox textBox2;
        private Label label30;
        private TextBox txtCostume;
        private TextBox txtCostume2;
        private TextBox txtCameraPosition;
        private TextBox txtI12;
        private TextBox txtI16;
        private TextBox txtHealth;
        private TextBox txtF24;
        private TextBox txtKi;
        private TextBox txtKiRecharge;
        private TextBox txtI36;
        private TextBox txtI40;
        private TextBox txtI44;
        private TextBox txtStamina;
        private TextBox txtStaminaRechargeMove;
        private TextBox txtStaminaRechargeAir;
        private TextBox txtStaminaRechargeGround;
        private TextBox txtStaminaDrainRate1;
        private TextBox txtStaminaDrainRate2;
        private TextBox txtF72;
        private TextBox txtBasicAtk;
        private TextBox txtBasicKiAtk;
        private TextBox txtStrikeAtk;
        private TextBox txtSuperKiBlastAtk;
        private TextBox txtBasicAtkDefense;
        private TextBox txtBasicKiAtkDefense;
        private TextBox txtStrikeAtkDefense;
        private TextBox txtSuperKiBlastDefense;
        private TextBox txtGroundSpeed;
        private TextBox txtAirSpeed;
        private TextBox txtBoostingSpeed;
        private TextBox txtDashSpeed;
        private TextBox txtF124;
        private TextBox txtReinforcementSkillDuration;
        private TextBox txtF132;
        private TextBox txtRevivalHpAmount;
        private TextBox txtF140;
        private TextBox txtRevivingSpeed;
        private TextBox txtI148;
        private TextBox txtI152;
        private TextBox txtI156;
        private TextBox txtI160;
        private TextBox txtI164;
        private TextBox txtI168;
        private TextBox txtI172;
        private TextBox txtI176;
        private TextBox txtSuperSoul;
        private TextBox txtI184;
        private TextBox txtI188;
        private TextBox txtF192;
        private TextBox txtNewI20;

        private TextBox txtCUS5;
        private TextBox txtCUS4;
        private TextBox txtCUS3;
        private TextBox txtCUS2;
        private TextBox txtCUS1;
        private TextBox txtCUS7;
        private TextBox txtCUS6;
        private TextBox txtCUS9;
        private TextBox txtCUS8;
        private Label label31;
        private Label label4;
        private MenuStrip menuStrip2;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem copyValuesFromGameToolStripMenuItem;
    }
}