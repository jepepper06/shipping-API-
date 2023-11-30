namespace DropShipping.DAOs;

using DropShipping.Models;
using DropShipping.Data;
using System.Threading.Tasks;
using ErrorOr;
using System.Collections.Generic;
using Errors;
using Microsoft.EntityFrameworkCore;
public class OfficeDAO : OfficeDAOInterface
{
    private DropshippingContext _context;
    public OfficeDAO(DropshippingContext context){
        _context = context;
    }
    private List<Error> errors = new();

    public async Task<ErrorOr<bool>> Delete(long id)
    {
        try
        {
            var officeToRemove = (Office)_context.Offices.Where(o => o.Id == id);
            if (officeToRemove == null)
            {
                errors.Add(Errors.OfficeDAO.OfficeNotFound);
                return errors;
            }
            _context.Offices.Remove(officeToRemove);
            await _context.SaveChangesAsync();
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

    public async Task<ErrorOr<List<Office>>> GetAll(int pageNumber)
    {
        var offices = await _context.Offices
            .Include(o => o.City)
            .Skip(pageNumber - 1)
            .Take(100)
            .OrderBy(o => o.Id)
            .ToListAsync();
        if (offices == null)
        {
            errors.Add(Errors.OfficeDAO.OfficeNotFound);
            return errors;
        }
        return offices;
    }

    public async Task<ErrorOr<Office>> GetById(long id)
    {
        var office = await _context.Offices.FindAsync(id);
        if (office == null)
        {
            errors.Add(Errors.OfficeDAO.OfficeNotFound);
            return errors;
        }
        return office;
    }

    public async Task<ErrorOr<bool>> Save(Office office)
    {
        await _context.Offices.AddAsync(office);
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException e)
        {
            e = new DbUpdateException("An Error Has Ocurred Office Cannot be saved");
            errors.Add(Error.Unexpected(code: "Error.Office.NotSaved", description: e.Message));
            System.Console.WriteLine($"An exception while Updating Office: {e.ToString}");
            return errors;
        }
    }

    public async Task<ErrorOr<Office>> Upsert(Office office)
    {
        _context.Offices.Update(office);
        await _context.SaveChangesAsync();
        return office;
    }
}
