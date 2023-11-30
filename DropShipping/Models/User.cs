using Microsoft.AspNetCore.Identity;

namespace DropShipping.Models;

public class User : Base
{
    // IT SEEMS LIKE USER_ADDRESS MUST BE HERE
    public long Id {get; set;}
    public UserData? UserData {get;set;}
    public UserAddress? UserAddress{get;set;}
    public ICollection<Order>? Orders { get;set;}
    public ICollection<UserRole>? UserRoles {get;set;}
}