using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hlab.CncTable.Win.UI
{
    public partial class fmLogs : Form
    {
        public fmLogs()
        {
            InitializeComponent();
        }

        public void Log(string msg)
        {
            if (log.Lines.Length > 60)
            {
                log.Lines[log.Lines.Length - 1] = null;
            }
            log.Text = (msg + "\n") + log.Text;
        }
    }

}
