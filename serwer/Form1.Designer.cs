namespace serwer
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.ListView listViewSerwer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.textBoxPort = new System.Windows.Forms.TextBox() { Left = 10, Top = 10, Width = 100, Text = "5000" };
            this.buttonStart = new System.Windows.Forms.Button() { Left = 120, Top = 8, Width = 80, Text = "Start" };
            this.buttonStop = new System.Windows.Forms.Button() { Left = 210, Top = 8, Width = 80, Text = "Stop" };
            this.listViewSerwer = new System.Windows.Forms.ListView() { Left = 10, Top = 40, Width = 560, Height = 380, View = System.Windows.Forms.View.Details };
            this.listViewSerwer.Columns.Add("Tytuł", 300);
            this.listViewSerwer.Columns.Add("Autor", 140);
            this.listViewSerwer.Columns.Add("Czas", 120);

            this.ClientSize = new System.Drawing.Size(590, 430);
            this.Controls.AddRange(new System.Windows.Forms.Control[] { textBoxPort, buttonStart, buttonStop, listViewSerwer });
            this.Text = "Serwer Notatek";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.buttonStart.Click += new System.EventHandler(this.StartSerwera_Click);
            this.buttonStop.Click += new System.EventHandler(this.StopSerwera_Click);
        }
    }
}
