namespace Klient
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.txtServerIp = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lstNotes = new System.Windows.Forms.ListBox();
            this.txtNoteBody = new System.Windows.Forms.TextBox();
            this.btnSaveLocal = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblIp = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblPass = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtServerIp
            // 
            this.txtServerIp.Location = new System.Drawing.Point(70, 12);
            this.txtServerIp.Name = "txtServerIp";
            this.txtServerIp.Size = new System.Drawing.Size(140, 23);
            this.txtServerIp.TabIndex = 0;
            this.txtServerIp.Text = "127.0.0.1";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(260, 12);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(60, 23);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "9000";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(360, 12);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(140, 23);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.Text = "haslo";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(520, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 23);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // lstNotes
            // 
            this.lstNotes.FormattingEnabled = true;
            this.lstNotes.ItemHeight = 15;
            this.lstNotes.Location = new System.Drawing.Point(12, 60);
            this.lstNotes.Name = "lstNotes";
            this.lstNotes.Size = new System.Drawing.Size(300, 364);
            this.lstNotes.TabIndex = 4;
            // 
            // txtNoteBody
            // 
            this.txtNoteBody.Location = new System.Drawing.Point(330, 60);
            this.txtNoteBody.Multiline = true;
            this.txtNoteBody.Name = "txtNoteBody";
            this.txtNoteBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNoteBody.Size = new System.Drawing.Size(420, 330);
            this.txtNoteBody.TabIndex = 5;
            // 
            // btnSaveLocal
            // 
            this.btnSaveLocal.Location = new System.Drawing.Point(330, 400);
            this.btnSaveLocal.Name = "btnSaveLocal";
            this.btnSaveLocal.Size = new System.Drawing.Size(140, 23);
            this.btnSaveLocal.TabIndex = 6;
            this.btnSaveLocal.Text = "Save locally (XML)";
            this.btnSaveLocal.UseVisualStyleBackColor = true;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(12, 440);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(220, 23);
            this.txtSearch.TabIndex = 7;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(250, 440);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(62, 23);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // lblIp
            // 
            this.lblIp.Location = new System.Drawing.Point(12, 15);
            this.lblIp.Name = "lblIp";
            this.lblIp.Size = new System.Drawing.Size(52, 18);
            this.lblIp.Text = "Server IP:";
            // 
            // lblPort
            // 
            this.lblPort.Location = new System.Drawing.Point(220, 15);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(40, 18);
            this.lblPort.Text = "Port:";
            // 
            // lblPass
            // 
            this.lblPass.Location = new System.Drawing.Point(320, 15);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(40, 18);
            this.lblPass.Text = "Pass:";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(764, 480);
            this.Controls.Add(this.lblPass);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.lblIp);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnSaveLocal);
            this.Controls.Add(this.txtNoteBody);
            this.Controls.Add(this.lstNotes);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtServerIp);
            this.Name = "Form1";
            this.Text = "Klient Notatek";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtServerIp;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ListBox lstNotes;
        private System.Windows.Forms.TextBox txtNoteBody;
        private System.Windows.Forms.Button btnSaveLocal;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblIp;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblPass;
    }
}
