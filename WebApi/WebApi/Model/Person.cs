namespace WebApi;

public class Person {
    
    public DateTime CreateDate {get; set;}
    
    public string FirstName {get; set;}
    
    public string LastName {get; set;}

    public string jobPosition {get; set;}

    public Double Salary {get; set;}

    public Double WorkExperince {get; set;}

    public Address PersonAddress {get; set;}
    
    public Person(){}

    public Person(DateTime CreateDate, string FirstName, string LastName,
        string jobPosition, Double Salary, Double WorkExperince, Address PersonAddress)
    {
        this.CreateDate = CreateDate;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.jobPosition = jobPosition;
        this.Salary = Salary;
        this.WorkExperince = WorkExperince;
        this.PersonAddress = PersonAddress;
    }
}