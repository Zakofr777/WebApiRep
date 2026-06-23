using BankingApplication.Models;
using BankingApplication.Services;

namespace BankingApplication.Service;

public class BankService {
    
    private FileService fileService;
    private List<User> users;
    private LoggerService loggerService;
    
    private const decimal GelToUsdRate = 0.36m;
    private const decimal GelToEurRate = 0.33m;
    private const decimal UsdToGelRate = 2.78m;
    private const decimal EurToGelRate = 3.03m;
    
    public BankService(FileService fileService, LoggerService loggerService) {
        this.fileService = fileService;
        users = this.fileService.LoadUsers();
        this.loggerService = loggerService;
    }

    public void DisplayBalance(User currentUser)
    {
        Console.WriteLine("\n" + " --balance-- ");
        Console.WriteLine("GEL : " + currentUser.BalanceGEL);
        Console.WriteLine("USD : " + currentUser.BalanceUSD);
        Console.WriteLine("EUR : " + currentUser.BalanceEUR);
        
        loggerService.LogInfo($"User {currentUser.username} checked balance.");
    }

    public void Withdraw(User currentUser, string currency, decimal amount)
    {
        try
        {
            if (amount <= 0)
            {
                Console.WriteLine("Amount must be greater than zero.");
                loggerService.LogInfo($"User {currentUser.username} tried to withdraw invalid amount: {amount} {currency}.");
                return;
            }
            
            User realUser = users.First(u => u.creditCard.CardNumber == currentUser.creditCard.CardNumber);

            switch (currency.ToUpper())
            {
                case "GEL":
                    if(realUser.BalanceGEL < amount) {
                        Console.WriteLine("Not enough balance.");
                        return; }
                    realUser.BalanceGEL -= amount;
                    currentUser.BalanceGEL -= amount;
                    realUser.TransactionHistory.Add(new Transaction(DateTime.Now, "Withdraw" ,
                        amount, 0, 0));
                    break;
                case "USD":
                    if (realUser.BalanceUSD < amount) {
                        Console.WriteLine("Not enough balance.");
                        return;
                    }
                    realUser.BalanceUSD -= amount;
                    currentUser.BalanceUSD -= amount;
                    realUser.TransactionHistory.Add(new Transaction(DateTime.Now, "Withdraw" ,
                        0, amount, 0));
                    break;
                case "EUR":
                    if (realUser.BalanceEUR < amount) {
                        Console.WriteLine("Not enough balance.");
                        return;
                    }
                    realUser.BalanceEUR -= amount;
                    currentUser.BalanceEUR -= amount;
                    realUser.TransactionHistory.Add(new Transaction(DateTime.Now, "Withdraw" ,
                        0, 0, amount));
                    break;
                default:
                    Console.WriteLine("Not supported currency.");
                    loggerService.LogInfo($"User {currentUser.username} entered invalid currency for withdraw: {currency}.");
                    return;
            }
            fileService.SaveUsers(users);
            loggerService.LogInfo($"Successfully withdrew {amount} {currency} for User: {realUser.username}.");
        }
        catch (Exception e) {
            Console.WriteLine($"Error during withdrawal: {e.Message}");
            loggerService.LogError($"Deposit failed for User {currentUser.username}", e);
        }
    }

    public void deposit(User currentUser, string currency, decimal amount)
    {
        try
        {
            if (amount <= 0)
            {
                Console.WriteLine("Amount must be greater than zero.");
                loggerService.LogInfo($"User {currentUser.username} tried to deposit invalid amount: {amount} {currency}.");
                return;
            }
            
            User realUser = users.First(u => u.creditCard.CardNumber == currentUser.creditCard.CardNumber);

            switch (currency.ToUpper())
            {
                case "GEL":
                    realUser.BalanceGEL += amount;
                    currentUser.BalanceGEL += amount;
                    realUser.TransactionHistory.Add(new Transaction(DateTime.Now, "Deposit" ,
                        amount, 0, 0));
                    break;
                case "USD":
                    realUser.BalanceUSD += amount;
                    currentUser.BalanceUSD += amount;
                    realUser.TransactionHistory.Add(new Transaction(DateTime.Now, "Deposit" ,
                        0, amount, 0));
                    break;
                case "EUR":
                    realUser.BalanceEUR += amount;
                    currentUser.BalanceEUR += amount;
                    realUser.TransactionHistory.Add(new Transaction(DateTime.Now, "Deposit" ,
                        0, 0, amount));
                    break;
                default:
                    Console.WriteLine("Not supported currency.");
                    loggerService.LogInfo($"User {currentUser.username} entered invalid currency for deposit: {currency}.");
                    return;
            }
            fileService.SaveUsers(users);
            loggerService.LogInfo($"Successfully deposited {amount} {currency} for User: {realUser.username}.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error during deposit: " + e.Message);
            loggerService.LogError($"Deposit failed for User {currentUser.username}", e);
            throw;
        }
    }
    
