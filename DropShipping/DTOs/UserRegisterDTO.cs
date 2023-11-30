using DropShipping.Models;

namespace DropShipping.DTOs;

public class UserRegisterDTO{
    public string Name{get;set;}
    public string Email{ get; set; }
    public string PhoneNumber{get; set;}
    public string Password{ get; set;}
    public string IdentificationDocument{get; set;}
    public User ToUser(){
        var userData =  new UserData{
            Name = Name,
            Email = Email,
            PhoneNumber = PhoneNumber,
            Password = Password,
            IdentificationDocument = IdentificationDocument
        };
        return new User{
            UserData = userData
        };
    }
}
