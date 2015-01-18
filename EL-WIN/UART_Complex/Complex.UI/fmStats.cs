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
    public partial class fmStats : Form
    {
        string bytes = "";
        private byte data = 0;
        private byte counter = 0;
        SerialManager manager;

        private Brush[] colors = {
                                     Brushes.Blue, Brushes.Gold, Brushes.Green, Brushes.Tomato, Brushes.Violet, Brushes.Red, Brushes.SkyBlue, Brushes.SpringGreen
                                 };

        private Bitmap currentImage;
        
        public fmStats(SerialManager sm)
        {
            InitializeComponent();
            manager = sm;
        }

        protected override void OnClosed(EventArgs e)
        {
            tmReader.Enabled = false;
            manager.Close();
            base.OnClosed(e);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            counter = 0;
            tmReader.Enabled = true;
        }

        private void tmReader_Tick(object sender, EventArgs e)
        {
            //txtValue.Text += bytes;
            counter += 1;
            data = manager.ReadByte();
            txtValue.Text = data.ToString();
            pbImage.Invalidate();
        }

        private void fmStats_Paint(object sender, PaintEventArgs e)
        {
            
        }

        public void DrawValue(int shift, byte value, Color color)
        {
            
        }

        private void fmStats_ResizeEnd(object sender, EventArgs e)
        {
            currentImage = new Bitmap(pbImage.Width - 120, pbImage.Height);
        }

        private void fmStats_Shown(object sender, EventArgs e)
        {
            currentImage = new Bitmap(pbImage.Width - 120, pbImage.Height);
        }

        private void pbImage_Paint(object sender, PaintEventArgs e)
        {
            int peakWidth = 3;
            Graphics graph = Graphics.FromImage(currentImage);
            graph.DrawImage(currentImage, peakWidth, 0);
            graph.FillRectangle(Brushes.White, 0, 0, peakWidth, pbImage.Height);

            if (counter >= 5)
            {
                graph.DrawLine(Pens.Gray, 0, 0, 0, pbImage.Height);
                counter = 0;
            }

            int max = 40;

            for (int y = 0; y < pbImage.Height; y += 5)
            {
                graph.DrawLine(Pens.Gray, 0, y, peakWidth, y);
            }

            byte pow = 1;
            for (int i = 0; i < 8; i++)
            {
                int center = (i + 1) * max + 10;

                //graph.DrawLine(Pens.Red, 0, center + 1, peakWidth, center + 1);

                if ((data & pow) > 0)
                {
                    graph.FillRectangle(colors[i], 0, center - max, peakWidth, center);
                }
                else
                {
                    graph.FillRectangle(colors[i], 0, center - 1, peakWidth, center);
                }

                graph.DrawLine(Pens.Gray, 0, center, peakWidth, center);

                pow *= 2;
            }
           
            e.Graphics.DrawImage(currentImage, 0, 0);
        }
    }
}
