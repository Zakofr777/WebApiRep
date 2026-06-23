namespace BankingApplication.Models;

public class Transaction {
    
    public DateTime TransactionDate { get; set; }
    public string TransactionType { get; set; }
    public decimal AmountGel { get; set; }
    public decimal AmountUSD { get; set; }
    public decimal AmountEUR { get; set; }

    public Transaction(DateTime TransactionDate, string TransactionType, decimal AmountGel, decimal AmountUSD, decimal AmountEUR) {
        this.TransactionDate = TransactionDate;
        this.TransactionType = TransactionType;
        this.AmountGel = AmountGel;
        this.AmountUSD = AmountUSD;
        this.AmountEUR = AmountEUR;
    }

    public Transaction() {
        
    }
    
}