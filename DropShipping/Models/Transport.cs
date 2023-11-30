namespace DropShipping.Models;

public class Transport: Base 
{
    // GOES DIRECTLY TO THE ENDPOINT
    public long Id {get;set;}
    public long OrderId {get;set;}
    public Order? Order {get;set;}
    public long OfficeId {get;set;}
    public Office? Office {get;set;}
    public string ClientDocument {get;set;}
    public bool IsInOffice {get;set;} = false;
    public bool Delivered {get;set;} = false;
}