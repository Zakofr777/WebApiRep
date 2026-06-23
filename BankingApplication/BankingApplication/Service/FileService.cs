using System.Text.Json;
using BankingApplication.Models;

namespace BankingApplication.Service;

public class FileService
{
    private string filePath = "OurBankUsers.json";
    
    public List<User> LoadUsers()
    {
        try {
            if (!File.Exists(filePath))
            {
                var DefaultUsers = SeedDefaultData();
                SaveUsers(DefaultUsers);
                return DefaultUsers;
            }
            
            string jsonString = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            List<User>? users = JsonSerializer.Deserialize<List<User>>(jsonString, options);

            return users == null ? new List<User>() : users;
            
        }catch (Exception e) {
            Console.WriteLine($"Error reading data file: {e.Message}");
            return new List<User>();
        }
    }

    public void SaveUsers(List<User> users)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            
            string jsonString = JsonSerializer.Serialize(users, options);

            File.WriteAllText(filePath, jsonString);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving data file: {e.Message}");
        }
    }

    private List<User> SeedDefaultData()
    {
        var card1 = new CreditCard("1234567890123456", "123", "12/28", "1111");
        
        var user1 = new User("John", "Doe", card1, 1500.00m, 200.00m, 50.00m, new List<Transaction>());
        
        user1.TransactionHistory.Add(new Transaction(
            DateTime.Now,          
            "Account Opened",    
            1500.00m,              
            200.00m,               
            50.00m           
        ));
        
        var card2 = new CreditCard("1111111111111111", "555", "11/26", "1323");
        
        var user2 = new User("gio", "labadze", card2, 150.00m, 200.00m, 50.00m, new List<Transaction>());
        
        user2.TransactionHistory.Add(new Transaction(
            DateTime.Now,          
            "Account Opened",    
            150.00m,              
            200.00m,               
            50.00m           
        ));

        var DefaultUsers = new List<User>();
        DefaultUsers.Add(user1);
        DefaultUsers.Add(user2);
        return DefaultUsers;
    }
}