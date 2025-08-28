using DVLD.Applications;
using DVLD.Employee;
using DVLD.People;
using DVLD.Tests;
using DVLD.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    // Application.Run(new frmMain());
        //  // Application.Run(new Form1(null));
        //  Application.Run(new frmNewLogin());



        //}
        [STAThread]
        static void Main()
        {
            if(Environment.OSVersion.Version.Major >= 6) 
            {
                SetProcessDPIAware();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
             
           //  Application.Run(new frmTest());
            Application.Run(new frmNewLogin());
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetProcessDPIAware();
    }
}
