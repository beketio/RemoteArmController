using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArmController.Core;
using ArmController.Gui;

namespace ArmController
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Controller controller = new Controller();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm mainForm = new MainForm(controller);
            Application.Run(mainForm);

            mainForm.Dispose();
            // controller.Dispose();
        }
    }
}
