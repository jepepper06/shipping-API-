namespace DropShipping.Controllers;

using DropShipping.Services;
using Microsoft.AspNetCore.Mvc;
using DropShipping.Contracts;
using DropShipping.Models;
using System;
using ErrorOr;

[Route("manager")]
[ApiController]
public class ManagerController : Controller{
    private readonly OfficeManagerService _officeService;
    private readonly OrderManagerService _orderService;
    private readonly ShipmentStateManagerService _shipmentService;

    public ManagerController
        (OfficeManagerService officeService,
        OrderManagerService orderService,
        ShipmentStateManagerService shipmentService)
        {
            _officeService = officeService;
            _orderService = orderService;
            _shipmentService = shipmentService;
        }
    [HttpGet("all-orders/{page:int}")]
    public async Task<IActionResult> GetOrderManagerResources(int page){
        
        var resultOrders = await _orderService.GetOrderManagerDTOs(page);
        
        return resultOrders.Match(
            porsi => Ok(resultOrders.Value),
            porno => Problem(
                detail:null,
                instance: null,
                statusCode: 500,
                title: resultOrders.FirstError.Code,
                type: "Error On Orders Manager Service"));
    }
    [HttpGet("all-offices/{page:int}")]
    public async Task<IActionResult> GetOfficeManagerResource(int page){
        
        var resultOrders = await _officeService.GetAllOfficeManagerResponse(page);

        return resultOrders.Match(
            porsi => Ok(resultOrders.Value),
            porno => Problem(
                detail:resultOrders.FirstError.Description,
                instance: null,
                statusCode: 500,
                title: resultOrders.FirstError.Code,
                type: "Error On Orders Manager Service"));
    }
    [HttpGet("all-shipment/{page:int}")]
    public async Task<IActionResult> GetAllShipmentManagerResource(int page){
        var resultShipments = await _shipmentService.GetAllShipmentStates(page);
        return resultShipments.Match(
            porsi => Ok(resultShipments.Value),
            oporno => Problem(
                detail: resultShipments.FirstError.Description,
                instance: null,
                statusCode: StatusCodes.Status406NotAcceptable,
                title:resultShipments.FirstError.Code,
                type: "Error On Shipment Manager Service"
            ));
    }
}