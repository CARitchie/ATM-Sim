using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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
        *   This funciton demonstrates Race Condition withdrawing of two
        *   users withdrawing money from the same account at the same time.
        *   The delay simulates this and allows a slight window for the users
        *   to withdraw at the same time. If user 2 withdraws within 5 seconds
        *   of user one then user1's transaction effectively gets overriden
        *   and only user2's transaction will show on the account balance
        *   
        *   reurns:
        *   true if the transactions if possible
        *   false if there are insufficent funds in the account
        */
        public Boolean dataRaceDecrementBalance(int amount)
        {
            int tempBalance = balance;

            if (tempBalance >= amount)
            {
                //delay here
                Thread.Sleep(5000);
                tempBalance -= amount;
                this.balance = tempBalance;
                return true;
            }
            else
            {
                return false;
            }
        }



        /*
         *  [ CRAIGS DEFAULT DECREMENT BALANCE ]
         *  
        *   This funciton allows us to decrement the balance of an account
        *   it perfomes a simple check to ensure the balance is greater tha
        *   the amount being debeted
        *   
        *   reurns:
        *   true if the transactions if possible
        *   false if there are insufficent funds in the account
        */
        public Boolean semaphoreDecrementBalance(int amount)
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
