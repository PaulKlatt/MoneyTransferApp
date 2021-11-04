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
                Console.WriteLine("You currently have no accounts.");
            }
            else if (accounts.Count == 1)
            {
                Console.WriteLine($"The current balance in your account is {accounts[0].Balance} TE bucks.");
            }
            else
            {
                Console.WriteLine("ACCOUNTS:");
                int validChoice = 1;
                foreach(Account acc in accounts)
                {
                    Console.WriteLine($"{validChoice}: {acc.AccountId}");
                    validChoice++;
                }
                Console.WriteLine("Please choose one of your accounts.");
            }
        }

        public void PrintUserList(List<UserInfo> usersInfo, ApiUser user)
        {
            
            if (usersInfo.Count <= 1)
            {
                Console.WriteLine("No valid users to send funds.");
            }
            else
            {
                Console.WriteLine("USERS:");
                int validChoice = 1;
                foreach (UserInfo u in usersInfo)
                {
                    if(u.UserId != user.UserId)
                    {
                        Console.WriteLine($"{validChoice}: {u.UserId} - {u.Username}");
                        validChoice++;
                    }
                }
                Console.WriteLine("Please choose a user to receive your TE bucks.");
            }
        }
    }
}
