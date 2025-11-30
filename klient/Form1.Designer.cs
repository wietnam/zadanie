namespace klient
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.TextBox textBoxNazwa;
        private System.Windows.Forms.TextBox textBoxHaslo;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.ListView listViewNotatki;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.TextBox textBoxTags;
        private System.Windows.Forms.TextBox textBoxBody;
        private System.Windows.Forms.Button buttonZapiszLocal;
        private System.Windows.Forms.Button buttonWyslij;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.textBoxIP = new System.Windows.Forms.TextBox() { Left = 10, Top = 10, Width = 120, Text = "127.0.0.1" };
            this.textBoxPort = new System.Windows.Forms.TextBox() { Left = 140, Top = 10, Width = 60, Text = "port" };
            this.textBoxNazwa = new System.Windows.Forms.TextBox() { Left = 210, Top = 10, Width = 120, Text = "imie" };
            this.textBoxHaslo = new System.Windows.Forms.TextBox() { Left = 340, Top = 10, Width = 120, UseSystemPasswordChar = true, Text = "haslo" };
            this.buttonConnect = new System.Windows.Forms.Button() { Left = 470, Top = 8, Width = 80, Text = "Connect" };
            this.buttonDisconnect = new System.Windows.Forms.Button() { Left = 560, Top = 8, Width = 80, Text = "Disconnect" };

            this.listViewNotatki = new System.Windows.Forms.ListView() { Left = 10, Top = 40, Width = 630, Height = 150, View = System.Windows.Forms.View.Details };
            this.listViewNotatki.Columns.Add("Tytuł", 300);
            this.listViewNotatki.Columns.Add("Data", 160);
            this.listViewNotatki.Columns.Add("Tagi", 160);

            this.textBoxTitle = new System.Windows.Forms.TextBox() { Left = 10, Top = 200, Width = 300 };
            this.textBoxTags = new System.Windows.Forms.TextBox() { Left = 320, Top = 200, Width = 320 };
            this.textBoxBody = new System.Windows.Forms.TextBox() { Left = 10, Top = 230, Width = 630, Height = 160, Multiline = true, ScrollBars = System.Windows.Forms.ScrollBars.Vertical };

            this.buttonZapiszLocal = new System.Windows.Forms.Button() { Left = 10, Top = 400, Width = 120, Text = "Zapisz lokalnie" };
            this.buttonWyslij = new System.Windows.Forms.Button() { Left = 140, Top = 400, Width = 120, Text = "Wyślij na serwer" };

            this.ClientSize = new System.Drawing.Size(660, 440);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                textBoxIP, textBoxPort, textBoxNazwa, textBoxHaslo, buttonConnect, buttonDisconnect,
                listViewNotatki, textBoxTitle, textBoxTags, textBoxBody, buttonZapiszLocal, buttonWyslij
            });
            this.Text = "Klient Notatek - 4P-Cloud";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.buttonConnect.Click += new System.EventHandler(this.Polacz_Click);
            this.buttonDisconnect.Click += new System.EventHandler(this.Rozlacz_Click);
            this.buttonZapiszLocal.Click += new System.EventHandler(this.ZapiszNotatkiLokalnie_Click);
            this.buttonWyslij.Click += new System.EventHandler(this.WyslijNotatkeNaSerwer_Click);
        }
    }
}
