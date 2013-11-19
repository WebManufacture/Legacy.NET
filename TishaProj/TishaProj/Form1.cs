using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TishaProj
{
    public partial class Form1 : Form
    {
        public Color[] colors = {
                                    Color.White,
                                    Color.Black,
                                    Color.Blue,
                                    Color.Yellow,
                                    Color.Red,
                                    Color.Purple,
                                    Color.Navy,
                                    Color.Green,
                                    Color.Orange,
                                    Color.Violet
                                };

        public String[] names = {
                                    "Белый",
                                    "Черный",
                                    "Голубой"
                                };

        public int currentColor = 0;
        public int nextColor = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // this.Full
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            currentColor++;
            if (currentColor >= colors.Length){
                currentColor = 0;
            }
            nextColor++;
            if (nextColor >= colors.Length){
                nextColor = 0;
            }
            this.BackColor = colors[currentColor];
            this.lblColor.ForeColor = colors[nextColor];
        }
    }
}
