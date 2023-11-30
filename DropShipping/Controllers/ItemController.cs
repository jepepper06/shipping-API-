namespace DropShipping.Controllers;

using DropShipping.DAOs;
using Microsoft.AspNetCore.Mvc;
using DropShipping.Contracts;
using DropShipping.Models;
using System;
using ErrorOr;
using DropShipping.Services;

[Route("items")]
[ApiController]
public class ItemController : Controller
{
    private readonly ItemDAO itemDAO;
    private readonly CartService cartService;

    public ItemController(
        ItemDAO _itemDAO,
        CartService _cartService){
        itemDAO = _itemDAO;
        cartService = _cartService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateItem(ItemRequest request){
        // TODO VALIDATION
        Item item = new Item();
        item.ProductId = request.ProductId;
        item.OrderId = request.OrderId;
        item.CreatedAt = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;
        var result = await itemDAO.Save(item);
        return result.Match(
            item => Ok(item), 
            errors => Problem(
                result.FirstError.Description,
                null,
                StatusCodes.Status400BadRequest,
                "Error on Creating Item",
                result.FirstError.Code));
    }

    [HttpGet("all/{page:int}")]
    public async Task<IActionResult> AllItems(int page){
        var result = await itemDAO.GetAll(page);
        return result.Match(
            items => Ok(items),
            errors => Problem(result.FirstError.Description,null,StatusCodes.Status500InternalServerError,result.FirstError.Code,null));
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetItemById( long id){
        ErrorOr<Item> result = await itemDAO.GetById(id);
        Error error = result.FirstError;
        return result.Match(
            item => Ok(result),
            errors => Problem(
                error.Description,
                null,
                StatusCodes.Status404NotFound,
                error.Code,
                null
            ));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateItem(long id, ItemRequest request){
        Item item = new Item();
        item.Id = id;
        item.ProductId = request.ProductId;
        item.OrderId = request.OrderId;
        item.UpdatedAt = DateTime.UtcNow;
        var result = await itemDAO.Upsert(item);
        return result.Match(
            porsi => Ok(item),
            oporno => Problem());
    }
    // WHEN THE APP HAS ITS AUTH 
    // THIS IS GONNA CHECK IF THE 
    // ITEM IS FROM THE USERS PURCHASE
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteItem(long id){
        var result = await cartService.removeItemFromOrder(id);
        return Ok(result.Value);
    }
}
