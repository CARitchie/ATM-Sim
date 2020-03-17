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
            Account[] ac = new Account[3];
            ac[0] = new Account(300, 1111, 111111);
            ac[1] = new Account(750, 2222, 222222);
            ac[2] = new Account(3000, 3333, 333333);
            Program prog = new Program();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            prog.MakeATM(ac);
        }

        void MakeATM(Account[] ac)
        {
            Application.Run(new Atm(ac));
        }
    }
}
