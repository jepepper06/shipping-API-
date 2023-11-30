using DropShipping.Config;
using DropShipping.DAOs;
using DropShipping.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DropShipping.Controllers;

[Route("authentication")]
[ApiController]
public class AuthenticationController : Controller
{
    private readonly UserDataDAO _userData;
    private readonly JwtConfig _jwtConfig;
    public AuthenticationController(UserDataDAO userData,JwtConfig jwtConfig){
        _userData = userData;
        _jwtConfig = jwtConfig;
    }
    // public async Task<IActionResult> Register(UserRegisterDTO request){
    //     await Task.Delay(1000);
    //     if(!ModelState.IsValid){
    //         return BadRequest("The Requested Data was not correct!");
    //     }
    //     if(await _userData.IsEmailRegistered(request.Email))
    //     {
    //         return BadRequest("Email Already Exist Please Login");
    //     }
        
    //     return Ok(new AuthResultDTO());
    // }
}