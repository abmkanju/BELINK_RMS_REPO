using Newtonsoft.Json;
using Pharmacy.UI.Franchise_Deployement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new SplashForm());
            }
            //catch (Exception ex)
            {
                //MessageBox.Show("An Unknown error occurred, please try again.", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //Application.Run(new ActivationForm());
        }
    }
}