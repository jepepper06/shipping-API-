using System.Text.Json.Serialization;

namespace DropShipping.Models;

public class PaymentStatus : Base
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public Order? Order {get;set;}
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PaymentMethod PaymentMethod { get; set; }
    public bool Payed { get; set; } = false;
}