namespace DropShipping.DTOs;

using DropShipping.Models;

public class CartResponseDTO{
    public CartResponseDTO(Order order){
        OrderId = order.Id;
        Total = order.Total;
        Items = order.Items!; 
    }
    public long OrderId{ get;}
    public double Total{get;}
    public ICollection<Item> Items{get;}
}