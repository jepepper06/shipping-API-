namespace DropShipping.Models;

public class Role : Base
{
	public long Id { get; set; }
	public ERole Name { get; set; } 
	public string Description {get; set;}
	public ICollection<UserRole>? UserRoles { get; set;}
}