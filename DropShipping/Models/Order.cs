namespace DropShipping.Models;

public class Order : Base
{
    public long Id {get;set;}
    public long UserId{get;set;}
    public User? User {get;set;}
    public Transport? Transport {get;set;}
    public long? ShipmentStateId{get;set;}
    public ShipmentState? ShipmentState {get;set;}
    public double Total {get;set;}
    public ICollection<Item>? Items {get;set;} = new List<Item>();
    public PaymentStatus? PaymentStatus {get;set;}
}