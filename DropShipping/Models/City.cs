using System.Text.Json.Serialization;


namespace DropShipping.Models;

public class City : Base 
{
	public long Id { get; set; }
	public string Name { get; set; }
	public string Description {get; set;}
	public ICollection<Office>? Offices {get; set;}
}