namespace DropShipping.DTOs;

using DropShipping.Models;

public class OrderManagerResponseDTO{
    public OrderManagerResponseDTO(Order order){
        OrderId = order.Id;
        UserId = order.User!.Id;
        PaymentStatusId = order.PaymentStatus!.Id;
        Total = order.Total;
        UserName = order.User.UserData!.Name;
        IdentificationDocument = order.User.UserData.IdentificationDocument;
        Payed = order.PaymentStatus.Payed;
        PaymentMethod = order.PaymentStatus.PaymentMethod.ToString();
    }
    public long OrderId {get;}
    public long UserId {get;}
    public long PaymentStatusId {get;}
    public double Total {get;}
    public string UserName {get;}
    public string IdentificationDocument {get;}
    public bool Payed{get;}
    public string PaymentMethod {get;}

}