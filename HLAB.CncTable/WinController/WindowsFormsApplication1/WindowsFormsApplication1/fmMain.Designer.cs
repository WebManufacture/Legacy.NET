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
            this.lblState = new System.Windows.Forms.Label();
            this.cbPort = new System.Windows.Forms.ComboBox();
            this.stateTimer = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.cbChannel = new System.Windows.Forms.NumericUpDown();
            this.cbFill = new System.Windows.Forms.NumericUpDown();
            this.btnMMode = new System.Windows.Forms.Button();
            this.tbProg = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbFill)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnMMode);
            this.panel1.Controls.Add(this.lblState);
            this.panel1.Controls.Add(this.cbPort);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(526, 56);
            this.panel1.TabIndex = 0;
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
            // panel2
            // 
            this.panel2.Controls.Add(this.cbFill);
            this.panel2.Controls.Add(this.cbChannel);
            this.panel2.Controls.Add(this.btnSend);
            this.panel2.Controls.Add(this.cbType);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 56);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(526, 70);
            this.panel2.TabIndex = 4;
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(99, 24);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(121, 21);
            this.cbType.TabIndex = 0;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(12, 22);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // cbChannel
            // 
            this.cbChannel.Location = new System.Drawing.Point(226, 25);
            this.cbChannel.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.cbChannel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cbChannel.Name = "cbChannel";
            this.cbChannel.Size = new System.Drawing.Size(41, 20);
            this.cbChannel.TabIndex = 3;
            this.cbChannel.Value = new decimal(new int[] {
            23,
            0,
            0,
            0});
            // 
            // cbFill
            // 
            this.cbFill.Location = new System.Drawing.Point(273, 25);
            this.cbFill.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.cbFill.Name = "cbFill";
            this.cbFill.Size = new System.Drawing.Size(120, 20);
            this.cbFill.TabIndex = 4;
            // 
            // btnMMode
            // 
            this.btnMMode.Location = new System.Drawing.Point(139, 29);
            this.btnMMode.Name = "btnMMode";
            this.btnMMode.Size = new System.Drawing.Size(123, 21);
            this.btnMMode.TabIndex = 5;
            this.btnMMode.Text = "Manual Mode";
            this.btnMMode.UseVisualStyleBackColor = true;
            this.btnMMode.Click += new System.EventHandler(this.btnMMode_Click);
            // 
            // tbProg
            // 
            this.tbProg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbProg.Location = new System.Drawing.Point(0, 126);
            this.tbProg.Multiline = true;
            this.tbProg.Name = "tbProg";
            this.tbProg.Size = new System.Drawing.Size(526, 375);
            this.tbProg.TabIndex = 5;
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 501);
            this.Controls.Add(this.tbProg);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fmMain";
            this.Text = "eBox Programmer";
            this.Load += new System.EventHandler(this.fmMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cbChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbFill)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbPort;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Timer stateTimer;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.NumericUpDown cbChannel;
        private System.Windows.Forms.NumericUpDown cbFill;
        private System.Windows.Forms.Button btnMMode;
        private System.Windows.Forms.TextBox tbProg;
    }
}