    public void DisplayLastTransactions(User currentUser)
    {
        Console.WriteLine("\n--- Last 5 Transactions ---");
        
        User masterUser = users.First(u => u.creditCard.CardNumber == currentUser.creditCard.CardNumber);
        
        var lastTransactions = masterUser.TransactionHistory.TakeLast(5).ToList();

        if (lastTransactions.Count == 0)
        {
            Console.WriteLine("No transactions found.");
            return;
        }

        foreach (var t in lastTransactions)
        {
            Console.WriteLine( t.TransactionDate + " --- " +  t.TransactionType + " | GEL: " + t.AmountGel + " | USD: " +  t.AmountUSD + "| EUR: " + t.AmountEUR);
        }
        
        loggerService.LogInfo($"User {masterUser.username} viewed transaction history.");
    }
    
    public void ChangePin(User currentUser, string newPin)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(newPin) || newPin.Length != 4)
            {
                Console.WriteLine("PIN must be exactly 4 digits long.");
                loggerService.LogInfo($" {currentUser.username} invalid pin format occured while changing pin");
                return;
            }

            User masterUser = users.First(u => u.creditCard.CardNumber == currentUser.creditCard.CardNumber);
            masterUser.creditCard.Pincode = newPin;
            masterUser.TransactionHistory.Add(new Transaction(DateTime.Now,"PIN Changed", 0, 0, 0));

            fileService.SaveUsers(users);
            Console.WriteLine("PIN changed successfully!");
            
            fileService.SaveUsers(users);
            
            loggerService.LogInfo($"User {masterUser.username} successfully changed their PIN.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error changing PIN:" +  e.Message);
            loggerService.LogError("Error changing PIN: " + e.Message);
        }
    }
    
    public void ConvertCurrency(User currentUser, string fromCurrency, string toCurrency, decimal amountToConvert)
    {
        try
        {
            if (amountToConvert <= 0) { Console.WriteLine("Amount must be greater than zero."); 
                loggerService.LogInfo($" {currentUser.username} invalid amount to convert.");
                return;
            }

            User masterUser = users.First(u => u.creditCard.CardNumber == currentUser.creditCard.CardNumber);

            if (fromCurrency.ToUpper() == "GEL" && toCurrency.ToUpper() == "USD")
            {
                if (masterUser.BalanceGEL < amountToConvert) { Console.WriteLine("Insufficient GEL balance."); return; }
                decimal resultingUsd = amountToConvert * GelToUsdRate;
                masterUser.BalanceGEL -= amountToConvert;
                masterUser.BalanceUSD += resultingUsd;
                currentUser.BalanceGEL -= amountToConvert;
                currentUser.BalanceUSD += resultingUsd;
                
                masterUser.TransactionHistory.Add(new Transaction(DateTime.Now ,"Exchange (GEL to USD)", amountToConvert, resultingUsd, 0));
            }
            else if (fromCurrency.ToUpper() == "USD" && toCurrency.ToUpper() == "GEL")
            {
                if (masterUser.BalanceUSD < amountToConvert) { Console.WriteLine("Insufficient USD balance."); return; }
                decimal resultingGel = amountToConvert * UsdToGelRate;
                masterUser.BalanceUSD -= amountToConvert;
                masterUser.BalanceGEL += resultingGel;
                currentUser.BalanceUSD -= amountToConvert;
                currentUser.BalanceGEL += resultingGel;
                masterUser.TransactionHistory.Add(new Transaction(DateTime.Now,"Exchange (USD to GEL)", resultingGel, amountToConvert, 0));
            }
            else
            {
                Console.WriteLine("This currency conversion path is currently unsupported.");
                loggerService.LogInfo($" {currentUser.username} invalid currency to convert.");
                return;
            }

            fileService.SaveUsers(users);
            Console.WriteLine("Conversion completed successfully.");
            loggerService.LogInfo($"User {masterUser.username} successfully converted their amount.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error during currency conversion: {e.Message}");
            loggerService.LogError("Error during currency conversion: " + e.Message);
        }
    }
}