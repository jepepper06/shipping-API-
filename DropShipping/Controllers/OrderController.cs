namespace DropShipping.Controllers;

using DropShipping.DAOs;
using DropShipping.Models;
using DropShipping.Services;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

[Route("orders")]
[ApiController]
public class OrderController : Controller
{
    private readonly PurchaseService _purchaseService;
    private readonly OrderDAO _orderDao;

    public OrderController
        (OrderDAO orderDao,
        PurchaseService purchaseService)
    {
        _orderDao = orderDao;
        _purchaseService = purchaseService;
    }


    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var order = await _orderDao.GetById(id);
        if (order.IsError)
        {
            return NotFound(order.Errors.First());
        }
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Save(Order order)
    {
        var x = await _orderDao.Save(order);
        if(x.IsError){
            return Problem(
                detail: x.FirstError.Code,
                instance: null,
                statusCode: StatusCodes.Status400BadRequest,
                title:"",
                type:"Internal Server Error");
        }
        return Ok();
    }
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id,Order order)
    {
        await _orderDao.Upsert(order);
        return Ok();
    }

    [HttpGet("all/{pageNumber:long}")]
    public async Task<IActionResult> GetAll( int pageNumber)
    {
        var orders = await _orderDao.GetAll(pageNumber);
        
        if (orders.Value != null)
            return Ok(orders.Value);
        
        return NotFound();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _orderDao.Delete(id);
        return Ok();
    }

    [HttpPost("add-item/{userId:long}/{productId:long}/{quantity:int}")]
    public async Task<IActionResult> AddItemToPurchase(long userId, long productId, int quantity){
        var  result = await _purchaseService.AddItemToOrder(userId,productId,quantity);
        return result.Match<IActionResult>(
            result => Ok(),
            result => Problem()
        );
    }
    [HttpGet("first/{userId:long}")]
    public async Task<IActionResult> GetFirtsByUserId(long userId){
        var result = await _orderDao.GetFirstByUserId(userId);
        return result.Match<IActionResult>(
            porsi => Ok(result.Value),
            oporno => Problem(result.FirstError.Code)
        );
    }
}