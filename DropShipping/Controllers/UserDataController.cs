namespace DropShipping.Controllers;

using DropShipping.DAOs;
using Microsoft.AspNetCore.Mvc;
using DropShipping.Contracts;
using DropShipping.Models;
using System;
using ErrorOr;

[ApiController]
[Route("userdata")]
public class UserDataController : Controller
{
    private readonly UserDataDAO userDataDAO;

    public UserDataController(UserDataDAO _userDataDAO){
        userDataDAO = _userDataDAO;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserData(UserDataRequest request)
    {
        // TODO VALIDATION
        UserData userData = new UserData
        {
            UserId = request.UserId,
            PhoneNumber = request.PhoneNumber,
            Password = request.Password,
            Email = request.Email,
            Name = request.Name,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await userDataDAO.Save(userData);
        return Ok(result);
    }

    [HttpGet("all/{page:int}")]
    public async Task<IActionResult> AllUserData(int page)
    {
        var result = await userDataDAO.GetAll(page);
        return result.Match(
            noError => Ok(result.Value),
            onError => Problem(
                detail:result.FirstError.Code,
                instance:null,
                statusCode:500,
                title:"Error On UserData",
                type:result.FirstError.Description
            )
        );
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetUserDataById(long id)
    {
        var result = await userDataDAO.GetById(id);
        return result.Match(
            userData => Ok(result),
            errors => Problem(
                result.FirstError.Description,
                null,
                StatusCodes.Status404NotFound,
                result.FirstError.Code,
                null
            ));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateUserData(long id, UserDataRequest request)
    {
        UserData userData = new UserData
        {
            Id = id,
            UserId = request.UserId,
            PhoneNumber = request.PhoneNumber,
            Password = request.Password,
            Email = request.Email,
            Name = request.Name,
            IdentificationDocument = request.IdentificationDocument,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await userDataDAO.Upsert(userData);

        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteUserData(long id)
    {
        ErrorOr<bool> result =  await userDataDAO.Delete(id);
        return Ok(result);
    }
}
