namespace HLab.eBox
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlService = new System.Windows.Forms.Panel();
            this.btnSend = new System.Windows.Forms.Button();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.cbChannel = new System.Windows.Forms.NumericUpDown();
            this.cbFill = new System.Windows.Forms.NumericUpDown();
            this.lblState = new System.Windows.Forms.Label();
            this.cbPort = new System.Windows.Forms.ComboBox();
            this.stateTimer = new System.Windows.Forms.Timer(this.components);
            this.pnlControls = new System.Windows.Forms.Panel();
            this.button22 = new System.Windows.Forms.Button();
            this.button21 = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnResume = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.btnCompile = new System.Windows.Forms.Button();
            this.btnWrite = new System.Windows.Forms.Button();
            this.tbProg = new System.Windows.Forms.TextBox();
            this.tbReaded = new System.Windows.Forms.TextBox();
            this.cbStartMode = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblstartMode = new System.Windows.Forms.Label();
            this.cbEndMode = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.pnlService.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbFill)).BeginInit();
            this.pnlControls.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlService);
            this.panel1.Controls.Add(this.lblState);
            this.panel1.Controls.Add(this.cbPort);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(536, 56);
            this.panel1.TabIndex = 0;
            // 
            // pnlService
            // 
            this.pnlService.Controls.Add(this.btnSend);
            this.pnlService.Controls.Add(this.cbType);
            this.pnlService.Controls.Add(this.btnReset);
            this.pnlService.Controls.Add(this.cbChannel);
            this.pnlService.Controls.Add(this.cbFill);
            this.pnlService.Location = new System.Drawing.Point(147, 4);
            this.pnlService.Name = "pnlService";
            this.pnlService.Size = new System.Drawing.Size(387, 52);
            this.pnlService.TabIndex = 2;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(297, 3);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(78, 22);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(99, 4);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(74, 21);
            this.cbType.TabIndex = 0;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(3, 4);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(78, 42);
            this.btnReset.TabIndex = 8;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // cbChannel
            // 
            this.cbChannel.Location = new System.Drawing.Point(179, 5);
            this.cbChannel.Maximum = new decimal(new int[] {
            27,
            0,
            0,
            0});
            this.cbChannel.Name = "cbChannel";
            this.cbChannel.Size = new System.Drawing.Size(41, 20);
            this.cbChannel.TabIndex = 3;
            this.cbChannel.Value = new decimal(new int[] {
            22,
            0,
            0,
            0});
            // 
            // cbFill
            // 
            this.cbFill.Location = new System.Drawing.Point(226, 5);
            this.cbFill.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.cbFill.Name = "cbFill";
            this.cbFill.Size = new System.Drawing.Size(65, 20);
            this.cbFill.TabIndex = 4;
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblState.Location = new System.Drawing.Point(12, 6);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(55, 20);
            this.lblState.TabIndex = 1;
            this.lblState.Text = "Offline";
            // 
            // cbPort
            // 
            this.cbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPort.FormattingEnabled = true;
            this.cbPort.Location = new System.Drawing.Point(12, 29);
            this.cbPort.Name = "cbPort";
            this.cbPort.Size = new System.Drawing.Size(121, 21);
            this.cbPort.TabIndex = 0;
            this.cbPort.SelectedIndexChanged += new System.EventHandler(this.cbPort_SelectedIndexChanged);
            // 
            // stateTimer
            // 
            this.stateTimer.Interval = 500;
            this.stateTimer.Tick += new System.EventHandler(this.stateTimer_Tick);
            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this.button22);
            this.pnlControls.Controls.Add(this.button21);
            this.pnlControls.Controls.Add(this.button20);
            this.pnlControls.Controls.Add(this.button17);
            this.pnlControls.Controls.Add(this.button14);
            this.pnlControls.Controls.Add(this.button15);
            this.pnlControls.Controls.Add(this.button16);
            this.pnlControls.Controls.Add(this.button9);
            this.pnlControls.Controls.Add(this.button10);
            this.pnlControls.Controls.Add(this.button11);
            this.pnlControls.Controls.Add(this.button12);
            this.pnlControls.Controls.Add(this.button5);
            this.pnlControls.Controls.Add(this.button6);
            this.pnlControls.Controls.Add(this.button7);
            this.pnlControls.Controls.Add(this.button8);
            this.pnlControls.Controls.Add(this.button3);
            this.pnlControls.Controls.Add(this.button4);
            this.pnlControls.Controls.Add(this.button2);
            this.pnlControls.Controls.Add(this.button1);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControls.Location = new System.Drawing.Point(0, 56);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(536, 37);
            this.pnlControls.TabIndex = 4;
            // 
            // button22
            // 
            this.button22.BackColor = System.Drawing.Color.Silver;
            this.button22.Location = new System.Drawing.Point(3, 6);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(27, 23);
            this.button22.TabIndex = 26;
            this.button22.Text = "1";
            this.button22.UseVisualStyleBackColor = false;
            this.button22.Click += new System.EventHandler(this.button18_Click);
            // 
            // button21
            // 
            this.button21.BackColor = System.Drawing.Color.LightSkyBlue;
            this.button21.ForeColor = System.Drawing.Color.Black;
            this.button21.Location = new System.Drawing.Point(495, 6);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(27, 23);
            this.button21.TabIndex = 25;
            this.button21.Text = "B";
            this.button21.UseVisualStyleBackColor = false;
            this.button21.Click += new System.EventHandler(this.button21_Click);
            // 
            // button20
            // 
            this.button20.BackColor = System.Drawing.Color.PaleGreen;
            this.button20.ForeColor = System.Drawing.Color.Black;
            this.button20.Location = new System.Drawing.Point(468, 6);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(27, 23);
            this.button20.TabIndex = 24;
            this.button20.Text = "G";
            this.button20.UseVisualStyleBackColor = false;
            this.button20.Click += new System.EventHandler(this.button21_Click);
            // 
            // button17
            // 
            this.button17.BackColor = System.Drawing.Color.LightCoral;
            this.button17.ForeColor = System.Drawing.Color.Black;
            this.button17.Location = new System.Drawing.Point(441, 6);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(27, 23);
            this.button17.TabIndex = 23;
            this.button17.Text = "R";
            this.button17.UseVisualStyleBackColor = false;
            this.button17.Click += new System.EventHandler(this.button21_Click);
            // 
            // button14
            // 
            this.button14.BackColor = System.Drawing.Color.Silver;
            this.button14.Location = new System.Drawing.Point(408, 6);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(27, 23);
            this.button14.TabIndex = 19;
            this.button14.Text = "16";
            this.button14.UseVisualStyleBackColor = false;
            this.button14.Click += new System.EventHandler(this.button18_Click);
            // 
            // button15
            // 
            this.button15.BackColor = System.Drawing.Color.Silver;
            this.button15.Location = new System.Drawing.Point(381, 6);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(27, 23);
            this.button15.TabIndex = 18;
            this.button15.Text = "15";
            this.button15.UseVisualStyleBackColor = false;
            this.button15.Click += new System.EventHandler(this.button18_Click);
            // 
            // button16
            // 
            this.button16.BackColor = System.Drawing.Color.Silver;
            this.button16.Location = new System.Drawing.Point(354, 6);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(27, 23);
            this.button16.TabIndex = 17;
            this.button16.Text = "14";
            this.button16.UseVisualStyleBackColor = false;
            this.button16.Click += new System.EventHandler(this.button18_Click);
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.Silver;
            this.button9.Location = new System.Drawing.Point(327, 6);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(27, 23);
            this.button9.TabIndex = 16;
            this.button9.Text = "13";
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button18_Click);
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.Color.Silver;
            this.button10.Location = new System.Drawing.Point(300, 6);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(27, 23);
            this.button10.TabIndex = 15;
            this.button10.Text = "12";
            this.button10.UseVisualStyleBackColor = false;
            this.button10.Click += new System.EventHandler(this.button18_Click);
            // 
            // button11
            // 
            this.button11.BackColor = System.Drawing.Color.Silver;
            this.button11.Location = new System.Drawing.Point(273, 6);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(27, 23);
            this.button11.TabIndex = 14;
            this.button11.Text = "11";
            this.button11.UseVisualStyleBackColor = false;
            this.button11.Click += new System.EventHandler(this.button18_Click);
            // 
            // button12
            // 
            this.button12.BackColor = System.Drawing.Color.Silver;
            this.button12.Location = new System.Drawing.Point(246, 6);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(27, 23);
            this.button12.TabIndex = 13;
            this.button12.Text = "10";
            this.button12.UseVisualStyleBackColor = false;
            this.button12.Click += new System.EventHandler(this.button18_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Silver;
            this.button5.Location = new System.Drawing.Point(219, 6);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(27, 23);
            this.button5.TabIndex = 12;
            this.button5.Text = "9";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button18_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Silver;
            this.button6.Location = new System.Drawing.Point(192, 6);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(27, 23);
            this.button6.TabIndex = 11;
            this.button6.Text = "8";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button18_Click);
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Silver;
            this.button7.Location = new System.Drawing.Point(165, 6);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(27, 23);
            this.button7.TabIndex = 10;
            this.button7.Text = "7";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button18_Click);
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.Silver;
            this.button8.Location = new System.Drawing.Point(138, 6);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(27, 23);
            this.button8.TabIndex = 9;
            this.button8.Text = "6";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button18_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Silver;
            this.button3.Location = new System.Drawing.Point(111, 6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(27, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "5";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button18_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Silver;
            this.button4.Location = new System.Drawing.Point(84, 6);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(27, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "4";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button18_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Silver;
            this.button2.Location = new System.Drawing.Point(57, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(27, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "3";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button18_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Silver;
            this.button1.Location = new System.Drawing.Point(30, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(27, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "2";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button18_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnResume);
            this.panel2.Controls.Add(this.btnPause);
            this.panel2.Controls.Add(this.btnStart);
            this.panel2.Controls.Add(this.lblError);
            this.panel2.Controls.Add(this.btnCompile);
            this.panel2.Controls.Add(this.btnWrite);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 463);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(536, 38);
            this.panel2.TabIndex = 7;
            // 
            // btnResume
            // 
            this.btnResume.Location = new System.Drawing.Point(366, 9);
            this.btnResume.Name = "btnResume";
            this.btnResume.Size = new System.Drawing.Size(69, 21);
            this.btnResume.TabIndex = 10;
            this.btnResume.Text = "Resume";
            this.btnResume.UseVisualStyleBackColor = true;
            this.btnResume.Click += new System.EventHandler(this.btnResume_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(300, 9);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(60, 21);
            this.btnPause.TabIndex = 9;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(81, 6);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(57, 29);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(143, 6);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(130, 13);
            this.lblError.TabIndex = 6;
            this.lblError.Text = "Программу писать тут ^";
            // 
            // btnCompile
            // 
            this.btnCompile.Location = new System.Drawing.Point(457, 9);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(79, 21);
            this.btnCompile.TabIndex = 5;
            this.btnCompile.Text = "Compile";
            this.btnCompile.UseVisualStyleBackColor = true;
            this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(3, 6);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(72, 29);
            this.btnWrite.TabIndex = 1;
            this.btnWrite.Text = "Write";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // tbProg
            // 
            this.tbProg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbProg.Location = new System.Drawing.Point(0, 122);
            this.tbProg.Multiline = true;
            this.tbProg.Name = "tbProg";
            this.tbProg.Size = new System.Drawing.Size(412, 341);
            this.tbProg.TabIndex = 9;
            // 
            // tbReaded
            // 
            this.tbReaded.Dock = System.Windows.Forms.DockStyle.Right;
            this.tbReaded.Location = new System.Drawing.Point(408, 93);
            this.tbReaded.Multiline = true;
            this.tbReaded.Name = "tbReaded";
            this.tbReaded.ReadOnly = true;
            this.tbReaded.Size = new System.Drawing.Size(128, 370);
            this.tbReaded.TabIndex = 10;
            // 
            // cbStartMode
            // 
            this.cbStartMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStartMode.FormattingEnabled = true;
            this.cbStartMode.Items.AddRange(new object[] {
            "idle",
            "output",
            "processing",
            "reset"});
            this.cbStartMode.Location = new System.Drawing.Point(68, 2);
            this.cbStartMode.Name = "cbStartMode";
            this.cbStartMode.Size = new System.Drawing.Size(121, 21);
            this.cbStartMode.TabIndex = 11;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.lblstartMode);
            this.panel3.Controls.Add(this.cbEndMode);
            this.panel3.Controls.Add(this.cbStartMode);
            this.panel3.Location = new System.Drawing.Point(0, 93);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(412, 26);
            this.panel3.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(223, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "End Mode";
            // 
            // lblstartMode
            // 
            this.lblstartMode.AutoSize = true;
            this.lblstartMode.Location = new System.Drawing.Point(3, 5);
            this.lblstartMode.Name = "lblstartMode";
            this.lblstartMode.Size = new System.Drawing.Size(59, 13);
            this.lblstartMode.TabIndex = 13;
            this.lblstartMode.Text = "Start Mode";
            // 
            // cbEndMode
            // 
            this.cbEndMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEndMode.FormattingEnabled = true;
            this.cbEndMode.Items.AddRange(new object[] {
            "idle",
            "output",
            "processing",
            "reset"});
            this.cbEndMode.Location = new System.Drawing.Point(285, 2);
            this.cbEndMode.Name = "cbEndMode";
            this.cbEndMode.Size = new System.Drawing.Size(121, 21);
            this.cbEndMode.TabIndex = 12;
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 501);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.tbReaded);
            this.Controls.Add(this.tbProg);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.panel1);
            this.Name = "fmMain";
            this.Text = "eBox Programmer";
            this.Load += new System.EventHandler(this.fmMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlService.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cbChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbFill)).EndInit();
            this.pnlControls.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbPort;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Timer stateTimer;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.NumericUpDown cbChannel;
        private System.Windows.Forms.NumericUpDown cbFill;
        private System.Windows.Forms.Button button21;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCompile;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.TextBox tbProg;
        private System.Windows.Forms.TextBox tbReaded;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnResume;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Panel pnlService;
        private System.Windows.Forms.ComboBox cbStartMode;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblstartMode;
        private System.Windows.Forms.ComboBox cbEndMode;
        private System.Windows.Forms.Button button22;
    }
}

