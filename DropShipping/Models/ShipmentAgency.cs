namespace DropShipping.Models;

public class ShipmentAgency : Base
{
	public long Id {get; set; }
	public string Name {get; set; }
	public string Email {get; set;}
	public string ContactNumber {get; set;}
	public ICollection<ShipmentState>? ShipmentStates {get; set;}
}