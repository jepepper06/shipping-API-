using DropShipping.DTOs;
using ErrorOr;

namespace DropShipping.Services; 

public interface ILoginService{
    Task<ErrorOr<bool>> Login(UserLoginDTO userDTO);
}