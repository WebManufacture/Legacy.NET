namespace MRS.Hardware.UI.Analyzer
{
    partial class fmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fmMain));
            this.txtConsole = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.boxConnect = new System.Windows.Forms.ToolStripDropDownButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.uARTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bitBangToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analizerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aDCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rescanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hTTPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uARTToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cbCOMs = new System.Windows.Forms.ToolStripComboBox();
            this.cbUARTSpeeds = new System.Windows.Forms.ToolStripComboBox();
            this.menuUARTconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cbStoredDevices = new System.Windows.Forms.CheckedListBox();
            this.btnAnalyzer = new System.Windows.Forms.Button();
            this.btnADC = new System.Windows.Forms.Button();
            this.btnUART = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtConsole
            // 
            this.txtConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConsole.BackColor = System.Drawing.Color.Black;
            this.txtConsole.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtConsole.ForeColor = System.Drawing.Color.Lime;
            this.txtConsole.Location = new System.Drawing.Point(75, 27);
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.Size = new System.Drawing.Size(462, 363);
            this.txtConsole.TabIndex = 2;
            this.txtConsole.Text = "";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.boxConnect});
            this.statusStrip1.Location = new System.Drawing.Point(0, 393);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.Size = new System.Drawing.Size(748, 22);
            this.statusStrip1.TabIndex = 21;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // boxConnect
            // 
            this.boxConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.boxConnect.Image = ((System.Drawing.Image)(resources.GetObject("boxConnect.Image")));
            this.boxConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.boxConnect.Name = "boxConnect";
            this.boxConnect.Size = new System.Drawing.Size(29, 20);
            this.boxConnect.Click += new System.EventHandler(this.boxConnect_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolsMenu,
            this.aToolStripMenuItem,
            this.hTTPToolStripMenuItem,
            this.uARTToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(748, 24);
            this.menuStrip1.TabIndex = 22;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ToolsMenu
            // 
            this.ToolsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uARTToolStripMenuItem,
            this.bitBangToolStripMenuItem,
            this.statsToolStripMenuItem,
            this.analizerToolStripMenuItem,
            this.aDCToolStripMenuItem});
            this.ToolsMenu.Enabled = false;
            this.ToolsMenu.Name = "ToolsMenu";
            this.ToolsMenu.Size = new System.Drawing.Size(48, 20);
            this.ToolsMenu.Text = "Tools";
            // 
            // uARTToolStripMenuItem
            // 
            this.uARTToolStripMenuItem.Name = "uARTToolStripMenuItem";
            this.uARTToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.uARTToolStripMenuItem.Text = "UART";
            this.uARTToolStripMenuItem.Click += new System.EventHandler(this.UART_Click);
            // 
            // bitBangToolStripMenuItem
            // 
            this.bitBangToolStripMenuItem.Name = "bitBangToolStripMenuItem";
            this.bitBangToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.bitBangToolStripMenuItem.Text = "BitBang";
            this.bitBangToolStripMenuItem.Click += new System.EventHandler(this.BitBang_Click);
            // 
            // statsToolStripMenuItem
            // 
            this.statsToolStripMenuItem.Name = "statsToolStripMenuItem";
            this.statsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.statsToolStripMenuItem.Text = "Stats";
            this.statsToolStripMenuItem.Click += new System.EventHandler(this.btnStats_Click);
            // 
            // analizerToolStripMenuItem
            // 
            this.analizerToolStripMenuItem.Name = "analizerToolStripMenuItem";
            this.analizerToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.analizerToolStripMenuItem.Text = "Analizer";
            this.analizerToolStripMenuItem.Click += new System.EventHandler(this.Analizer_Click);
            // 
            // aDCToolStripMenuItem
            // 
            this.aDCToolStripMenuItem.Name = "aDCToolStripMenuItem";
            this.aDCToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aDCToolStripMenuItem.Text = "ADC";
            this.aDCToolStripMenuItem.Click += new System.EventHandler(this.ADC_Click);
            // 
            // aToolStripMenuItem
            // 
            this.aToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rescanToolStripMenuItem,
            this.resetToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.aToolStripMenuItem.Name = "aToolStripMenuItem";
            this.aToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.aToolStripMenuItem.Text = "Actions";
            // 
            // rescanToolStripMenuItem
            // 
            this.rescanToolStripMenuItem.Name = "rescanToolStripMenuItem";
            this.rescanToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.rescanToolStripMenuItem.Text = "Rescan";
            this.rescanToolStripMenuItem.Click += new System.EventHandler(this.Rescan_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.Reset_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.Close_Click);
            // 
            // hTTPToolStripMenuItem
            // 
            this.hTTPToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem});
            this.hTTPToolStripMenuItem.Name = "hTTPToolStripMenuItem";
            this.hTTPToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.hTTPToolStripMenuItem.Text = "HTTP";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.HttpStart_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.HttpStop_Click);
            // 
            // uARTToolStripMenuItem1
            // 
            this.uARTToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cbCOMs,
            this.cbUARTSpeeds,
            this.menuUARTconnect,
            this.disconnectToolStripMenuItem});
            this.uARTToolStripMenuItem1.Name = "uARTToolStripMenuItem1";
            this.uARTToolStripMenuItem1.Size = new System.Drawing.Size(49, 20);
            this.uARTToolStripMenuItem1.Text = "UART";
            // 
            // cbCOMs
            // 
            this.cbCOMs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCOMs.Name = "cbCOMs";
            this.cbCOMs.Size = new System.Drawing.Size(121, 23);
            this.cbCOMs.Click += new System.EventHandler(this.menuUARTconnect_Click);
            // 
            // cbUARTSpeeds
            // 
            this.cbUARTSpeeds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUARTSpeeds.Items.AddRange(new object[] {
            "300",
            "600",
            "1200",
            "1800",
            "2400",
            "4000",
            "4800",
            "7200",
            "9600",
            "14400",
            "16000",
            "19200",
            "28800",
            "38400",
            "51200",
            "56000",
            "57600",
            "64000",
            "76800",
            "115200",
            "128000",
            "153600",
            "230400",
            "250000",
            "256000",
            "460800",
            "500000",
            "576000",
            "921600"});
            this.cbUARTSpeeds.Name = "cbUARTSpeeds";
            this.cbUARTSpeeds.Size = new System.Drawing.Size(121, 23);
            // 
            // menuUARTconnect
            // 
            this.menuUARTconnect.Name = "menuUARTconnect";
            this.menuUARTconnect.Size = new System.Drawing.Size(181, 22);
            this.menuUARTconnect.Text = "Connect...";
            this.menuUARTconnect.Click += new System.EventHandler(this.menuUARTconnect_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
            // 
            // cbStoredDevices
            // 
            this.cbStoredDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbStoredDevices.FormattingEnabled = true;
            this.cbStoredDevices.Location = new System.Drawing.Point(536, 27);
            this.cbStoredDevices.Name = "cbStoredDevices";
            this.cbStoredDevices.Size = new System.Drawing.Size(212, 364);
            this.cbStoredDevices.TabIndex = 23;
            this.cbStoredDevices.SelectedIndexChanged += new System.EventHandler(this.cbStoredDevices_SelectedIndexChanged);
            // 
            // btnAnalyzer
            // 
            this.btnAnalyzer.Enabled = false;
            this.btnAnalyzer.Location = new System.Drawing.Point(0, 28);
            this.btnAnalyzer.Name = "btnAnalyzer";
            this.btnAnalyzer.Size = new System.Drawing.Size(75, 23);
            this.btnAnalyzer.TabIndex = 24;
            this.btnAnalyzer.Text = "Analyzer";
            this.btnAnalyzer.UseVisualStyleBackColor = true;
            this.btnAnalyzer.Click += new System.EventHandler(this.Analizer_Click);
            // 
            // btnADC
            // 
            this.btnADC.Enabled = false;
            this.btnADC.Location = new System.Drawing.Point(0, 57);
            this.btnADC.Name = "btnADC";
            this.btnADC.Size = new System.Drawing.Size(75, 23);
            this.btnADC.TabIndex = 25;
            this.btnADC.Text = "ADC";
            this.btnADC.UseVisualStyleBackColor = true;
            this.btnADC.Click += new System.EventHandler(this.ADC_Click);
            // 
            // btnUART
            // 
            this.btnUART.Enabled = false;
            this.btnUART.Location = new System.Drawing.Point(0, 86);
            this.btnUART.Name = "btnUART";
            this.btnUART.Size = new System.Drawing.Size(75, 23);
            this.btnUART.TabIndex = 26;
            this.btnUART.Text = "UART";
            this.btnUART.UseVisualStyleBackColor = true;
            this.btnUART.Click += new System.EventHandler(this.btnUART_Click);
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 415);
            this.Controls.Add(this.btnUART);
            this.Controls.Add(this.btnADC);
            this.Controls.Add(this.btnAnalyzer);
            this.Controls.Add(this.cbStoredDevices);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.txtConsole);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "fmMain";
            this.Text = "FTDI Control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fmMain_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtConsole;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolsMenu;
        private System.Windows.Forms.ToolStripMenuItem uARTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bitBangToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem analizerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aDCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rescanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hTTPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton boxConnect;
        private System.Windows.Forms.ToolStripMenuItem uARTToolStripMenuItem1;
        private System.Windows.Forms.ToolStripComboBox cbCOMs;
        private System.Windows.Forms.ToolStripComboBox cbUARTSpeeds;
        private System.Windows.Forms.ToolStripMenuItem menuUARTconnect;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.CheckedListBox cbStoredDevices;
        private System.Windows.Forms.Button btnAnalyzer;
        private System.Windows.Forms.Button btnADC;
        private System.Windows.Forms.Button btnUART;
    }
}

