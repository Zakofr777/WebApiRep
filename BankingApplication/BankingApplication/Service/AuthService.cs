using BankingApplication.Models;

namespace BankingApplication.Service;

public class AuthService {
    
    private FileService fileService;

    public AuthService(FileService fileService) {
        this.fileService = fileService;
    }

    public User? IsValidCard(string cardNumber, string expirationDate, string CVC)
    {
        List<User> users = fileService.LoadUsers();

        foreach (User user in users) {
            if(cardNumber == user.creditCard.CardNumber && expirationDate == user.creditCard.ExpirationDate
               && CVC.Equals(user.creditCard.CVC)) {
                return user;
            }
        }

        Console.WriteLine("Please Provide Correct Data.");
        return null;
    }
    
    public bool VerifiyPin(User user, string pin)
    {
        try
        {
            if (user.creditCard.Pincode == pin)
            {
                return true; 
            }

            Console.WriteLine("Please Provide Correct Pin.");
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error during PIN verification: {e.Message}");
            return false;
        }
    }
    
}