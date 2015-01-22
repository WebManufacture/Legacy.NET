using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MRS.Hardware.UART;

namespace MRS.Hardware.UI.Analyzer
{
    public partial class fmADC : Form
    {
        private Single[,] data = new Single[10, 6];
        private Single[] avg = new Single[6];
        private int[] min = new int[6];
        private int[] max = new int[6];
        private List<PointF[]> points = new List<PointF[]>(6);
        private int counter = 0;
        private int length = 0;
        int peakHeight = 100;
        int maxval = 1023;
        BindingList<ADCmeasure> results;
        BindingSource source;

        SerialPacketManager manager;



        private Brush[] colors = {
                                     Brushes.Blue, Brushes.Gold, Brushes.Green, Brushes.Tomato, Brushes.Violet, Brushes.Red, Brushes.SkyBlue, Brushes.SpringGreen
                                 };

        private Bitmap currentImage;
        private Bitmap gridImage;

        protected bool Started { get; set; }
        
        private void Stop()
        {
            manager.Send(40);
            Started = false;
        }

        private void Start()
        {
            manager.Send(42);
            Started = true;
        }
        
        public fmADC(SerialPacketManager manager)
        {
            InitializeComponent();
            Started = false;
            this.manager = manager;
            manager.OnReceive += manager_OnReceive;
        }

        private void fmADC_Load(object sender, EventArgs e)
        {
            results = new BindingList<ADCmeasure>();
            for (byte i = 0; i < 6; i++)
            {
                results.Add(new ADCmeasure(i));
            }
            results.AllowRemove = false;
            results.AllowNew = false;
            source = new BindingSource(results, null);
            grdVals.DataSource = source;
        }
        protected override void OnClosed(EventArgs e)
        {
            tmReader.Enabled = false;
            Stop();
            base.OnClosed(e);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (Started)
            {
                Stop();
                btnStart.Text = "Start"; 
            }
            else
            {
                counter = 0;
                Start();
                btnStart.Text = "Stop";
                for (var line = 0; line < 6; line++)
                {
                    avg[line] = 0;
                    min[line] = ADCmeasure.AdcMaxValue;
                    max[line] = 0;
                    for (var i = 0; i < data.GetUpperBound(0) + 1; i++)
                    {
                        data[i, line] = 0;

                    }
                }
            }
            tmReader.Enabled = Started;
        }


        void manager_OnReceive(byte[] bytes, SerialManager manager)
        {
            if (bytes.Length >= 12){
                if (counter >= length) { counter = 0; };
               // txtValue.Text = "";
                for (var i = 0; i < 6; i++)
                {
                    int val = bytes[i*2];
                    val = (val << 2);
                    val = val + (bytes[i * 2 + 1] >> 6);
                    var last = data[data.GetUpperBound(0) - 1, i];
                    if (counter > 0)
                    {
                        last = data[counter-1, i];
                    }
                    data[counter, i] = (last + val) / 2;
                    avg[i] = ((avg[i] * 600) + val) / 601;
                    Single center = (i + 1) * (peakHeight + 20);
                    Single pt = (avg[i] * peakHeight / maxval);
                    points[i][counter] = new PointF(counter, center - pt);
                    if (val < min[i]) min[i] = val;
                    if (val > max[i]) max[i] = val;
                    //txtValue.Text += i + ":   " + last + "\n";
                }
                counter += 1;
            }
        }

        private void tmReader_Tick(object sender, EventArgs e)
        {
            for (byte i = 0; i < 6; i++)
            {
                try
                {
                    Single now = data[counter, i];
                    results[i] = new ADCmeasure(i, (int)now, avg[i], min[i], max[i]);
                }
                catch(Exception err)
                {
                    
                }
                //txtValue.Text += i + ":   " + last + "      A: " + (int)avg[i] + "\n" + "   V: "  + voltage;
            }
            source.ResetBindings(false);
            pbImage.Invalidate();
        }

        private void fmStats_Paint(object sender, PaintEventArgs e)
        {

        }

        public void DrawValue(int shift, byte value, Color color)
        {

        }


        private void fmADC_Resize(object sender, EventArgs e)
        {
            fmADC_Shown(sender, e);
        }

        private void fmADC_ResizeEnd(object sender, EventArgs e)
        {
            fmADC_Shown(sender, e);
        }

        private void fmADC_Shown(object sender, EventArgs e)
        {
            if (pbImage.Width == 0 || pbImage.Height == 0) return;
            currentImage = new Bitmap(pbImage.Width, pbImage.Height);
            gridImage = new Bitmap(pbImage.Width, pbImage.Height);
            length = pbImage.Width;
            data = new Single[length, 6];
            points = new List<PointF[]>(6);
            for (var i = 0; i < 6; i++)
            {
                points.Add(new PointF[length]);
            }
            DrawGrid();
            pbImage.Invalidate();
        }

        private void DrawGrid()
        {
            Graphics graph = Graphics.FromImage(gridImage);

            for (int x = 0; x < pbImage.Width; x += 5)
            {
                graph.DrawLine(Pens.LightGray, x, 0, x, gridImage.Height);
            }

            for (int y = 0; y < pbImage.Height; y += 5)
            {
                graph.DrawLine(Pens.LightGray, 0, y, gridImage.Width, y);
            }

            int center = gridImage.Height / 2;
            // graph.DrawLine(Pens.Red, 0, center, gridImage.Width, center);
        }

        private void pbImage_Paint(object sender, PaintEventArgs e)
        {
            Graphics graph = Graphics.FromImage(currentImage);
            graph.FillRectangle(Brushes.White, 0, 0, pbImage.Width, pbImage.Height);
            graph.DrawImage(gridImage, 0, 0);

            for (var line = 0; line < 6; line++)
            {
                int center = (line + 1) * (peakHeight + 20);
                for (var i = 0; i < length; i++)
                {
                    var val = data[i, line];
                    graph.FillRectangle(colors[line], i, center - (val * peakHeight / maxval), 1, (val * peakHeight / maxval));
                }
                graph.DrawLines(Pens.Black, points[line]);
            }

            e.Graphics.DrawImage(currentImage, 0, 0);
        }

    }
}
