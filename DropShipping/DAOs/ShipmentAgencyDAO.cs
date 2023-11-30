namespace DropShipping.DAOs;

using DropShipping.Models;
using DropShipping.Data;
using System.Threading.Tasks;
using ErrorOr;
using System.Collections.Generic;
using Errors;
using Microsoft.EntityFrameworkCore;

public class ShipmentAgencyDAO : ShipmentAgencyDAOInterface
{
    private readonly DropshippingContext context;
    public ShipmentAgencyDAO(DropshippingContext _context){
        context = _context;
    }
    private List<Error> errors = new();

    public async Task<ErrorOr<bool>> Delete(long id)
    {
        try
        {
            var shipmentAgencyToRemove = (ShipmentAgency)context.ShipmentAgencies.Where(sa => sa.Id == id);
            if (shipmentAgencyToRemove == null)
            {
                errors.Add(Errors.ShipmentAgencyDAO.ShipmentAgencyNotFound);
                return errors;
            }
            context.ShipmentAgencies.Remove(shipmentAgencyToRemove);
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

    public async Task<ErrorOr<List<ShipmentAgency>>> GetAll(int pageNumber)
    {
        var shipmentAgencies = await context.ShipmentAgencies
            .Skip(pageNumber - 1)
            .Take(100)
            .OrderBy(sa => sa.Id)
            .ToListAsync();
        if (shipmentAgencies == null)
        {
            errors.Add(Errors.ShipmentAgencyDAO.ShipmentAgencyNotFound);
            return errors;
        }
        return shipmentAgencies;
    }

    public async Task<ErrorOr<ShipmentAgency>> GetById(long id)
    {
        var shipmentAgency = await context.ShipmentAgencies.FindAsync(id);
        if (shipmentAgency == null)
        {
            errors.Add(Errors.ShipmentAgencyDAO.ShipmentAgencyNotFound);
            return errors;
        }
        return shipmentAgency;
    }

    public async Task<ErrorOr<bool>> Save(ShipmentAgency shipmentAgency)
    {
        await context.ShipmentAgencies.AddAsync(shipmentAgency);
        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException e)
        {
            e = new DbUpdateException("An Error Has Ocurred ShipmentAgency Cannot be saved");
            errors.Add(Error.Unexpected(code: "Error.ShipmentAgency.NotSaved", description: e.Message));
            System.Console.WriteLine($"An exception while Updating ShipmentAgency: {e.ToString}");
            return errors;
        }
        catch (Exception e)
        {
            e = new Exception("Unexpected Exception Has Ocurred");
            errors.Add(Error.Unexpected(code: "Error.ShipmentAgency.NotSaved", description:e.Message));
            System.Console.WriteLine($"An exception while Updating ShipmentAgency: {e.ToString}");
            return errors;
        }    
    }

    public async Task<ErrorOr<ShipmentAgency>> Upsert(ShipmentAgency shipmentAgency)
    {
        context.ShipmentAgencies.Update(shipmentAgency);
        await context.SaveChangesAsync();
        return shipmentAgency;
    }
}
