using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATM
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            Program prog = new Program();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            prog.StartBank();
        }

        void StartBank()
        {
            Application.Run(new Bank());
        }
    }
}
