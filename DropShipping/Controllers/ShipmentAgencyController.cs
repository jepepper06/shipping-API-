namespace DropShipping.Controllers;

using DropShipping.DAOs;
using Microsoft.AspNetCore.Mvc;
using DropShipping.Contracts;
using DropShipping.Models;
using System;
using ErrorOr;

[ApiController]
[Route("shipment-agencies")]
public class ShipmentAgencyController : Controller
{
    private readonly ShipmentAgencyDAO shipmentAgencyDAO;

    public ShipmentAgencyController(ShipmentAgencyDAO _shipmentAgencyDAO){
        shipmentAgencyDAO = _shipmentAgencyDAO;
    }

    [HttpPost]
    public async Task<IActionResult> CreateShipmentAgency(ShipmentAgencyRequest request)
    {
        // TODO VALIDATION
        ShipmentAgency shipmentAgency = new ShipmentAgency
        {
            Name = request.Name,
            Email = request.Email,
            ContactNumber = request.ContactNumber,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await shipmentAgencyDAO.Save(shipmentAgency);
        return result.Match(
            shipmentAgency => Ok(shipmentAgency), 
            errors => Problem(
                result.FirstError.Description,
                null,
                StatusCodes.Status400BadRequest,
                "Error on Creating Shipment Agency",
                result.FirstError.Code));
    }

    [HttpGet]
    public async Task<IActionResult> AllShipmentAgencies(int page)
    {
        var result = await shipmentAgencyDAO.GetAll(page);
        return result.Match(
            shipmentAgencies => Ok(result.Value),
            errors => Problem(result.FirstError.Description,null,StatusCodes.Status500InternalServerError,result.FirstError.Code,null));
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetShipmentAgencyById(long id)
    {
        ErrorOr<ShipmentAgency> result = await shipmentAgencyDAO.GetById(id);
        Error error = result.FirstError;
        return result.Match(
            shipmentAgency => Ok(result),
            errors => Problem(
                error.Description,
                null,
                StatusCodes.Status404NotFound,
                error.Code,
                null
            ));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateShipmentAgency(long id, ShipmentAgencyRequest request)
    {
        ShipmentAgency shipmentAgency = new();
        shipmentAgency.Id = id;
        shipmentAgency.Name = request.Name;
        shipmentAgency.Email = request.Email;
        shipmentAgency.ContactNumber = request.ContactNumber;
        shipmentAgency.UpdatedAt = DateTime.UtcNow;
        var result = await shipmentAgencyDAO.Upsert(shipmentAgency);

        return result.Match(
            shipmentAgency => Ok(shipmentAgency),
            errors => Problem());
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteShipmentAgency(long id)
    {
        ErrorOr<bool> result =  await shipmentAgencyDAO.Delete(id);
        return result.Match(
            correct => Ok(result),
            error => Problem(
                detail:result.FirstError.Description,
                statusCode:StatusCodes.Status500InternalServerError,
                title:result.FirstError.Code));
    }
}
