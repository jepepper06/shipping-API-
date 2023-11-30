namespace DropShipping.Controllers;

using DropShipping.DAOs;
using Microsoft.AspNetCore.Mvc;
using DropShipping.Contracts;
using DropShipping.Models;
using System;
using ErrorOr;

[ApiController]
[Route("useraddresses")]
public class UserAddressController : Controller
{
    private readonly UserAddressDAO userAddressDAO;

    public UserAddressController(UserAddressDAO _userAddressDAO){
        userAddressDAO = _userAddressDAO;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserAddress(UserAddressRequest request)
    {
        // TODO VALIDATION
        UserAddress userAddress = new UserAddress
        {
            UserId = request.UserId,
            Address = request.Address,
            CityId = request.CityId,
            PostalCode = request.PostalCode,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await userAddressDAO.Save(userAddress);
        return result.Match(
            userAddress => Ok(userAddress), 
            errors => Problem(
                result.FirstError.Description,
                null,
                StatusCodes.Status400BadRequest,
                "Error on Creating User Address",
                result.FirstError.Code));
    }

    [HttpGet]
    public async Task<IActionResult> AllUserAddresses(int page)
    {
        var result = await userAddressDAO.GetAll(page);
        return result.Match(
            userAddresses => Ok(userAddresses),
            errors => Problem(result.FirstError.Description,null,StatusCodes.Status500InternalServerError,result.FirstError.Code,null));
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetUserAddressById(long id)
    {
        ErrorOr<UserAddress> result = await userAddressDAO.GetById(id);
        Error error = result.FirstError;
        return result.Match(
            userAddress => Ok(result),
            errors => Problem(
                error.Description,
                null,
                StatusCodes.Status404NotFound,
                error.Code,
                null
            ));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateUserAddress(long id, UserAddressRequest request)
    {
        UserAddress userAddress = new();
        userAddress.Id = id;
        userAddress.UserId = request.UserId;
        userAddress.Address = request.Address;
        userAddress.CityId = request.CityId;
        userAddress.UpdatedAt = DateTime.UtcNow;
        var result = await userAddressDAO.Upsert(userAddress);

        return result.Match(
            userAddress => Ok(userAddress),
            errors => Problem());
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteUserAddress(long id)
    {
        ErrorOr<bool> result =  await userAddressDAO.Delete(id);
        return result.Match(
            correct => Ok(result),
            error => Problem(
                detail:result.FirstError.Description,
                statusCode:StatusCodes.Status500InternalServerError,
                title:result.FirstError.Code));
    }
}
