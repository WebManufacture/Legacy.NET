namespace Hlab.CncTable.Win.UI
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
            this.cbPort = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.log = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.lblCommand = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblZ = new System.Windows.Forms.Label();
            this.lblY = new System.Windows.Forms.Label();
            this.lblX = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblProgram = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.stateTmr = new System.Windows.Forms.Timer(this.components);
            this.devStateTmr = new System.Windows.Forms.Timer(this.components);
            this.btnControl = new System.Windows.Forms.Button();
            this.resetXbtn = new System.Windows.Forms.Button();
            this.resetYbtn = new System.Windows.Forms.Button();
            this.resetZbtn = new System.Windows.Forms.Button();
            this.lblB = new System.Windows.Forms.Label();
            this.lblA = new System.Windows.Forms.Label();
            this.lnkState = new System.Windows.Forms.Label();
            this.lblHttpConnection = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbPort
            // 
            this.cbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPort.FormattingEnabled = true;
            this.cbPort.Location = new System.Drawing.Point(60, 6);
            this.cbPort.Name = "cbPort";
            this.cbPort.Size = new System.Drawing.Size(183, 21);
            this.cbPort.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(135, 34);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 24);
            this.button1.TabIndex = 1;
            this.button1.Text = "Refresh";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // log
            // 
            this.log.Location = new System.Drawing.Point(390, 2);
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(310, 632);
            this.log.TabIndex = 5;
            this.log.Text = "";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(299, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "clear";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblCommand
            // 
            this.lblCommand.AutoSize = true;
            this.lblCommand.Font = new System.Drawing.Font("Verdana", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCommand.ForeColor = System.Drawing.Color.DarkGray;
            this.lblCommand.Location = new System.Drawing.Point(73, 160);
            this.lblCommand.Name = "lblCommand";
            this.lblCommand.Size = new System.Drawing.Size(0, 35);
            this.lblCommand.TabIndex = 11;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnControl);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbPort);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.lnkState);
            this.panel1.Location = new System.Drawing.Point(3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(381, 74);
            this.panel1.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.DarkGray;
            this.label1.Location = new System.Drawing.Point(7, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 18);
            this.label1.TabIndex = 13;
            this.label1.Text = "FTDI";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblHttpConnection);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(3, 82);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(381, 66);
            this.panel2.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.DarkGray;
            this.label2.Location = new System.Drawing.Point(8, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 18);
            this.label2.TabIndex = 14;
            this.label2.Text = "HTTP";
            // 
            // lblZ
            // 
            this.lblZ.AutoSize = true;
            this.lblZ.Font = new System.Drawing.Font("Verdana", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblZ.Location = new System.Drawing.Point(73, 265);
            this.lblZ.Name = "lblZ";
            this.lblZ.Size = new System.Drawing.Size(103, 35);
            this.lblZ.TabIndex = 15;
            this.lblZ.Text = "label1";
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Font = new System.Drawing.Font("Verdana", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblY.Location = new System.Drawing.Point(73, 230);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(103, 35);
            this.lblY.TabIndex = 16;
            this.lblY.Text = "label1";
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Font = new System.Drawing.Font("Verdana", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblX.Location = new System.Drawing.Point(73, 195);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(103, 35);
            this.lblX.TabIndex = 17;
            this.lblX.Text = "label1";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.lblA);
            this.panel3.Controls.Add(this.lblB);
            this.panel3.Controls.Add(this.resetZbtn);
            this.panel3.Controls.Add(this.resetYbtn);
            this.panel3.Controls.Add(this.resetXbtn);
            this.panel3.Controls.Add(this.lblProgram);
            this.panel3.Controls.Add(this.button5);
            this.panel3.Controls.Add(this.button4);
            this.panel3.Controls.Add(this.button3);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.lblCommand);
            this.panel3.Controls.Add(this.lblZ);
            this.panel3.Controls.Add(this.lblY);
            this.panel3.Controls.Add(this.lblX);
            this.panel3.Location = new System.Drawing.Point(3, 303);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(381, 331);
            this.panel3.TabIndex = 18;
            // 
            // lblProgram
            // 
            this.lblProgram.AutoSize = true;
            this.lblProgram.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblProgram.Location = new System.Drawing.Point(64, 7);
            this.lblProgram.Name = "lblProgram";
            this.lblProgram.Size = new System.Drawing.Size(121, 19);
            this.lblProgram.TabIndex = 15;
            this.lblProgram.Text = "Not connected";
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button5.ForeColor = System.Drawing.Color.Black;
            this.button5.Location = new System.Drawing.Point(8, 98);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(99, 56);
            this.button5.TabIndex = 22;
            this.button5.Text = "Resume";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button4.ForeColor = System.Drawing.Color.Black;
            this.button4.Location = new System.Drawing.Point(8, 32);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(99, 60);
            this.button4.TabIndex = 14;
            this.button4.Text = "Pause";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button3.ForeColor = System.Drawing.Color.Red;
            this.button3.Location = new System.Drawing.Point(113, 32);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(130, 122);
            this.button3.TabIndex = 14;
            this.button3.Text = "STOP";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.ForeColor = System.Drawing.Color.DarkGray;
            this.label7.Location = new System.Drawing.Point(8, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 18);
            this.label7.TabIndex = 15;
            this.label7.Text = "CNC";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.ForeColor = System.Drawing.Color.Green;
            this.label6.Location = new System.Drawing.Point(46, 265);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 35);
            this.label6.TabIndex = 21;
            this.label6.Text = "Z";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.ForeColor = System.Drawing.Color.DarkViolet;
            this.label5.Location = new System.Drawing.Point(46, 230);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 35);
            this.label5.TabIndex = 20;
            this.label5.Text = "Y";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(46, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 35);
            this.label4.TabIndex = 19;
            this.label4.Text = "X";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.DarkGray;
            this.label3.Location = new System.Drawing.Point(46, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 35);
            this.label3.TabIndex = 18;
            this.label3.Text = "C";
            // 
            // stateTmr
            // 
            this.stateTmr.Enabled = true;
            this.stateTmr.Interval = 1000;
            this.stateTmr.Tick += new System.EventHandler(this.stateTmr_Tick);
            // 
            // devStateTmr
            // 
            this.devStateTmr.Enabled = true;
            this.devStateTmr.Interval = 1000;
            this.devStateTmr.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnControl
            // 
            this.btnControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnControl.ForeColor = System.Drawing.Color.Black;
            this.btnControl.Location = new System.Drawing.Point(299, 33);
            this.btnControl.Name = "btnControl";
            this.btnControl.Size = new System.Drawing.Size(75, 25);
            this.btnControl.TabIndex = 23;
            this.btnControl.Text = "Controls";
            this.btnControl.UseVisualStyleBackColor = true;
            this.btnControl.Click += new System.EventHandler(this.btnControl_Click);
            // 
            // resetXbtn
            // 
            this.resetXbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.resetXbtn.Location = new System.Drawing.Point(9, 195);
            this.resetXbtn.Name = "resetXbtn";
            this.resetXbtn.Size = new System.Drawing.Size(38, 35);
            this.resetXbtn.TabIndex = 23;
            this.resetXbtn.TabStop = false;
            this.resetXbtn.Text = ">0";
            this.resetXbtn.UseVisualStyleBackColor = true;
            this.resetXbtn.Click += new System.EventHandler(this.resetXbtn_Click);
            // 
            // resetYbtn
            // 
            this.resetYbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.resetYbtn.Location = new System.Drawing.Point(9, 230);
            this.resetYbtn.Name = "resetYbtn";
            this.resetYbtn.Size = new System.Drawing.Size(38, 35);
            this.resetYbtn.TabIndex = 24;
            this.resetYbtn.TabStop = false;
            this.resetYbtn.Text = ">0";
            this.resetYbtn.UseVisualStyleBackColor = true;
            this.resetYbtn.Click += new System.EventHandler(this.resetYbtn_Click);
            // 
            // resetZbtn
            // 
            this.resetZbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.resetZbtn.Location = new System.Drawing.Point(9, 265);
            this.resetZbtn.Name = "resetZbtn";
            this.resetZbtn.Size = new System.Drawing.Size(38, 35);
            this.resetZbtn.TabIndex = 25;
            this.resetZbtn.TabStop = false;
            this.resetZbtn.Text = ">0";
            this.resetZbtn.UseVisualStyleBackColor = true;
            this.resetZbtn.Click += new System.EventHandler(this.resetZbtn_Click);
            // 
            // lblB
            // 
            this.lblB.AutoSize = true;
            this.lblB.Font = new System.Drawing.Font("Verdana", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblB.Location = new System.Drawing.Point(249, 67);
            this.lblB.Name = "lblB";
            this.lblB.Size = new System.Drawing.Size(0, 35);
            this.lblB.TabIndex = 26;
            // 
            // lblA
            // 
            this.lblA.AutoSize = true;
            this.lblA.Font = new System.Drawing.Font("Verdana", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblA.Location = new System.Drawing.Point(249, 32);
            this.lblA.Name = "lblA";
            this.lblA.Size = new System.Drawing.Size(0, 35);
            this.lblA.TabIndex = 27;
            // 
            // lnkState
            // 
            this.lnkState.AutoSize = true;
            this.lnkState.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lnkState.Location = new System.Drawing.Point(8, 39);
            this.lnkState.Name = "lnkState";
            this.lnkState.Size = new System.Drawing.Size(121, 19);
            this.lnkState.TabIndex = 4;
            this.lnkState.Text = "Not connected";
            this.lnkState.Click += new System.EventHandler(this.lnkState_Click);
            // 
            // lblHttpConnection
            // 
            this.lblHttpConnection.AutoSize = true;
            this.lblHttpConnection.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblHttpConnection.Location = new System.Drawing.Point(8, 38);
            this.lblHttpConnection.Name = "lblHttpConnection";
            this.lblHttpConnection.Size = new System.Drawing.Size(121, 19);
            this.lblHttpConnection.TabIndex = 14;
            this.lblHttpConnection.Text = "Not connected";
            this.lblHttpConnection.Click += new System.EventHandler(this.lblHttpConnection_Click);
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 638);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.log);
            this.Name = "fmMain";
            this.Text = "cncTable";
            this.Load += new System.EventHandler(this.fmMain_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fmMain_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbPort;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox log;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lblCommand;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblZ;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Timer stateTmr;
        private System.Windows.Forms.Label lblProgram;
        private System.Windows.Forms.Timer devStateTmr;
        private System.Windows.Forms.Button btnControl;
        private System.Windows.Forms.Button resetZbtn;
        private System.Windows.Forms.Button resetYbtn;
        private System.Windows.Forms.Button resetXbtn;
        private System.Windows.Forms.Label lblA;
        private System.Windows.Forms.Label lblB;
        private System.Windows.Forms.Label lnkState;
        private System.Windows.Forms.Label lblHttpConnection;
    }
}

