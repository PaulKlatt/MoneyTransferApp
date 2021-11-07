using System;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoServer.Models;

namespace TenmoClient
{
    class Program
    {
        private static readonly ConsoleService consoleService = new ConsoleService();
        private static readonly AuthService authService = new AuthService();
        private static readonly ApiService apiService = new ApiService();
        private static ApiUser user;

        static void Main(string[] args)
        {
            Run();
        }

        private static void Run()
        {
            while (true)
            {
                int loginRegister = -1;
                while (loginRegister != 1 && loginRegister != 2)
                {
                    Console.WriteLine("Welcome to TEnmo!");
                    Console.WriteLine("1: Login");
                    Console.WriteLine("2: Register");
                    Console.Write("Please choose an option: ");

                    if (!int.TryParse(Console.ReadLine(), out loginRegister))
                    {
                        Console.WriteLine("Invalid input. Please enter only a number.");
                    }
                    else if (loginRegister == 1)
                    {
                        while (!UserService.IsLoggedIn()) //will keep looping until user is logged in
                        {
                            LoginUser loginUser = consoleService.PromptForLogin();
                            user = authService.Login(loginUser);
                            if (user != null)
                            {
                                UserService.SetLogin(user);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (loginRegister == 2)
                    {
                        bool isRegistered = false;
                        while (!isRegistered) //will keep looping until user is registered
                        {
                            LoginUser registerUser = consoleService.PromptForLogin();
                            isRegistered = authService.Register(registerUser);
                            if (isRegistered)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Registration successful. You can now log in.");
                                loginRegister = -1; //reset outer loop to allow choice for login
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection.");
                    }
                }
                if (UserService.IsLoggedIn())
                {
                    MenuSelection();
                }
            }
        }

        private static void MenuSelection()
        {
            int menuSelection = -1;
            bool firstTime = true;
            while (menuSelection != 0)
            {
                Console.WriteLine("");
                if (firstTime)
                {
                    Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                    firstTime = false;
                }
                else 
                {
                    Console.WriteLine("Please choose another selection.");
                }
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (menuSelection == 1)
                {
                    try
                    {
                        List<Account> accounts = apiService.ViewAccountsByUserId(user.UserId);
                        consoleService.PrintAccounts(accounts);

                        consoleService.PromptForAccountThenPrintAccountBalance(accounts);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                else if (menuSelection == 2)
                {
                    try
                    {
                        //Maybe were only passing a list of account id's, but this uses our already made methods
                        // Get the list of transfers

                        List<Transfer> myTransfers = apiService.GetTransfersByUserId(user.UserId);

                        //call console service to print list of transfers
                        consoleService.PrintAllUserTransfers(myTransfers);

                        //prompt for user selection of transfer id
                        int selectedTransferId = consoleService.PromptForTransferId(myTransfers);
                        if (selectedTransferId != 0)
                        {
                            Transfer selectedTransfer = new Transfer();
                            foreach (Transfer transfer in myTransfers)
                            {
                                if (transfer.TransferId == selectedTransferId)
                                {
                                    selectedTransfer = transfer;
                                }
                            }
                            //call console service to print transfer id details
                            consoleService.PrintSelectedTransfer(selectedTransfer);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else if (menuSelection == 3)
                {

                }
                else if (menuSelection == 4)
                {
                    try
                    {
                        Account receiverAccount = new Account();
                        Account userAccount = new Account();
                        decimal amount;
                        //Get the userAccount
                        List<Account> userAccounts = apiService.ViewAccountsByUserId(user.UserId);
                        if (userAccounts.Count >= 1)
                        {
                            consoleService.PrintAccounts(userAccounts);
                            userAccount = consoleService.PromptForAccount(userAccounts, 2, true);
                        }
                        else
                        {
                            // Maybe come back and replace with a custom exception?
                            throw new Exception("You have no valid accounts from which to send TE bucks.");
                        }
                        //Get the receiverAccount
                        List<UserInfo> usersInfo = apiService.GetAllUsers();
                        consoleService.PrintUserList(usersInfo, user);
                        Console.WriteLine("Please choose a user to receive your TE bucks.");

                        if (usersInfo.Count >= 1)
                        {
                            int userSelection = -1;
                            while (!int.TryParse(Console.ReadLine(), out userSelection) || userSelection < 0 || userSelection > usersInfo.Count)
                            {
                                Console.WriteLine("Invalid input. Please enter the number of a user listed above.");
                            }

                            List<Account> receiverAccounts = apiService.ViewAccountsByUserId(usersInfo[userSelection - 1].UserId);

                            consoleService.PrintAccounts(receiverAccounts);
                            if (receiverAccounts.Count >= 1)
                            {
                                receiverAccount = consoleService.PromptForAccount(receiverAccounts, 2, false);
                            }
                            else
                            {
                                // Maybe custom ex
                                throw new Exception("The user has no valid accounts to receive TE bucks.");
                            }
                        }

                        //Get the amount to send
                        Console.WriteLine("Please enter an amount to send.");

                        while (!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0 || amount > userAccount.Balance)
                        {
                            Console.WriteLine("Invalid input. Please enter a number less than or equal to your current balance or enter 0 to cancel.");
                        }
                        if (amount == 0)
                        {
                            // Not sure what message to put here, not really an error, just needed to get to the catch?
                            throw new Exception();
                        }
                        else
                        {
                            Transfer transfer = new Transfer();
                            transfer.AccountTo = (int)receiverAccount.AccountId;
                            transfer.AccountFrom = (int)userAccount.AccountId;
                            transfer.Amount = amount;
                            //call api service that goes to post endpoint
                            Transfer createdTransfer = apiService.SendAmount(transfer);

                            //call console service, feed it created transfer
                            consoleService.PrintNewTransfer(createdTransfer, receiverAccount.UserId, user.UserId);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else if (menuSelection == 5)
                {
                    //prompt for your account
                    try
                    {
                        Account requestFromAccount = new Account();
                        Account userAccount = new Account();
                        decimal amount;
                        //Get the userAccount
                        List<Account> userAccounts = apiService.ViewAccountsByUserId(user.UserId);
                        if (userAccounts.Count >= 1)
                        {
                            consoleService.PrintAccounts(userAccounts);
                            userAccount = consoleService.PromptForAccount(userAccounts, 1, true);
                        }
                        else
                        {
                            // Maybe come back and replace with a custom exception?
                            throw new Exception("You have no valid accounts from which to send TE bucks.");
                        }
                        //get requester from account
                        List<UserInfo> usersInfo = apiService.GetAllUsers();
                        consoleService.PrintUserList(usersInfo, user);
                        Console.WriteLine("Please choose a user to request TE bucks from.");

                        if (usersInfo.Count >= 1)
                        {
                            int userSelection = -1;
                            while (!int.TryParse(Console.ReadLine(), out userSelection) || userSelection < 0 || userSelection > usersInfo.Count)
                            {
                                Console.WriteLine("Invalid input. Please enter the number of a user listed above.");
                            }

                            List<Account> requestFromAccounts = apiService.ViewAccountsByUserId(usersInfo[userSelection - 1].UserId);

                            consoleService.PrintAccounts(requestFromAccounts);
                            if (requestFromAccounts.Count >= 1)
                            {
                                requestFromAccount = consoleService.PromptForAccount(requestFromAccounts, 1, false);
                            }
                            else
                            {
                                // Maybe custom ex
                                throw new Exception("The user has no valid accounts to receive TE bucks.");
                            }
                        }
                        //Get the amount to send
                        Console.WriteLine("Please enter an amount to request.");

                        while (!decimal.TryParse(Console.ReadLine(), out amount) || amount < 0)
                        {
                            Console.WriteLine("Invalid input. Please enter a number a valid number to request or enter 0 to cancel.");
                        }
                        if (amount != 0)
                        {
                            Transfer transfer = new Transfer();
                            transfer.TransferStatusId = 1;
                            transfer.TransferTypeId = 1;
                            transfer.AccountTo = (int)requestFromAccount.AccountId;
                            transfer.AccountFrom = (int)userAccount.AccountId;
                            transfer.Amount = amount;
                            //call api service that goes to post endpoint
                            Transfer newTransfer = apiService.MakeNewRequest(transfer);
                            Console.WriteLine("Request sent - currently pending.");
                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (menuSelection == 6)
                {
                    Console.WriteLine("");
                    UserService.SetLogin(new ApiUser()); //wipe out previous login info
                    Console.Clear();
                    menuSelection = 0;
                }
                else
                {
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                }
            }
        }
    }
}
