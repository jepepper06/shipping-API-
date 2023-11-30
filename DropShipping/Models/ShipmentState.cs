using System.Text.Json.Serialization;

namespace DropShipping.Models;

public class ShipmentState : Base 
{
	public long  Id {get;set;}
	public ICollection<Order>? Orders {get;set;}
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public ShipmentStatus? ShipmentStatus{get;set;}
	public long ShipmentAgencyId {get;set;}
	public ShipmentAgency? ShipmentAgency {get;set;}
	public double Cost {get;set;}
}