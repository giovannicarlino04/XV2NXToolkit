using System.Windows.Forms;
using System;

public partial class InputForm : Form
{
    // Property to get the user input
    public string UserInput => txtInput.Text;

    public InputForm(string prompt, string defaultValue = "")
    {
        InitializeComponent();

        // Set the prompt text
        lblPrompt.Text = prompt;

        // Set the default value for the TextBox
        txtInput.Text = defaultValue;
    }

    // OK button click event
    private void btnOk_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtInput.Text))
        {
            MessageBox.Show("Input cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else
        {
            DialogResult = DialogResult.OK;
        }
    }

    // Cancel button click event
    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }

    private TextBox txtInput;
    private Button btnOk;
    private Button btnCancel;
    private Label lblPrompt;

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputForm));
            this.txtInput = new System.Windows.Forms.TextBox();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtInput
            // 
            this.txtInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtInput.Location = new System.Drawing.Point(0, 23);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(284, 20);
            this.txtInput.TabIndex = 0;
            // 
            // lblPrompt
            // 
            this.lblPrompt.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPrompt.Location = new System.Drawing.Point(0, 0);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(284, 23);
            this.lblPrompt.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnOk.Location = new System.Drawing.Point(0, 215);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(284, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(0, 238);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(284, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // InputForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.lblPrompt);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InputForm";
            this.Text = "Input";
            this.ResumeLayout(false);
            this.PerformLayout();

    }
}
