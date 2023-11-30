namespace DropShipping.Models;

public class Office : Base
{
    public long Id {get;set;}
    public string Name {get;set;}
    public string PhoneNumber {get;set;}
    public string Email {get;set;}
    public City? City {get;set;}
    public long CityId {get;set;}
    public int PostalCode {get;set;}
    public ICollection<Transport>? Transports {get;set;}
}