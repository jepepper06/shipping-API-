using DropShipping.DTOs;
using DropShipping.Services;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace DropShipping.Controllers;
[ApiController]
[Route("user")]
public class UserController: Controller{
    private readonly IPurchaseService _purchaseService;
    private readonly ILoginService _loginService;
    private readonly IUserRegistrationService _registrationService;
    public UserController
        (PurchaseService purchaseService,
        LoginService loginService,
        UserRegistrationService registrationService){
        _purchaseService = purchaseService;
        _loginService = loginService;
        _registrationService = registrationService;

    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO){
        var loginResult = await _loginService.Login(userLoginDTO);
        return loginResult.Match<IActionResult>(
            porsi => Ok(loginResult.Value),
            porno => Problem(loginResult.FirstError.Description,null,null,loginResult.FirstError.Code,null)
        );
    }
    
    [HttpPost("registration")]
    public async Task<IActionResult> Registration( UserRegisterDTO userRegisterDTO){
        var result = await _registrationService.RegisterUser(userRegisterDTO);
        if(result == true){
            return Ok();
        }
        return Problem();
    }

}