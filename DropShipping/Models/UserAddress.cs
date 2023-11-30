namespace DropShipping.Models;

public class UserAddress : Base
{
    // GOES DIRECTLY TO THE ENDPOINT
    public long Id { get; set; }
    public long UserId { get; set; }
    public User? User { get; set;}
    public string Address { get; set; }
    public int Number { get; set;}
    public long CityId { get; set; }
    public City? City{get;set;}
    public int PostalCode { get; set;}
}