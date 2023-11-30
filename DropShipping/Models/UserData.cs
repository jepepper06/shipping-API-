namespace DropShipping.Models;

public class UserData : Base
{
	public long Id { get; set;}
	public long UserId { get; set;}
	public string PhoneNumber { get; set;}
	public string Password { get; set;}
	public string Email { get; set;}
	public string Name { get; set;}
	public string IdentificationDocument { get; set;}
	public User? User { get; set;}
}