namespace DropShipping.Controllers;

using DropShipping.DAOs;
using Microsoft.AspNetCore.Mvc;
using DropShipping.Contracts;
using DropShipping.Models;
using System;
using ErrorOr;

[ApiController]
[Route("roles")]
public class RoleController : Controller
{
    private readonly RoleDAO roleDAO;

    public RoleController(RoleDAO _roleDAO){
        roleDAO = _roleDAO;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(RoleRequest request)
    {
        // TODO VALIDATION
        Role role = new Role
        {
            Name = Enum.Parse<ERole>(request.Name),
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await roleDAO.Save(role);
        return result.Match(
            role => Ok(role), 
            errors => Problem(
                result.FirstError.Description,
                null,
                StatusCodes.Status400BadRequest,
                "Error on Creating Role",
                result.FirstError.Code));
    }

    [HttpGet("all/{pageNumber:long}")]
    public async Task<IActionResult> AllRoles(int pageNumber)
    {
        var result = await roleDAO.GetAll(pageNumber);
        return result.Match(
            roles => Ok(roles),
            errors => Problem(result.FirstError.Description,null,StatusCodes.Status500InternalServerError,result.FirstError.Code,null));
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetRoleById(long id)
    {
        ErrorOr<Role> result = await roleDAO.GetById(id);
        Error error = result.FirstError;
        return result.Match(
            role => Ok(result),
            errors => Problem(
                error.Description,
                null,
                StatusCodes.Status404NotFound,
                error.Code,
                null
            ));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateRole(long id, RoleRequest request)
    {
        Role role = new();
        role.Id = id;
        role.Name = Enum.Parse<ERole>(request.Name);
        role.Description = request.Description;
        role.UpdatedAt = DateTime.UtcNow;
        var result = await roleDAO.Upsert(role);

        return result.Match(
            role => Ok(role),
            errors => Problem());
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteRole(long id)
    {
        ErrorOr<bool> result =  await roleDAO.Delete(id);
        return result.Match(
            correct => Ok(result),
            error => Problem(
                detail:result.FirstError.Description,
                statusCode:StatusCodes.Status500InternalServerError,
                title:result.FirstError.Code));
    }
}
