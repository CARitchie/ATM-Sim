using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    public class Account
    {
        private int balance;
        private int pin;
        private int accountNum;
        private int fails;              // Number of times that an incorrect pin has been entered
        private bool locked = false;

        // a constructor that takes initial values for each of the attributes (balance, pin, accountNumber)
        public Account(int balance, int pin, int accountNum)
        {
            this.balance = balance;
            this.pin = pin;
            this.accountNum = accountNum;
        }

        //getter and setter functions
        public int getBalance()
        {
            return balance;
        }

        public int getAccountNum()
        {
            return accountNum;
        }

        public bool GetLocked()
        {
            return locked;
        }

        public void setBalance(int newBalance)
        {
            this.balance = newBalance;
        }


        /*
        * This funciton check the account pin against the argument passed to it
        *
        * returns:
        * true if they match
        * false if they do not
        */
        public Boolean checkPin(int pinEntered)
        {
            if (pinEntered == pin)
            {
                fails = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        // Method to increase the number of failed attempts
        // If this exceeds 2, lock the account and return true
        public bool CheckFail()
        {
            fails++;
            if(fails > 2)
            {
                locked = true;
                return true;
            }
            else
            {
                return false;
            }

        }


        /*
        *   This funciton allows us to decrement the balance of an account
        *   it perfomes a simple check to ensure the balance is greater tha
        *   the amount being debeted
        *   
        *   reurns:
        *   true if the transactions if possible
        *   false if there are insufficent funds in the account
        */
        public Boolean decrementBalance(int amount)
        {
            if (this.balance >= amount)
            {
                balance -= amount;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
