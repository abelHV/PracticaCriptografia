namespace ClientCriptografia
{
    partial class FrmMain
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
            btStop = new Button();
            lbLog = new ListBox();
            gbRemot = new GroupBox();
            tbNom = new TextBox();
            lbNom = new Label();
            tbIpremota = new TextBox();
            lbIP = new Label();
            lbPortParlar = new Label();
            btParlar = new Button();
            nupPortParlar = new NumericUpDown();
            tbRebre = new TextBox();
            gbEnviar = new GroupBox();
            btEnviar = new Button();
            tbEnviar = new TextBox();
            lbPersonas = new ListBox();
            gbRemot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nupPortParlar).BeginInit();
            gbEnviar.SuspendLayout();
            SuspendLayout();
            // 
            // btStop
            // 
            btStop.BackColor = Color.Red;
            btStop.FlatStyle = FlatStyle.Flat;
            btStop.ForeColor = Color.White;
            btStop.Location = new Point(45, 214);
            btStop.Name = "btStop";
            btStop.Size = new Size(108, 29);
            btStop.TabIndex = 22;
            btStop.Text = "stop";
            btStop.UseVisualStyleBackColor = false;
            btStop.Visible = false;
            btStop.Click += btStop_Click;
            // 
            // lbLog
            // 
            lbLog.FormattingEnabled = true;
            lbLog.Location = new Point(28, 546);
            lbLog.Name = "lbLog";
            lbLog.Size = new Size(960, 164);
            lbLog.TabIndex = 21;
            // 
            // gbRemot
            // 
            gbRemot.Controls.Add(tbNom);
            gbRemot.Controls.Add(lbNom);
            gbRemot.Controls.Add(tbIpremota);
            gbRemot.Controls.Add(btStop);
            gbRemot.Controls.Add(lbIP);
            gbRemot.Controls.Add(lbPortParlar);
            gbRemot.Controls.Add(btParlar);
            gbRemot.Controls.Add(nupPortParlar);
            gbRemot.Location = new Point(14, 29);
            gbRemot.Name = "gbRemot";
            gbRemot.Size = new Size(195, 273);
            gbRemot.TabIndex = 20;
            gbRemot.TabStop = false;
            gbRemot.Text = "** amb qui parlar **";
            // 
            // tbNom
            // 
            tbNom.Location = new Point(92, 137);
            tbNom.Name = "tbNom";
            tbNom.Size = new Size(91, 27);
            tbNom.TabIndex = 24;
            tbNom.TextAlign = HorizontalAlignment.Center;
            // 
            // lbNom
            // 
            lbNom.BackColor = Color.Sienna;
            lbNom.BorderStyle = BorderStyle.FixedSingle;
            lbNom.ForeColor = Color.White;
            lbNom.Location = new Point(15, 137);
            lbNom.Name = "lbNom";
            lbNom.Padding = new Padding(2);
            lbNom.Size = new Size(57, 27);
            lbNom.TabIndex = 23;
            lbNom.Text = "NOM";
            // 
            // tbIpremota
            // 
            tbIpremota.Location = new Point(66, 37);
            tbIpremota.Name = "tbIpremota";
            tbIpremota.Size = new Size(117, 27);
            tbIpremota.TabIndex = 11;
            tbIpremota.TextAlign = HorizontalAlignment.Center;
            // 
            // lbIP
            // 
            lbIP.BackColor = Color.Sienna;
            lbIP.BorderStyle = BorderStyle.FixedSingle;
            lbIP.ForeColor = Color.White;
            lbIP.Location = new Point(15, 37);
            lbIP.Name = "lbIP";
            lbIP.Padding = new Padding(2);
            lbIP.Size = new Size(45, 22);
            lbIP.TabIndex = 10;
            lbIP.Text = "IP";
            // 
            // lbPortParlar
            // 
            lbPortParlar.AutoSize = true;
            lbPortParlar.BackColor = Color.Sienna;
            lbPortParlar.BorderStyle = BorderStyle.FixedSingle;
            lbPortParlar.ForeColor = Color.White;
            lbPortParlar.Location = new Point(15, 86);
            lbPortParlar.Name = "lbPortParlar";
            lbPortParlar.Padding = new Padding(2);
            lbPortParlar.Size = new Size(82, 26);
            lbPortParlar.TabIndex = 5;
            lbPortParlar.Text = "nº de port";
            // 
            // btParlar
            // 
            btParlar.BackColor = Color.ForestGreen;
            btParlar.FlatStyle = FlatStyle.Flat;
            btParlar.ForeColor = Color.White;
            btParlar.Location = new Point(45, 196);
            btParlar.Name = "btParlar";
            btParlar.Size = new Size(108, 29);
            btParlar.TabIndex = 6;
            btParlar.Text = "parlar";
            btParlar.UseVisualStyleBackColor = false;
            btParlar.Click += btParlar_Click;
            // 
            // nupPortParlar
            // 
            nupPortParlar.Location = new Point(124, 86);
            nupPortParlar.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nupPortParlar.Minimum = new decimal(new int[] { 1000, 0, 0, 0 });
            nupPortParlar.Name = "nupPortParlar";
            nupPortParlar.Size = new Size(64, 27);
            nupPortParlar.TabIndex = 7;
            nupPortParlar.TextAlign = HorizontalAlignment.Right;
            nupPortParlar.Value = new decimal(new int[] { 8000, 0, 0, 0 });
            // 
            // tbRebre
            // 
            tbRebre.Location = new Point(223, 189);
            tbRebre.Multiline = true;
            tbRebre.Name = "tbRebre";
            tbRebre.ReadOnly = true;
            tbRebre.ScrollBars = ScrollBars.Both;
            tbRebre.Size = new Size(750, 320);
            tbRebre.TabIndex = 18;
            // 
            // gbEnviar
            // 
            gbEnviar.Controls.Add(btEnviar);
            gbEnviar.Controls.Add(tbEnviar);
            gbEnviar.Location = new Point(223, 20);
            gbEnviar.Name = "gbEnviar";
            gbEnviar.Size = new Size(750, 163);
            gbEnviar.TabIndex = 17;
            gbEnviar.TabStop = false;
            gbEnviar.Text = " ** missatge a enviar ** ";
            // 
            // btEnviar
            // 
            btEnviar.BackColor = Color.ForestGreen;
            btEnviar.Enabled = false;
            btEnviar.FlatStyle = FlatStyle.Flat;
            btEnviar.ForeColor = Color.White;
            btEnviar.Location = new Point(598, 119);
            btEnviar.Name = "btEnviar";
            btEnviar.Size = new Size(136, 29);
            btEnviar.TabIndex = 12;
            btEnviar.Text = "enviar";
            btEnviar.UseVisualStyleBackColor = false;
            btEnviar.Click += btEnviar_Click;
            // 
            // tbEnviar
            // 
            tbEnviar.Enabled = false;
            tbEnviar.Location = new Point(26, 22);
            tbEnviar.Multiline = true;
            tbEnviar.Name = "tbEnviar";
            tbEnviar.Size = new Size(708, 84);
            tbEnviar.TabIndex = 10;
            // 
            // lbPersonas
            // 
            lbPersonas.FormattingEnabled = true;
            lbPersonas.Location = new Point(14, 325);
            lbPersonas.Name = "lbPersonas";
            lbPersonas.Size = new Size(183, 184);
            lbPersonas.TabIndex = 23;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Tan;
            ClientSize = new Size(1037, 772);
            Controls.Add(lbPersonas);
            Controls.Add(lbLog);
            Controls.Add(gbRemot);
            Controls.Add(tbRebre);
            Controls.Add(gbEnviar);
            Name = "FrmMain";
            Text = "Form1";
            Load += FrmMain_Load;
            gbRemot.ResumeLayout(false);
            gbRemot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nupPortParlar).EndInit();
            gbEnviar.ResumeLayout(false);
            gbEnviar.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btStop;
        private ListBox lbLog;
        private GroupBox gbRemot;
        private TextBox tbIpremota;
        private Label lbIP;
        private Label lbPortParlar;
        private Button btParlar;
        private NumericUpDown nupPortParlar;
        private TextBox tbRebre;
        private GroupBox gbEnviar;
        private Button btEnviar;
        private TextBox tbEnviar;
        private ListBox lbPersonas;
        private TextBox tbNom;
        private Label lbNom;
    }
}
