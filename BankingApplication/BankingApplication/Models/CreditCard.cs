namespace BankingApplication.Models;

public class CreditCard {
    public string CardNumber { get; set; }
    public string CVC { get; set; }
    public string ExpirationDate { get; set; }
    public string Pincode { get; set; }

    public CreditCard(string CardNumber, string CVC, string ExpirationDate, string Pincode) {
        this.CardNumber = CardNumber;
        this.CVC = CVC;
        this.ExpirationDate = ExpirationDate;
        this.Pincode = Pincode;
    }

    public CreditCard() {
        
    }
    
}