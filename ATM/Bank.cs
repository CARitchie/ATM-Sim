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
        Account[] accounts = new Account[3];

        Thread thread1;
        Thread thread2;
        public Bank()
        {
            accounts[0] = new Account(300, 1111, 111111);
            accounts[1] = new Account(750, 2222, 222222);
            accounts[2] = new Account(3000, 3333, 333333);
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
