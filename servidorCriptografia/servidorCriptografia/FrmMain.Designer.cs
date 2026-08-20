namespace servidorCriptografia
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
            llPeticions = new ListBox();
            lbPeticions = new Label();
            btStop = new Button();
            btStart = new Button();
            lbPort = new Label();
            nupPort = new NumericUpDown();
            lbEstat = new Label();
            ((System.ComponentModel.ISupportInitialize)nupPort).BeginInit();
            SuspendLayout();
            // 
            // llPeticions
            // 
            llPeticions.FormattingEnabled = true;
            llPeticions.HorizontalScrollbar = true;
            llPeticions.Location = new Point(12, 99);
            llPeticions.Name = "llPeticions";
            llPeticions.ScrollAlwaysVisible = true;
            llPeticions.Size = new Size(525, 344);
            llPeticions.TabIndex = 7;
            // 
            // lbPeticions
            // 
            lbPeticions.BackColor = Color.Peru;
            lbPeticions.ForeColor = Color.Black;
            lbPeticions.Location = new Point(12, 68);
            lbPeticions.Name = "lbPeticions";
            lbPeticions.Padding = new Padding(3);
            lbPeticions.Size = new Size(529, 28);
            lbPeticions.TabIndex = 8;
            lbPeticions.Text = "peticions";
            lbPeticions.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btStop
            // 
            btStop.BackColor = Color.Red;
            btStop.FlatStyle = FlatStyle.Flat;
            btStop.ForeColor = Color.White;
            btStop.Location = new Point(346, 12);
            btStop.Name = "btStop";
            btStop.Size = new Size(191, 41);
            btStop.TabIndex = 12;
            btStop.Text = "Stop";
            btStop.UseVisualStyleBackColor = false;
            btStop.Visible = false;
            btStop.Click += btStop_Click;
            // 
            // btStart
            // 
            btStart.BackColor = Color.LawnGreen;
            btStart.FlatStyle = FlatStyle.Flat;
            btStart.Location = new Point(346, 12);
            btStart.Name = "btStart";
            btStart.Size = new Size(191, 41);
            btStart.TabIndex = 11;
            btStart.Text = "Start";
            btStart.UseVisualStyleBackColor = false;
            btStart.Click += btStart_Click;
            // 
            // lbPort
            // 
            lbPort.BackColor = Color.Peru;
            lbPort.ForeColor = Color.Black;
            lbPort.Location = new Point(12, 18);
            lbPort.Name = "lbPort";
            lbPort.Padding = new Padding(3);
            lbPort.Size = new Size(191, 28);
            lbPort.TabIndex = 10;
            lbPort.Text = "nº de port servidor";
            lbPort.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // nupPort
            // 
            nupPort.Location = new Point(214, 20);
            nupPort.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nupPort.Minimum = new decimal(new int[] { 1000, 0, 0, 0 });
            nupPort.Name = "nupPort";
            nupPort.Size = new Size(73, 27);
            nupPort.TabIndex = 9;
            nupPort.TextAlign = HorizontalAlignment.Right;
            nupPort.Value = new decimal(new int[] { 8000, 0, 0, 0 });
            // 
            // lbEstat
            // 
            lbEstat.AutoSize = true;
            lbEstat.Location = new Point(246, 462);
            lbEstat.Name = "lbEstat";
            lbEstat.Size = new Size(41, 20);
            lbEstat.TabIndex = 13;
            lbEstat.Text = "estat";
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(566, 517);
            Controls.Add(lbEstat);
            Controls.Add(btStop);
            Controls.Add(btStart);
            Controls.Add(lbPort);
            Controls.Add(nupPort);
            Controls.Add(lbPeticions);
            Controls.Add(llPeticions);
            Name = "FrmMain";
            Text = "Form1";
            Load += FrmMain_Load;
            ((System.ComponentModel.ISupportInitialize)nupPort).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox llPeticions;
        private Label lbPeticions;
        private Button btStop;
        private Button btStart;
        private Label lbPort;
        private NumericUpDown nupPort;
        private Label lbEstat;
    }
}
