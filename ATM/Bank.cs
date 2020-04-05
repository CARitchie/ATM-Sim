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

        //handler for function that simulates semaphores
        private void button1_Click(object sender, EventArgs e)
        {
            Boolean dataRace = false;
            
            if (GetThreadActive())              // If any ATMs are still running
            {
                MessageBox.Show("Please close existing ATMs", "Warning");
            }
            else
            {
                runATMS(dataRace);
            }
            
        }

        //handler for function that simulates data race
        private void button2_Click(object sender, EventArgs e)
        {
            Boolean dataRace = true;
            
            if (GetThreadActive())              // If any ATMs are still running
            {
                MessageBox.Show("Please close existing ATMs", "Warning");
            }
            else
            {
                runATMS(dataRace);
            }
        }

        //This function runs two ATMs based on whether data race is being
        //simulated or not. The ATMs are run on separate threads so they
        //run simultaneously.
        private void runATMS(Boolean dataRace)
        {
            thread1 = new Thread(() =>
            {
                Application.Run(new Atm(accounts, dataRace,this,"ATM1"));
            });

            thread2 = new Thread(() =>
            {
                Application.Run(new Atm(accounts, dataRace,this,"ATM2"));
            });

            thread1.Start();
            thread2.Start();
        }


        // Method to detect whether any ATMs are running
        bool GetThreadActive()
        {
            if(thread1 != null && thread2 != null)
            {
                if(thread1.IsAlive || thread2.IsAlive)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private void ClrBtn_Click(object sender, EventArgs e)
        {
            BankLog.Items.Clear();
        }


        public void AddToLog(String TextToAdd)
        {
            this.Invoke((MethodInvoker)(() => BankLog.Items.Add(DateTime.Now.ToString() + "    " + TextToAdd)));
        }
    }
}
