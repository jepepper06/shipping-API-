using DropShipping.DAOs;
using DropShipping.DTOs;
using ErrorOr;

namespace DropShipping.Services;

public class LoginService : ILoginService
{
    private UserDAO _userDAO;
    public LoginService(UserDAO userDAO)
    {
        _userDAO = userDAO;
    }
    public async Task<ErrorOr<bool>> Login(UserLoginDTO userDTO)
    {
        var userResult = await _userDAO.GetUserByUserNameOrEmail(userDTO.UserName);
        if(userResult.IsError)
            return userResult.FirstError;
        
        var user = userResult.Value;
        if(user.UserData!.Password != userDTO.Password){
            return false;
        }
        return true;
    }
}