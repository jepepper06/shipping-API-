using Microsoft.AspNetCore.Identity;

namespace DropShipping.Models;

public class UserRole : Base
{
	public long Id { get;set;}
	public long UserId { get; set;}
	public User? User {get; set;}
	public long RoleId{get;set;}
	public Role? Role { get; set;} 
}