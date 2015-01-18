namespace MRS.Hardware.UI.Analyzer
{
    partial class fmUART
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
            this.txtSend = new System.Windows.Forms.TextBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.cbBaud = new System.Windows.Forms.ComboBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.cbBits = new System.Windows.Forms.ComboBox();
            this.cbStop = new System.Windows.Forms.ComboBox();
            this.cbParity = new System.Windows.Forms.ComboBox();
            this.txtByte = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.receiveTimer = new System.Windows.Forms.Timer(this.components);
            this.txtReceive = new System.Windows.Forms.RichTextBox();
            this.btnWrite = new System.Windows.Forms.Button();
            this.boxCMD = new System.Windows.Forms.TextBox();
            this.btnExec = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtSend
            // 
            this.txtSend.Location = new System.Drawing.Point(12, 31);
            this.txtSend.Multiline = true;
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(195, 366);
            this.txtSend.TabIndex = 1;
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(213, 2);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(75, 23);
            this.btnRead.TabIndex = 3;
            this.btnRead.Text = "Receive";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // cbBaud
            // 
            this.cbBaud.AllowDrop = true;
            this.cbBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBaud.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbBaud.FormattingEnabled = true;
            this.cbBaud.Items.AddRange(new object[] {
            "600",
            "1200",
            "9600",
            "14400",
            "56000",
            "115200",
            "512000",
            "1024000"});
            this.cbBaud.Location = new System.Drawing.Point(411, 31);
            this.cbBaud.MaxDropDownItems = 20;
            this.cbBaud.Name = "cbBaud";
            this.cbBaud.Size = new System.Drawing.Size(186, 21);
            this.cbBaud.TabIndex = 4;
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(522, 139);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(75, 23);
            this.btnSet.TabIndex = 5;
            this.btnSet.Text = "Set";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // cbBits
            // 
            this.cbBits.AllowDrop = true;
            this.cbBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBits.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbBits.FormattingEnabled = true;
            this.cbBits.Items.AddRange(new object[] {
            "8",
            "9"});
            this.cbBits.Location = new System.Drawing.Point(411, 58);
            this.cbBits.MaxDropDownItems = 20;
            this.cbBits.Name = "cbBits";
            this.cbBits.Size = new System.Drawing.Size(186, 21);
            this.cbBits.TabIndex = 6;
            // 
            // cbStop
            // 
            this.cbStop.AllowDrop = true;
            this.cbStop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbStop.FormattingEnabled = true;
            this.cbStop.Items.AddRange(new object[] {
            "7",
            "8"});
            this.cbStop.Location = new System.Drawing.Point(411, 85);
            this.cbStop.MaxDropDownItems = 20;
            this.cbStop.Name = "cbStop";
            this.cbStop.Size = new System.Drawing.Size(186, 21);
            this.cbStop.TabIndex = 7;
            // 
            // cbParity
            // 
            this.cbParity.AllowDrop = true;
            this.cbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbParity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbParity.FormattingEnabled = true;
            this.cbParity.Items.AddRange(new object[] {
            "0",
            "1"});
            this.cbParity.Location = new System.Drawing.Point(411, 112);
            this.cbParity.MaxDropDownItems = 20;
            this.cbParity.Name = "cbParity";
            this.cbParity.Size = new System.Drawing.Size(186, 21);
            this.cbParity.TabIndex = 8;
            // 
            // txtByte
            // 
            this.txtByte.Location = new System.Drawing.Point(12, 4);
            this.txtByte.Name = "txtByte";
            this.txtByte.Size = new System.Drawing.Size(100, 20);
            this.txtByte.TabIndex = 9;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(323, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "ReadCycle";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // receiveTimer
            // 
            this.receiveTimer.Tick += new System.EventHandler(this.receiveTimer_Tick);
            // 
            // txtReceive
            // 
            this.txtReceive.Location = new System.Drawing.Point(213, 31);
            this.txtReceive.Name = "txtReceive";
            this.txtReceive.Size = new System.Drawing.Size(192, 366);
            this.txtReceive.TabIndex = 11;
            this.txtReceive.Text = "";
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(132, 2);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(75, 23);
            this.btnWrite.TabIndex = 0;
            this.btnWrite.Text = "Send";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // boxCMD
            // 
            this.boxCMD.Location = new System.Drawing.Point(411, 309);
            this.boxCMD.Multiline = true;
            this.boxCMD.Name = "boxCMD";
            this.boxCMD.Size = new System.Drawing.Size(186, 59);
            this.boxCMD.TabIndex = 13;
            // 
            // btnExec
            // 
            this.btnExec.Location = new System.Drawing.Point(411, 374);
            this.btnExec.Name = "btnExec";
            this.btnExec.Size = new System.Drawing.Size(186, 23);
            this.btnExec.TabIndex = 12;
            this.btnExec.Text = "Exec";
            this.btnExec.UseVisualStyleBackColor = true;
            this.btnExec.Click += new System.EventHandler(this.btnExec_Click);
            // 
            // fmUART
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 402);
            this.Controls.Add(this.boxCMD);
            this.Controls.Add(this.btnExec);
            this.Controls.Add(this.txtReceive);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtByte);
            this.Controls.Add(this.cbParity);
            this.Controls.Add(this.cbStop);
            this.Controls.Add(this.cbBits);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.cbBaud);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.txtSend);
            this.Controls.Add(this.btnWrite);
            this.Name = "fmUART";
            this.Text = "fmUART";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.ComboBox cbBaud;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.ComboBox cbBits;
        private System.Windows.Forms.ComboBox cbStop;
        private System.Windows.Forms.ComboBox cbParity;
        private System.Windows.Forms.TextBox txtByte;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer receiveTimer;
        private System.Windows.Forms.RichTextBox txtReceive;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.TextBox boxCMD;
        private System.Windows.Forms.Button btnExec;
    }
}