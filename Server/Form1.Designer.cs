namespace Server
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
        ///  Required method for Designer support — do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtPort = new TextBox();
            btnStart = new Button();
            btnSend = new Button();
            btnSaveXml = new Button();
            lstNotes = new ListBox();
            txtTitle = new TextBox();
            txtBody = new TextBox();
            txtTags = new TextBox();
            btnAddNote = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // txtPort
            // 
            txtPort.Location = new Point(15, 15);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(80, 23);
            txtPort.TabIndex = 0;
            txtPort.Text = "9000";
            txtPort.TextChanged += txtPort_TextChanged;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(110, 15);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(120, 23);
            btnStart.TabIndex = 1;
            btnStart.Text = "Start nasłuchu";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click_1;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(250, 15);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(120, 23);
            btnSend.TabIndex = 2;
            btnSend.Text = "Wyślij notatki";
            btnSend.UseVisualStyleBackColor = true;
            // 
            // btnSaveXml
            // 
            btnSaveXml.Location = new Point(390, 15);
            btnSaveXml.Name = "btnSaveXml";
            btnSaveXml.Size = new Size(120, 23);
            btnSaveXml.TabIndex = 3;
            btnSaveXml.Text = "Zapisz do XML";
            btnSaveXml.UseVisualStyleBackColor = true;
            // 
            // lstNotes
            // 
            lstNotes.FormattingEnabled = true;
            lstNotes.ItemHeight = 15;
            lstNotes.Location = new Point(15, 55);
            lstNotes.Name = "lstNotes";
            lstNotes.Size = new Size(360, 364);
            lstNotes.TabIndex = 4;
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(410, 75);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(230, 23);
            txtTitle.TabIndex = 5;
            // 
            // txtBody
            // 
            txtBody.Location = new Point(410, 130);
            txtBody.Multiline = true;
            txtBody.Name = "txtBody";
            txtBody.ScrollBars = ScrollBars.Vertical;
            txtBody.Size = new Size(230, 180);
            txtBody.TabIndex = 6;
            // 
            // txtTags
            // 
            txtTags.Location = new Point(410, 335);
            txtTags.Name = "txtTags";
            txtTags.Size = new Size(230, 23);
            txtTags.TabIndex = 7;
            // 
            // btnAddNote
            // 
            btnAddNote.Location = new Point(410, 375);
            btnAddNote.Name = "btnAddNote";
            btnAddNote.Size = new Size(230, 23);
            btnAddNote.TabIndex = 8;
            btnAddNote.Text = "Dodaj notatkę";
            btnAddNote.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.Location = new Point(410, 55);
            label1.Name = "label1";
            label1.Size = new Size(230, 20);
            label1.TabIndex = 2;
            label1.Text = "Tytuł:";
            // 
            // label2
            // 
            label2.Location = new Point(410, 110);
            label2.Name = "label2";
            label2.Size = new Size(230, 20);
            label2.TabIndex = 1;
            label2.Text = "Treść:";
            // 
            // label3
            // 
            label3.Location = new Point(410, 315);
            label3.Name = "label3";
            label3.Size = new Size(230, 20);
            label3.TabIndex = 0;
            label3.Text = "Tagi (oddzielone przecinkiem):";
            // 
            // Form1
            // 
            ClientSize = new Size(660, 440);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnAddNote);
            Controls.Add(txtTags);
            Controls.Add(txtBody);
            Controls.Add(txtTitle);
            Controls.Add(lstNotes);
            Controls.Add(btnSaveXml);
            Controls.Add(btnSend);
            Controls.Add(btnStart);
            Controls.Add(txtPort);
            Name = "Form1";
            Text = "Serwer Notatek";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnSaveXml;
        private System.Windows.Forms.ListBox lstNotes;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.TextBox txtBody;
        private System.Windows.Forms.TextBox txtTags;
        private System.Windows.Forms.Button btnAddNote;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
