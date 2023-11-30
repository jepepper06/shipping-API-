using Microsoft.AspNetCore.Mvc;
using DropShipping.Services;
using DropShipping.DAOs;
namespace DropShipping.Controllers;

[ApiController]
[Route("shipment-status")]
public class ShipmentStateController : Controller
{
    private readonly ShipmentStateDAO _shipmentStateDAO;
    private readonly AddOrderToShipmentManagerService _addOrdersToShipment;
    public ShipmentStateController
        (AddOrderToShipmentManagerService addOrderToShipment,
        ShipmentStateDAO shipmentStateDAO)
        {
            _shipmentStateDAO = shipmentStateDAO;
            _addOrdersToShipment = addOrderToShipment;
        }
    
    [HttpPost("{shipmentAgencyId:long}")]
    public async Task<IActionResult> CreateShipmentStatus(long shipmentAgencyId)
    {
        bool result = await _addOrdersToShipment.AddOrdersToShipment(shipmentAgencyId);
        if(result == true){
            return Created("Pending orders added to a Shipment",result);
        }
        return Problem("failed to create Shipment");
    }

    [HttpGet("all/{page:int}")]
    public async Task<IActionResult> GetAll(int page)
    {
        var result = await _shipmentStateDAO.GetAll(page);
        return result.Match<IActionResult>
            (ok => Ok(result.Value),
            errors => Problem(result.FirstError.Code));
    }
}