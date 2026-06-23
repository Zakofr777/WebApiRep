using System;
using BankingApplication.Models;
using BankingApplication.Service;
using BankingApplication.Services;

namespace BankingApplication;

class Program
{
    static void Main(string[] args)
    {
        LoggerService logger = new LoggerService();
        FileService fileService = new FileService(); 
        AuthService authService = new AuthService(fileService, logger);
        BankService bankService = new BankService(fileService, logger);

        logger.LogInfo("ATM Application started successfully.");

        Console.WriteLine("=================================");
        Console.WriteLine("   Welcome to the ATM!       ");
        Console.WriteLine("=================================");

        while (true)
        {
            try
            {
                Console.WriteLine("\n[ ATM - PLEASE INSERT YOUR CARD ]");
                Console.Write("Enter 16-digit Card Number: ");
                string cardNumber = Console.ReadLine() ?? "";

                Console.Write("Enter Expiration Date (MM/YY): ");
                string expDate = Console.ReadLine() ?? "";

                Console.Write("Enter CVC: ");
                string cvc = Console.ReadLine() ?? "";

                User? authenticatedUser = authService.IsValidCard(cardNumber, expDate, cvc);

                if (authenticatedUser == null)
                {
                    continue; 
                }

                Console.Write("Enter PIN Code: ");
                string pin = Console.ReadLine() ?? "";

                if (!authService.VerifiyPin(authenticatedUser, pin))
                {
                    continue; 
                }

                Console.WriteLine($"\n>>> Welcome back, {authenticatedUser.username} {authenticatedUser.lastName}! <<<");
                
                bool inSession = true;
                while (inSession)
                {
                    Console.WriteLine("\n========================");
                    Console.WriteLine("       ATM MENU         ");
                    Console.WriteLine("========================");
                    Console.WriteLine("1. Check Balance");
                    Console.WriteLine("2. Deposit Money (Add Amount)");
                    Console.WriteLine("3. Withdraw Money (Get Amount)");
                    Console.WriteLine("4. Currency Conversion (Change Amount)");
                    Console.WriteLine("5. View Last 5 Transactions");
                    Console.WriteLine("6. Change PIN Code");
                    Console.WriteLine("7. Exit / Eject Card");
                    Console.Write("Select an option (1-7): ");

                    string choice = Console.ReadLine() ?? "";

                    switch (choice)
                    {
                        case "1":
                            bankService.DisplayBalance(authenticatedUser);
                            break;

                        case "2":
                            Console.Write("Enter currency (GEL/USD/EUR): ");
                            string depCurrency = Console.ReadLine() ?? "";
                            Console.Write("Enter amount to deposit: ");
                            
                            if (decimal.TryParse(Console.ReadLine(), out decimal depAmount))
                            {
                                bankService.deposit(authenticatedUser, depCurrency, depAmount);
                            }
                            else
                            {
                                Console.WriteLine("Invalid amount format. Please enter numbers only.");
                                logger.LogInfo($"User {authenticatedUser.username} entered non-numeric deposit amount.");
                            }
                            break;

                        case "3":
                            Console.Write("Enter currency (GEL/USD/EUR): ");
                            string witCurrency = Console.ReadLine() ?? "";
                            Console.Write("Enter amount to withdraw: ");
                            
                            if (decimal.TryParse(Console.ReadLine(), out decimal witAmount))
                            {
                                bankService.Withdraw(authenticatedUser, witCurrency, witAmount);
                            }
                            else
                            {
                                Console.WriteLine("Invalid amount format. Please enter numbers only.");
                                logger.LogInfo($"User {authenticatedUser.username} entered non-numeric withdrawal amount.");
                            }
                            break;

                        case "4":
                            Console.Write("Enter source currency (e.g., GEL): ");
                            string fromCurr = Console.ReadLine() ?? "";
                            Console.Write("Enter target currency (e.g., USD): ");
                            string toCurr = Console.ReadLine() ?? "";
                            Console.Write("Enter amount to convert: ");
                            
                            if (decimal.TryParse(Console.ReadLine(), out decimal convAmount))
                            {
                                bankService.ConvertCurrency(authenticatedUser, fromCurr, toCurr, convAmount);
                            }
                            else
                            {
                                Console.WriteLine("Invalid amount format.");
                            }
                            break;

                        case "5":
                            bankService.DisplayLastTransactions(authenticatedUser);
                            break;

                        case "6":
                            Console.Write("Enter new 4-digit PIN: ");
                            string newPin = Console.ReadLine() ?? "";
                            bankService.ChangePin(authenticatedUser, newPin);
                            break;

                        case "7":
                            Console.WriteLine("Card ejected. Thank you for choosing our bank!");
                            logger.LogInfo($"User {authenticatedUser.username} logged out.");
                            inSession = false; 
                            break;

                        default:
                            Console.WriteLine("Invalid option. Please choose between 1 and 7.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

Console.WriteLine("An unexpected system error occurred. Please try again.");
                logger.LogError("Global application exception caught in Main loop", ex);
            }
        }
    }
}