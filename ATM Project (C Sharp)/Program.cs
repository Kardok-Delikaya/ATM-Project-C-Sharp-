using System;
using System.Collections.Generic;
using System.IO;

public class Account
{
    String cardNumber;
    int pin;
    String name;
    String surName;
    double balance;

    public Account(string cardNumber, int pin, string name, string surName, double balance)
    {
        this.cardNumber = cardNumber;
        this.pin = pin;
        this.name = name;
        this.surName = surName;
        this.balance = balance;
    }

    #region Gets
    public String CardNumber()
    {
        return cardNumber;
    }

    public int Pin()
    {
        return pin;
    }

    public String Name()
    {
        return name;
    }
    public String SurName()
    {
        return surName;
    }

    public double Balance()
    {
        return balance;
    }
    #endregion

    #region Sets
    public void SetCardNumber(String newCardNumber)
    {
        cardNumber = newCardNumber;
    }

    public void SetPin(int newPin)
    {
        pin = newPin;
    }

    public void SetName(String newName)
    {
        name = newName;
    }
    public void SetSurName(String newSurName)
    {
        surName = newSurName;
    }

    public void SetBalance(Double amount)
    {
        balance += amount;
        Console.WriteLine("Your new balance is: " + balance);
    }
    #endregion

    public static void Main(String[] args)
    {
        #region ATM
        void AddCurrency(Account account)
        {
            Console.WriteLine("Please enter the amount you want to add to your account: ");
            double amount = Double.Parse(Console.ReadLine());
            account.SetBalance(amount);
            EditAccount();
        }

        void WithdrarCurrency(Account account)
        {
            Console.WriteLine("Please enter the amount you want to withdraw from your account: ");
            double amount = Double.Parse(Console.ReadLine());

            if (amount <= account.balance)
            {
                account.SetBalance(-amount);
            }
            else
            {
                Console.WriteLine("Your balance is not enough for this process.");
            }

            EditAccount();
        }

        void ShowBalance(Account account)
        {
            Console.WriteLine("Your balance: " + account.balance);
        }
        #endregion

        TextReader tr = new StreamReader(@"DataBase.txt");
        int lineNum = File.ReadAllLines(@"DataBase.txt").Length;
        List<string> accountInformations = new List<string>();
        List<Account> accounts = new List<Account>();

        for (int i = 0; i < lineNum; i++)
        {
            string sentence = tr.ReadLine();
            accountInformations.Add(sentence);
        }

        for (int i = 0; i < accountInformations.Count; i++)
        {
            string[] words = accountInformations[i].Split(' ');
            accounts.Add(new Account(words[0], int.Parse(words[1]), words[2], words[3], double.Parse(words[4])));
        }
        tr.Close();
        String cardNum = "";
        Account curUser;

        void ATMScreen()
        {
            Console.WriteLine("Welcome to Kardok's Bank Atm");
            Console.WriteLine("Please write your 4 digit card number: ");

            while (true)
            {
                try
                {
                    cardNum = Console.ReadLine();
                    curUser = accounts.Find(a => a.cardNumber == cardNum);
                    if (curUser != null)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("You have entered wrong card number or you don't have an account on our bank.");
                        ATMScreen();
                    }
                }
                catch
                {
                    Console.WriteLine("An error has been catched. Please try again.");
                    ATMScreen();
                }
            }

            Console.WriteLine("Please enter your pin: ");
            GetPin();
        }

        void GetPin()
        {
            int curPin = 0;

            while (true)
            {
                try
                {
                    curPin = int.Parse(Console.ReadLine());
                    if (curUser.pin == curPin)
                    {
                        ATMChoices();
                    }
                    else
                    {
                        Console.WriteLine("You have entered wrong pin number. Please try again:");
                        GetPin();
                    }
                }
                catch
                {
                    Console.WriteLine("An error has been catched. Please try again.");
                    ATMScreen();
                }
            }
        }

        void ATMChoices()
        {
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. Add Currency");
            Console.WriteLine("2. Withdraw Currency");
            Console.WriteLine("3. Show Balance");
            Console.WriteLine("4. Delete Your Account");
            Console.WriteLine("5. Exit");
            int pick = int.Parse(Console.ReadLine());
            switch (pick)
            {
                case 1:
                    AddCurrency(curUser);
                    break;
                case 2:
                    WithdrarCurrency(curUser);
                    break;
                case 3:
                    ShowBalance(curUser);
                    break;
                case 4:
                    DeleteAccount();
                    break;
                case 5:
                    ATMScreen();
                    break;
                default:
                    Console.WriteLine("You have choosed poorly.");
                    break;
            }

            if (pick != 4)
            {
                ATMChoices();
            }
            else
            {
                ATMScreen();
            }
        }

        #region Account Options
        void EditAccount()
        {
            TextWriter writer = new StreamWriter(@"DataBase.txt");
            for (int i = 0; i < accounts.Count; i++)
            {
                accountInformations[i] = accounts[i].cardNumber + " " + accounts[i].pin + " " + accounts[i].name + " " + accounts[i].surName + " " + accounts[i].balance;
                writer.WriteLine(accountInformations[i]);
                
            }
            writer.Close();
        }

        void DeleteAccount()
        {
            accounts.Remove(curUser);

            EditAccount();
        }
        #endregion

        ATMScreen();
    }
}