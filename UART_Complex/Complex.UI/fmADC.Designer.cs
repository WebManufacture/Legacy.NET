namespace MRS.Hardware.UI.Analyzer
{
    partial class fmADC
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
            this.btnStart = new System.Windows.Forms.Button();
            this.tmReader = new System.Windows.Forms.Timer(this.components);
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.rb33V = new System.Windows.Forms.RadioButton();
            this.rb5V = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtTimer = new System.Windows.Forms.TextBox();
            this.grdVals = new System.Windows.Forms.DataGridView();
            this.aDCmeasureBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.colNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVoltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColAVG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdVals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aDCmeasureBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(306, 45);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tmReader
            // 
            this.tmReader.Interval = 10;
            this.tmReader.Tick += new System.EventHandler(this.tmReader_Tick);
            // 
            // pbImage
            // 
            this.pbImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbImage.Location = new System.Drawing.Point(1, 0);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(509, 439);
            this.pbImage.TabIndex = 2;
            this.pbImage.TabStop = false;
            this.pbImage.Paint += new System.Windows.Forms.PaintEventHandler(this.pbImage_Paint);
            // 
            // rb33V
            // 
            this.rb33V.AutoSize = true;
            this.rb33V.Checked = true;
            this.rb33V.Location = new System.Drawing.Point(12, 3);
            this.rb33V.Name = "rb33V";
            this.rb33V.Size = new System.Drawing.Size(50, 17);
            this.rb33V.TabIndex = 4;
            this.rb33V.TabStop = true;
            this.rb33V.Text = "3.3 V";
            this.rb33V.UseVisualStyleBackColor = true;
            // 
            // rb5V
            // 
            this.rb5V.AutoSize = true;
            this.rb5V.Location = new System.Drawing.Point(12, 26);
            this.rb5V.Name = "rb5V";
            this.rb5V.Size = new System.Drawing.Size(41, 17);
            this.rb5V.TabIndex = 5;
            this.rb5V.Text = "5 V";
            this.rb5V.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.txtTimer);
            this.panel1.Controls.Add(this.rb33V);
            this.panel1.Controls.Add(this.rb5V);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Location = new System.Drawing.Point(516, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(390, 81);
            this.panel1.TabIndex = 6;
            // 
            // txtTimer
            // 
            this.txtTimer.Location = new System.Drawing.Point(76, 47);
            this.txtTimer.Name = "txtTimer";
            this.txtTimer.Size = new System.Drawing.Size(95, 20);
            this.txtTimer.TabIndex = 6;
            // 
            // grdVals
            // 
            this.grdVals.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdVals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grdVals.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNum,
            this.colVoltage,
            this.colNow,
            this.ColAVG,
            this.colMin,
            this.colMax});
            this.grdVals.Location = new System.Drawing.Point(516, 87);
            this.grdVals.Name = "grdVals";
            this.grdVals.Size = new System.Drawing.Size(390, 351);
            this.grdVals.TabIndex = 7;
            this.grdVals.VirtualMode = true;
            // 
            // aDCmeasureBindingSource
            // 
            this.aDCmeasureBindingSource.DataSource = typeof(MRS.Hardware.UI.Analyzer.ADCmeasure);
            // 
            // colNum
            // 
            this.colNum.DataPropertyName = "Channel";
            this.colNum.HeaderText = "#";
            this.colNum.Name = "colNum";
            this.colNum.Width = 30;
            // 
            // colVoltage
            // 
            this.colVoltage.DataPropertyName = "Voltage";
            this.colVoltage.HeaderText = "Voltage";
            this.colVoltage.Name = "colVoltage";
            this.colVoltage.Width = 50;
            // 
            // colNow
            // 
            this.colNow.DataPropertyName = "Now";
            this.colNow.HeaderText = "Now";
            this.colNow.Name = "colNow";
            this.colNow.Width = 50;
            // 
            // ColAVG
            // 
            this.ColAVG.DataPropertyName = "Avg";
            this.ColAVG.HeaderText = "Avg";
            this.ColAVG.Name = "ColAVG";
            this.ColAVG.Width = 50;
            // 
            // colMin
            // 
            this.colMin.DataPropertyName = "Min";
            this.colMin.HeaderText = "Min";
            this.colMin.Name = "colMin";
            this.colMin.Width = 50;
            // 
            // colMax
            // 
            this.colMax.DataPropertyName = "Max";
            this.colMax.HeaderText = "Max";
            this.colMax.Name = "colMax";
            this.colMax.Width = 50;
            // 
            // fmADC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 440);
            this.Controls.Add(this.grdVals);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pbImage);
            this.DoubleBuffered = true;
            this.Name = "fmADC";
            this.Text = "ADC Analyzer";
            this.Load += new System.EventHandler(this.fmADC_Load);
            this.Shown += new System.EventHandler(this.fmADC_Shown);
            this.ResizeEnd += new System.EventHandler(this.fmADC_ResizeEnd);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.fmStats_Paint);
            this.Resize += new System.EventHandler(this.fmADC_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdVals)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aDCmeasureBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Timer tmReader;
        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.RadioButton rb33V;
        private System.Windows.Forms.RadioButton rb5V;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtTimer;
        private System.Windows.Forms.DataGridView grdVals;
        private System.Windows.Forms.BindingSource aDCmeasureBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVoltage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNow;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColAVG;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMax;
    }
}