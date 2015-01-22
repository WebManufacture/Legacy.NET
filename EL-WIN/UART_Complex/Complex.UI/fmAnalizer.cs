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
    public partial class fmAnalizer : Form
    {
        string bytes = "";
        private byte[] data = new byte[100];
        private int counter = 0;
        SerialPacketManager manager;

        private Brush[] colors = {
                                     Brushes.Blue, Brushes.Gold, Brushes.Green, Brushes.Tomato, Brushes.Violet, Brushes.Red, Brushes.SkyBlue, Brushes.SpringGreen
                                 };

        private Bitmap currentImage;
        private Bitmap gridImage;

        public fmAnalizer()
        {
            InitializeComponent();
        }

        public fmAnalizer(SerialPacketManager manager)
        {
            InitializeComponent();
            this.manager = manager;
        }

        protected override void OnClosed(EventArgs e)
        {
            tmReader.Enabled = false;
            manager.Send(40);
            base.OnClosed(e);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            counter = 0;
            tmReader.Enabled = true;
            manager.Send(41);
        }

        private void tmReader_Tick(object sender, EventArgs e)
        {
            //txtValue.Text += bytes;
            counter += 1;
            if (manager.HasData())
            {
                var bytes = manager.ReadData();
                if (bytes != null && bytes.Length > 0)
                {
                    var offset = 0;
                    if (bytes.Length / data.Length > 1)
                    {
                        offset = bytes.Length - data.Length;
                    }
                    for (var i = 0; i < bytes.Length; i++)
                    {
                        if (counter >= data.Length) { counter = 0; };
                        data[counter] = bytes[i];
                    }
                    txtValue.Text = data[data.Length - 1] + "";
                }
            }
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
            currentImage = new Bitmap(pbImage.Width, pbImage.Height);
            gridImage = new Bitmap(pbImage.Width, pbImage.Height);
            data = new byte[pbImage.Width];DrawGrid();
            DrawGrid();
        }

        private void fmStats_Shown(object sender, EventArgs e)
        {
            currentImage = new Bitmap(pbImage.Width, pbImage.Height);
            gridImage = new Bitmap(pbImage.Width, pbImage.Height);
            data = new byte[pbImage.Width];
            DrawGrid();
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
            int peakWidth = 3;
            Graphics graph = Graphics.FromImage(currentImage);
            graph.FillRectangle(Brushes.White, 0, 0, pbImage.Width, pbImage.Height);
            graph.DrawImage(gridImage, 0, 0);
            
            for (var line = 0; line < 8; line++)
            {
                int center = line*30 + 40;
                for (var i = 0; i < data.Length; i++)
                {
                    var val = data[i] & (1 << line);
                    if (val > 0)
                    {
                        graph.FillRectangle(colors[line], i, center - 14, 1, 14);
                    }
                    else
                    {
                        graph.FillRectangle(colors[line], i, center, 1, 1);
                    }
                }                
            } 
                           
            e.Graphics.DrawImage(currentImage, 0, 0);
        }
    }
}
