namespace WebApi;

public class Address
{
    public string Country {get; set;}
    
    public string City {get; set;}
    
    public string HomeNumber {get; set;}

    public Address(string Country, string City, string HomeNumber)
    {
        this.Country = Country;
        this.City = City;
        this.HomeNumber = HomeNumber;
    }
    
    public Address(){}
}