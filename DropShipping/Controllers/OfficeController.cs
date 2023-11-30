namespace DropShipping.Controllers;

using DropShipping.DAOs;
using Microsoft.AspNetCore.Mvc;
using DropShipping.Contracts;
using DropShipping.Models;
using System;
using ErrorOr;

[Route("offices")]
[ApiController]
public class OfficeController : Controller
{
    private readonly OfficeDAO officeDAO;

    public OfficeController(OfficeDAO _officeDAO){
        officeDAO = _officeDAO;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOffice([FromBody] OfficeRequest request)
    {
        // TODO VALIDATION
        Office office = new Office
        {
            Name = request.Name,
            CityId = request.CityId,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            PostalCode = request.PostalCode
        };

        var result = await officeDAO.Save(office);
        return result.Match(
            office => Ok(office), 
            errors => Problem(
                result.FirstError.Description,
                null,
                StatusCodes.Status400BadRequest,
                "Error on Creating Office",
                result.FirstError.Code));
    }

    [HttpGet("all/{page:long}")]
    public async Task<IActionResult> AllOffices(int page)
    {
        var result = await officeDAO.GetAll(page);
        return result.Match(
            offices => Ok(offices),
            errors => Problem(result.FirstError.Description,null,StatusCodes.Status500InternalServerError,$"Error fetching All Offices Page {page}",result.FirstError.Code));
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetOfficeById(long id)
    {
        var result = await officeDAO.GetById(id);
        Error error = result.FirstError;
        return result.Match(
            office => Ok(result),
            errors => Problem(
                error.Description,
                null,
                StatusCodes.Status404NotFound,
                error.Code,
                null
            ));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateOffice(long id, [FromBody] OfficeRequest request)
    {
        Office office = new Office();
        
        office.Name = request.Name;
        office.CityId = request.CityId;
        office.Email = request.Email;
        office.UpdatedAt = DateTime.UtcNow;
        var result = await officeDAO.Upsert(office);
        return result.Match(
            office => Ok(office),
            errors => Problem());
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteOffice(long id)
    {
        ErrorOr<bool> result =  await officeDAO.Delete(id);
        if(result.IsError){
            return Problem(detail:result.FirstError.Description,statusCode:StatusCodes.Status500InternalServerError,title:result.FirstError.Code);
        }
        return Ok(result);
    }
}
