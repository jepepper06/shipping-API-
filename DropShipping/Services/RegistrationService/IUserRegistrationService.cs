using DropShipping.DTOs;

namespace DropShipping.Services;

public interface IUserRegistrationService{
    Task<bool> RegisterUser(UserRegisterDTO newUser);
}