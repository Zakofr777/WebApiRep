namespace BankingApplication.Models;

public class User
{
    public string username { get; set; }
    public string lastName { get; set; }
    public CreditCard creditCard  { get; set; }
    
    public decimal BalanceGEL { get; set; }
    public decimal BalanceUSD { get; set; }
    public decimal BalanceEUR { get; set; }

    public List<Transaction> TransactionHistory { get; set; }

    public User(string username, string lastName, CreditCard creditCard, 
        decimal BalanceGEL, decimal BalanceUSD, decimal BalanceEUR, List<Transaction> TransactionHistory) {
        this.username = username;
        this.lastName = lastName;
        this.creditCard = creditCard;
        this.BalanceGEL = BalanceGEL;
        this.BalanceUSD = BalanceUSD;
        this.BalanceEUR = BalanceEUR;
        this.TransactionHistory = TransactionHistory;
    }
}