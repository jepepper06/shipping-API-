using DropShipping.DTOs;
using DropShipping.Models;
using DropShipping.DAOs;

namespace DropShipping.Services;

public class UserRegistrationService : IUserRegistrationService
{
    private readonly UserDAO _userDAO;
    public UserRegistrationService(UserDAO userDAO){
        _userDAO = userDAO;
    }
    public async Task<bool> RegisterUser(UserRegisterDTO userRegisterDTO)
    {
        User user = userRegisterDTO.ToUser();

        var result = await _userDAO.Save(user);

        if(result.IsError)
            return false;
            
        return true;
    }
}