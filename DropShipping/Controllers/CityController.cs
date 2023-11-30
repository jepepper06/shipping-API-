namespace DropShipping.Controllers;

using DropShipping.DAOs;
using Microsoft.AspNetCore.Mvc;
using DropShipping.Contracts;
using DropShipping.Models;
using ErrorOr;

[Route("cities")]
[ApiController]
public class CityController : Controller
{
    private readonly CityDAO cityDAO;

    public CityController(CityDAO _cityDAO){
        cityDAO = _cityDAO;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCity(CityRequest request){
        // TODO VALIDATION
        City city = new City();
        city.Name = request.Name;
        city.Description = request.Description;
        city.Create();
        var result = await cityDAO.Save(city);
        return result.Match(
            city => Ok(city), 
            errors => Problem(
                result.FirstError.Description,
                null,
                StatusCodes.Status400BadRequest,
                "Error on Creating City",
                result.FirstError.Code));
    }

    [HttpGet("all/{page:int}")]
    public async Task<IActionResult> AllCities(int page){
        var result = await cityDAO.GetAll(page);
        return result.Match(
            cities => Ok(cities),
            errors => Problem(result.FirstError.Description,null,StatusCodes.Status500InternalServerError,result.FirstError.Code,null));
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetCityById( long id){
        ErrorOr<City> result = await cityDAO.GetById(id);
        Error error = result.FirstError;
        return result.Match(
            city => Ok(result.Value),
            errors => Problem(
                error.Description,
                null,
                StatusCodes.Status404NotFound,
                error.Code,
                null
            ));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateCity(long id, CityRequest request){
        City city = new City();
        city.Id = id;
        city.Name = request.Name;
        city.Description = request.Description;
        city.Update();
        var result = await cityDAO.Upsert(city);
        return result.Match(
            porsi => Ok(city),
            oporno => Problem());
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteCity(long id){
        ErrorOr<bool> result =  await cityDAO.Delete(id);
        if(result.IsError){
            return Problem(detail:result.FirstError.Description,statusCode:StatusCodes.Status500InternalServerError,title:result.FirstError.Code);
        }
        return Ok(result);
    }
}
