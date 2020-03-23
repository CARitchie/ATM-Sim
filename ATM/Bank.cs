using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATM
{
    public partial class Bank : Form
    {
        Account[] accounts;

        Thread thread1;
        Thread thread2;
        public Bank(Account[] ac)
        {
            accounts = ac;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            thread1 = new Thread(() =>
            {
                Application.Run(new Atm(accounts));
            });

            thread2 = new Thread(() =>
            {
                Application.Run(new Atm(accounts));
            });

            thread1.Start();
            thread2.Start();

        }
    }
}
