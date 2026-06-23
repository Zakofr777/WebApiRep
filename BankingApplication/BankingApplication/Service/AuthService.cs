using BankingApplication.Models;
using BankingApplication.Services;

namespace BankingApplication.Service;

public class AuthService {
    
    private FileService fileService;
    private LoggerService loggerService;
    

    public AuthService(FileService fileService, LoggerService loggerService) {
        this.fileService = fileService;
        this.loggerService = loggerService;
    }

    public User? IsValidCard(string cardNumber, string expirationDate, string CVC)
    {
        try
        {
            List<User> users = fileService.LoadUsers();
            foreach (User user in users) 
            {
                if (cardNumber == user.creditCard.CardNumber && 
                    expirationDate == user.creditCard.ExpirationDate &&
                    CVC == user.creditCard.CVC) 
                {
                    loggerService.LogInfo($"Card validation successful for User: {user.username}");
                    return user;
                }
            }

            loggerService.LogInfo($"Failed card validation attempt for Card Number: {cardNumber}"); 
            Console.WriteLine("Please Provide Correct Data.");
            return null;
        }
        catch (Exception e)
        {
            loggerService.LogError("Error during card verification", e); 
            return null;
        }
    }
    
    public bool VerifiyPin(User user, string pin)
    {
        try
        {
            if (user.creditCard.Pincode == pin)
            {
                loggerService.LogInfo($"PIN verification successful for User: {user.username}");
                return true; 
            }

            loggerService.LogInfo($"PIN verification failed for User: {user.username}");
            Console.WriteLine("Please Provide Correct Pin.");
            return false;
        }
        catch (Exception e)
        {
            loggerService.LogError("Error during verification of pin ", e);
            Console.WriteLine($"Error during PIN verification: {e.Message}");
            return false;
        }
    }
    
}