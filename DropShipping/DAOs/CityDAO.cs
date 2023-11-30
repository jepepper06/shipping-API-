namespace DropShipping.DAOs;

using DropShipping.Models;
using DropShipping.Data;
using System.Threading.Tasks;
using ErrorOr;
using System.Collections.Generic;
using Errors;
using Microsoft.EntityFrameworkCore;
// using Errors;

public class CityDAO : CityDAOInterface
{
    private readonly DropshippingContext context;
    public CityDAO(DropshippingContext _context){
        context = _context;
    }
    private List<Error> errors = new();

    public async Task<ErrorOr<bool>> Delete(long id)
    {
        try
        {
            var cityToRemove = (City) context.Cities.Where(c => c.Id == id);
            if (cityToRemove == null)
            {
                errors.Add(Errors.CityDAO.CityNotFound);
                return errors;
            }
            context.Cities.Remove(cityToRemove);
            await context.SaveChangesAsync();
            return true;
        }
        catch
        {
            errors.Add(Error.Unexpected(
                code: "Error.InternalServerError",
                description: "Internal Server Error Has Ocurred!"
            ));
            return errors;
        }
    }

    public async Task<ErrorOr<List<City>>> GetAll(int pageNumber)
    {
        var cities = await context.Cities
            .Skip(pageNumber - 1)
            .Take(100)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
        if (cities == null)
        {
            errors.Add(Errors.CityDAO.CityNotFound);
            return errors;
        }
        return cities;
    }

    public async Task<ErrorOr<City>> GetById(long id)
    {
        var city = await context.Cities.FindAsync(id);
        if (city == null)
        {
            errors.Add(Errors.CityDAO.CityNotFound);
            return errors;
        }
        return city;
    }

    public async Task<ErrorOr<bool>> Save(City city)
    {
        await context.Cities.AddAsync(city);
        try{
            await context.SaveChangesAsync();
            return true;
        }catch(DbUpdateException e){
            e = new DbUpdateException("An Error Has Ocurred City Cannot be saved");
            errors.Add(Error.Unexpected(code: "Error.City.NotSaved", description: e.Message));
            System.Console.WriteLine($"An exception while Updating City: {e.ToString}");
            return errors;
        }
        
    }

    public async Task<ErrorOr<City>> Upsert(City city)
    {
        context.Cities.Update(city);
        await context.SaveChangesAsync();
        return city;
    }
}
