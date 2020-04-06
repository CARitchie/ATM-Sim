using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATM
{
    public partial class Atm : Form
    {
        String userInput = "";
        String lastMessage = "";
        int mode = 0;
        Account[] ac = new Account[3];
        Account account;
        Button[] controls;
        Boolean dataRace;

        public Atm(Account[] array, Boolean newDataRace)
        {
            InitializeComponent();

            ac = array;
            dataRace = newDataRace;

            controls = new Button[6] { Btn1, Btn2, Btn3, Btn4, Btn5, Btn6 };    // Add all menu buttons to an array
            EnableControls(false, 6);                                           // Disable all menu buttons

            lastMessage = "Enter your account number:";
            Screen.Items.Add(lastMessage);
        }


        // Method to control what happens when any number buttons are pressed
        void btnsEvent_Click(object sender, EventArgs e)
        {
            Button selected = sender as Button;
            if(mode < 2)                                                                            // If the ATM wants an account or pin number
            {
                if ((mode == 0 && userInput.Length < 6) || (mode == 1 && userInput.Length < 4))     // If the input isn't already too long
                {
                    userInput += selected.Text;                                                     // Add the buttons number to the userInput variable

                    ScreenClear();                                                                  // Remove the previous number from the screen

                    Screen.Items.Add(userInput);                                                    // Add the new number to the screen
                }
            }
        }


        // Method to control what happens when the clear button is clicked
        private void ClearBtn_Click(object sender, EventArgs e)
        {
            ScreenClear();
            userInput = "";     // Reset the userInput variable
        }


        // Method to control what happens when the enter button is clicked
        private void EnterBtn_Click(object sender, EventArgs e)
        {
            if(userInput != "")         // If userInput isn't empty
            {
                if (mode == 0)          // If the system wants an account number
                {
                    EnterAccount();
                }
                else if (mode == 1)     // Else if the system wants a pin number
                {
                    EnterPin();
                }
            }

        }


        // Method to control what happens when an account number has been entered
        public void EnterAccount()
        {
            account = GetAccount();
            userInput = "";                                     // Reset userInput
            if (account != null)                                // If an account has been found
            {
                Screen.Items.Clear();                           // Clear the screen
                mode = 1;                                       // Tell the system that a pin number is expected
                lastMessage = "Enter your pin number:";         // Change the value of lastMessage
                Screen.Items.Add(lastMessage);                  // Ask the user to enter their pin
            }
        }


        // Method to find and return the desired account
        public Account GetAccount()
        {
            int input = Int32.Parse(userInput);                 // Convert userInput into an integer
            Screen.Items.Clear();                               // Clear the screen
            Screen.Items.Add("Enter your account number:");     
            for (int i = 0; i < this.ac.Length; i++)            // Loop through all accounts
            {
                if (ac[i].getAccountNum() == input)             // If the current account number matches the entered one
                {
                    if (ac[i].GetLocked())                      // If the account is locked
                    {
                        Screen.Items.Add("Account is locked");  // Tell the user that the account is locked
                        return null;
                    }
                    return ac[i];                               // Return the correct account
                }
            }
            Screen.Items.Add("Incorrect account number");       // Tell the user that the entered number is not an account
            return null;                                        // Return an empty account
        }


        // Method to control what happens when a pin number has been entered
        public void EnterPin()
        {
            if (account.checkPin(Int32.Parse(userInput)))       // Convert userInput to an int and compare against the real pin
            {
                DisplayMenuOptions();
            }
            else                                                // If the pin wasn't valid                    
            {
                Screen.Items.Clear();                           // Clear the screen
                Screen.Items.Add(lastMessage);                  // Add back the last message
                Screen.Items.Add("Incorrect pin");              // Tell the user that the pin was incorrect
                userInput = "";
                if (account.CheckFail())                        // Check if the account has been incorrectly accessed too many times
                {
                    Screen.Items.Clear();
                    Screen.Items.Add("Too many attempts");      // Tell the user that they have attempted too many times
                    lastMessage = "Enter your account number:";
                    Screen.Items.Add(lastMessage);
                    mode = 0;                                   // Tell the system that an account number is expected
                }
            }
        }


        // Method to remove the last item on the screen that wasn't lastMessage
        public void ScreenClear()
        {
            if(Screen.Items.Count > 0)                                                  // If there is at least one item on the screen
            {
                if (Screen.Items[Screen.Items.Count - 1].ToString() != lastMessage)     // If the last item is not lastMessage
                {
                    Screen.Items.RemoveAt(Screen.Items.Count - 1);                      // Remove the last item
                }
            }
        }

        // Method to display the basic menu
        public void DisplayMenuOptions()
        {
            EnableControls(false, 6);           // Clear all menu buttons
            Screen.Items.Clear();
            userInput = "";
            lastMessage = "Welcome " + account.getAccountNum();
            Screen.Items.Add(lastMessage);
            mode = 2;                           // Tell the system that it is now in the basic menu
            EnableControls(true, 3);            // Enable the first three menu buttons
            Btn1.Text = "Withdraw";             // Set the menu button text
            Btn2.Text = "Check Balance";
            Btn3.Text = "Exit";
        }


        // Method to enable or disable menu buttons
        // Parameters:
        //      val - the value to set the buttons Enabled to
        //      quantity - the number of buttons that should be changed
        public void EnableControls(bool val, int quantity)
        {
            for(int i =0; i < quantity; i++)        // Loop through the desired number of buttons
            {
                controls[i].Enabled = val;          // Enable or disable the button
                if (!val)                           // If it is being disabled
                {
                    controls[i].Text = "";          // Clear the buttons text
                }
            }
        }


        // Handler for the first menu button
        // Either changes the menu to withdraw or withdraws £10
        private void Btn1_Click(object sender, EventArgs e)
        {
            if (mode == 2)                      // If the system is in the basic menu
            {
                ScreenClear();
                ScreenClear();
                ScreenClear();

                mode = 3;                       // Tell the system that it is in the withdraw menu
                EnableControls(true, 6);        // Enable all menu buttons

                Btn1.Text = "£10";              // Set the menu button text
                Btn2.Text = "£20";
                Btn3.Text = "£40";
                Btn4.Text = "£100";
                Btn5.Text = "£500";
                Btn6.Text = "Back";
            }
            else if (mode == 3)                 // Else if the system is in the withdraw menu
            {
                Withdraw(10);                   // Withdraw £10
            }
        }


        // Handler for the second menu button
        // Either displays the balance or withdraws £20
        private void Btn2_Click(object sender, EventArgs e)
        {
            if(mode == 2)                                                           // If the system is in the basic menu
            {
                ScreenClear();
                Screen.Items.Add("Account Balance £" + account.getBalance());       // Display the balance
            }
            else if (mode == 3)                                                     // Else if the system is in the withdraw menu
            {
                Withdraw(20);                                                       // Withdraw £20
            }

        }


        // Handler for the third menu button
        // Either exits or withdraws £40
        private void Btn3_Click(object sender, EventArgs e)
        {
            if(mode == 2)                                                           // If the system is in the basic menu
            {
                EnableControls(false, 6);                                           // Disable all menu buttons
                Screen.Items.Clear();
                Screen.Items.Add("Returning card. Goodbye!");
                lastMessage = "Enter your account number:";
                Screen.Items.Add(lastMessage);
                mode = 0;                                                           // Tell the system that an account number is expected
            }
            else if (mode == 3)                                                     // Else if the system is in the withdraw menu
            {
                Withdraw(40);                                                       // Withdraw £40
            }

        }


        // Handler for the fourth menu button, withdraws £100
        private void Btn4_Click(object sender, EventArgs e)
        {
            if (mode == 3)                                                          // If the system is in the withdraw menu
            {
                Withdraw(100);
            }
        }


        // Handler for the fith menu button, withdraws £500
        private void Btn5_Click(object sender, EventArgs e)
        {
            if(mode == 3)                                                           // If the system is in the withdraw menu
            {
                Withdraw(500);
            }
        }


        // Handler for the sixth menu button, returns to the basic menu
        private void Btn6_Click(object sender, EventArgs e)
        {
            EnableControls(false, 6);                                               // Disables all menu buttons
            DisplayMenuOptions();                                                   // Enables the basic menu
        }


        // Method to withdraw money
        // Parameters:
        //      amount - the amount of money to be withdrawn
        public void Withdraw(int amount)
        {
            ScreenClear();
            if (dataRace)
            {
                if (!account.dataRaceDecrementBalance(amount))                                  // If the money could not be withdrawn
                {
                    Screen.Items.Add("Insufficient funds");                             // Tell the user that they do not have enough money
                }
                else                                                                    // If the money was withdrawn
                {
                    DisplayMenuOptions();                                               // Enable the basic menu
                    Screen.Items.Add("Withdrawing £" + amount);                         // Tell the user that their money is being withdrawn
                    Screen.Items.Add("Race condition could happened");
                    Screen.Items.Add("Please check balance");
                    Console.WriteLine(amount + "£ have been withdrawed");

                }
            }
            else
            {
                if (!account.semaphoreDecrementBalance(amount))                                  // If the money could not be withdrawn
                {
                    Screen.Items.Add("Insufficient funds");                             // Tell the user that they do not have enough money
                }
                else                                                                    // If the money was withdrawn
                {
                    DisplayMenuOptions();                                               // Enable the basic menu
                    Screen.Items.Add("Withdrawing £" + amount);                         // Tell the user that their money is being withdrawn
                    Screen.Items.Add("Race condition was avoided");
                    Screen.Items.Add("Please check balance");
                    Console.WriteLine(amount + "£ have been withdrawed");
                }
            }

            
            
        }

    }
}
