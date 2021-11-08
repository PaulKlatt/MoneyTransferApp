using System;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoServer.Models;

namespace TenmoClient
{
    public class ConsoleService
    {
        /// <summary>
        /// Prompts for transfer ID to view, approve, or reject
        /// </summary>
        /// <param name="action">String to print in prompt. Expected values are "Approve" or "Reject" or "View"</param>
        /// <returns>ID of transfers to view, approve, or reject</returns>
        public int PromptForTransferID(string action)
        {
            Console.WriteLine("");
            Console.Write("Please enter transfer ID to " + action + " (0 to cancel): ");
            if (!int.TryParse(Console.ReadLine(), out int auctionId))//this declares a new variable
            {
                Console.WriteLine("Invalid input. Only input a number.");
                return 0;
            }
            else
            {
                return auctionId;//only spits this out as a boolean if successful
            }
        }

        public LoginUser PromptForLogin()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            string password = GetPasswordFromConsole("Password: ");

            LoginUser loginUser = new LoginUser
            {
                Username = username,
                Password = password
            };
            return loginUser;
        }

        private string GetPasswordFromConsole(string displayMessage)
        {
            string pass = "";
            Console.Write(displayMessage);
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (!char.IsControl(key.KeyChar))
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Remove(pass.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine("");
            return pass;
        }
        public void PrintAccounts(List<Account> accounts)
        {
            if (accounts.Count < 1)
            {
                Console.WriteLine("There are currently no accounts.");
            }
            else
            {
                Console.WriteLine("ACCOUNTS:");
                int validChoice = 1;
                foreach (Account acc in accounts)
                {
                    Console.WriteLine($"{validChoice}: {acc.AccountId}");
                    validChoice++;
                }
                Console.WriteLine("Please choose one of the accounts listed above or enter 0 to exit.");
            }
        }

        public void PrintUserList(List<UserInfo> usersInfo, ApiUser user)
        {

            if (usersInfo.Count <= 1)
            {
                throw new Exception("No valid user to send funds.");
            }
            else
            {
                Console.WriteLine("USERS:");
                int validChoice = 1;
                UserInfo userToRemove = new UserInfo();
                foreach (UserInfo u in usersInfo)
                {
                    if (u.UserId != user.UserId)
                    {
                        Console.WriteLine($"{validChoice}: {u.UserId} - {u.Username}");
                        validChoice++;
                    }
                    else
                    {
                        userToRemove = u;
                    }
                }
                usersInfo.Remove(userToRemove);
            }
        }

        public void PromptForAccountThenPrintAccountBalance(List<Account> accounts)
        {
            if (accounts.Count >= 1)
            {
                int accountSelection = -1;
                while (!int.TryParse(Console.ReadLine(), out accountSelection) || accountSelection < 0 || accountSelection > accounts.Count)
                {
                    Console.WriteLine("Invalid input. Please enter the number of an account listed above or 0 to exit.");
                }
                Console.Clear();
                if (accountSelection != 0)
                {              
                    Console.WriteLine($"Your current balance in {accounts[accountSelection - 1].AccountId} is {accounts[accountSelection - 1].Balance:C}.");
                }
            }
        }

        public void PrintNewTransfer(Transfer transfer, ApiUser user, UserInfo otherUser)
        {
            Console.Clear();
            Console.WriteLine("TRANSFER CREATED SUCCESSFULLY!");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine($"Transfer Id: {transfer.TransferId}");

            if (transfer.TransferTypeId == 1)
            {
                Console.WriteLine("Transfer type: request");
                if (transfer.TransferStatusId == 1)
                {
                    Console.WriteLine("Transfer status: pending");
                }
                else if (transfer.TransferStatusId == 2)
                {
                    Console.WriteLine("Transfer status: approved");
                }
                else if (transfer.TransferStatusId == 3)
                {
                    Console.WriteLine("Transfer status: rejected");
                }
                Console.WriteLine($"Requester: {user.UserId} - {user.Username}");
                Console.WriteLine($"Account To: {transfer.AccountTo}");
                Console.WriteLine($"Requestee: {otherUser.UserId} - {otherUser.Username}");
                Console.WriteLine($"Account From: {transfer.AccountFrom}");
                Console.WriteLine($"Amount: {transfer.Amount:C}");
            }
            else if (transfer.TransferTypeId == 2)
            {
                Console.WriteLine("Tranfer type: send");
                Console.WriteLine("Transfer status: approved");
                Console.WriteLine($"Receiver: {otherUser.UserId} - {otherUser.Username}");
                Console.WriteLine($"Account To: {transfer.AccountTo}");
                Console.WriteLine($"Sender: {user.UserId} - {user.Username}");
                Console.WriteLine($"Account From: {transfer.AccountFrom}");
                Console.WriteLine($"Amount: {transfer.Amount:C}");
            }
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }

        public void PrintUserTransfers(List<Transfer> myTransfers)
        {           
            if (myTransfers.Count == 0)
            {
                Console.WriteLine("You currently have no transfers.");
            }
            else
            {
                Console.WriteLine("TRANSFERS:");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                foreach (Transfer transfer in myTransfers)
                {
                    Console.WriteLine($"Transfer Id: {transfer.TransferId}");
                    Console.WriteLine($"Account To: {transfer.AccountTo}");
                    Console.WriteLine($"Account From: {transfer.AccountFrom}");
                    Console.WriteLine($"Amount: {transfer.Amount:C}");
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                }
                Console.WriteLine("");
                
            }
        }

        public int PromptForTransferId(List<Transfer> myTransfers)
        {
            if (myTransfers.Count >= 1)
            {
                List<int> transferIds = new List<int>();
                foreach (Transfer transfer in myTransfers)
                {
                    transferIds.Add((int)transfer.TransferId);
                }

                int userSelection = -1;
                while (!int.TryParse(Console.ReadLine(), out userSelection) || (userSelection != 0 && !transferIds.Contains(userSelection)))
                {
                    Console.WriteLine("Invalid input. Please enter the number of a transfer listed above or enter 0 to exit.");
                }
                return userSelection;
            }
            else
            {
                throw new Exception("You currently have no transfers to view.");
            }
            
        }

        public Account PromptForAccount(List<Account> accounts, int type, bool isUser)
        {
            int accountSelection = -1;
            Account selectedAccount = null;
            while (!int.TryParse(Console.ReadLine(), out accountSelection) || accountSelection < 0 || accountSelection > accounts.Count)
            {
                Console.WriteLine("Invalid input. Please enter the number of an account listed above or enter 0 to exit.");
            }
            if (accountSelection != 0)
            {
                selectedAccount = accounts[accountSelection - 1];
                if (isUser == true)
                {
                    if (type == 1)
                    {
                        Console.WriteLine($"You will be requesting TE bucks for {selectedAccount.AccountId}.");
                    }
                    else if (type == 2)
                    {
                        Console.WriteLine($"You will be sending from {selectedAccount.AccountId}.  It's current balance is {selectedAccount.Balance:C}.");
                    }
                }
                else
                {
                    if (type == 1)
                    {
                        Console.WriteLine($"You will be requesting TE bucks from {selectedAccount.AccountId}.");
                    }
                    else if (type == 2)
                    {
                        Console.WriteLine($"You will be sending to {selectedAccount.AccountId}.");
                    }
                }
            }
            return selectedAccount;
        }

        public void PrintSelectedTransfer(Transfer transfer)
        {
            Console.Clear();
            Console.WriteLine("TRANSFER SELECTED:");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine($"Transfer Id: {transfer.TransferId}");

            // Could do something cool here with enumerators
            if (transfer.TransferTypeId == 1)
            {
                Console.WriteLine("Transfer type: request");
            }
            else if (transfer.TransferTypeId == 2)
            {
                Console.WriteLine("Tranfer type: send");
            }
            if (transfer.TransferStatusId == 1)
            {
                Console.WriteLine("Transfer status: pending");
            }
            else if (transfer.TransferStatusId == 2)
            {
                Console.WriteLine("Transfer status: approved");
            }
            else if (transfer.TransferStatusId == 3)
            {
                Console.WriteLine("Transfer status: rejected");
            }

            Console.WriteLine($"Account To: {transfer.AccountTo}");
            Console.WriteLine($"Account From: {transfer.AccountFrom}");
            Console.WriteLine($"Amount: {transfer.Amount:C}");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }
        public int PromptForRequestUpdate()
        {
            Console.WriteLine("What would you like to do to this request?");
            Console.WriteLine("1.  Approve");
            Console.WriteLine("2.  Reject");
            Console.WriteLine();
            Console.WriteLine("Please enter the number of the action you would like to take above or enter 0 to cancel.");
            int userSelection = -1;
            while (!int.TryParse(Console.ReadLine(), out userSelection) || userSelection < 0 || userSelection > 2)
            {
                Console.WriteLine("Invalid input. Please enter the number of an account listed above or enter 0 to exit.");
            }
            return userSelection;
        }
    }
}