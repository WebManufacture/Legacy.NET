using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MRS.Hardware.UI.Analyzer
{
    public static class Program
    {
        public static fmMain mainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainForm = new fmMain();
            Application.Run(mainForm);
        }

        public static void ShowMessage(string message)
        {
            mainForm.ShowMessage(message);
        }

        public static void LogError(string message)
        {
            mainForm.ShowError(message);
        }

    }
}
