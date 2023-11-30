
namespace DropShipping.Controllers;

using DropShipping.DAOs;
using Microsoft.AspNetCore.Mvc;
using DropShipping.Contracts;
using DropShipping.Models;
using System;
using ErrorOr;

[Route("products")]
[ApiController]
public class ProductController : Controller
{
    private readonly ProductDAO productDAO;

    public ProductController(ProductDAO _productDAO){
        productDAO = _productDAO;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateProduct(ProductRequest request){
        // TODO VALIDATION
        Product product = new Product();
        product.Create();
        product.Name = request.Name;
        product.Price = request.Price;
        var result = await productDAO.Save(product);
        return result.Match(
            product => Ok(product), 
            errors => Problem(
                result.FirstError.Description,
                null,
                StatusCodes.Status400BadRequest,
                "Error on Creating Product",
                result.FirstError.Code));
    }

    [HttpGet("all/{page:int}")]
    public async Task<IActionResult> AllProducts(int page){
        var result = await productDAO.GetAll(page);
        return result.Match(
            products => Ok(products),
            errors => Problem(result.FirstError.Description,null,StatusCodes.Status500InternalServerError,result.FirstError.Code,null));
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById( long id){
        ErrorOr<Product> result = await productDAO.GetById(id);
        Error error = result.FirstError;
        return result.Match(
            product => Ok(result),
            errors => Problem(
                error.Description,
                null,
                StatusCodes.Status404NotFound,
                error.Code,
                null
            ));
    }
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, ProductRequest request){
        Product p = new Product
        {
            Name = request.Name,
            Price = request.Price,
            UpdatedAt = DateTime.UtcNow
        };
        var result = await productDAO.Upsert(p);
        return result.Match(
            porsi => Ok(p),
            oporno => Problem());
    }
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id){
        ErrorOr<bool> result =  await productDAO.Delete(id);
        if(result.IsError){
            return Problem(detail:result.FirstError.Description,statusCode:StatusCodes.Status500InternalServerError,title:result.FirstError.Code);
        }
        return Ok(result);
    }

    [HttpPost("link/{id:long}")]
    public async Task<IActionResult> AddLinkToProduct(long id){
        await Task.Delay(1000);
        return Ok("Putas Harry Putas");
    }
}