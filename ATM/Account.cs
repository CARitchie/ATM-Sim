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
        public static Semaphore semaphore = new Semaphore(1, 2);



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
        *   returns:
        *   true if the transaction is possible
        *   false if there are insufficient funds in the account
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

        // Method to decrement the balance with semaphores to prevent a race condition
        // Parameters:
        //      amount - the amount of money to withdraw
        public Boolean semaphoreDecrementBalance(int amount)
        {
	        semaphore.WaitOne();
	        int tempBalance = balance;

	        if( tempBalance >= amount){
		        Thread.Sleep(5000);
		        tempBalance -= amount;
		        this.balance = tempBalance;
		        semaphore.Release();
		        return true;
	        }
            else
            {
		        semaphore.Release();
		        return false;
	        }
        }
    }
}
