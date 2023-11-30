namespace DropShipping.DAOs;

using DropShipping.Models;
using DropShipping.Data;
using System.Threading.Tasks;
using ErrorOr;
using System.Collections.Generic;
using Errors;
using Microsoft.EntityFrameworkCore;

public class TransportDAO : TransportDAOInterface
{
    private readonly DropshippingContext _context;
    public TransportDAO(DropshippingContext context){
        _context = context;
    }
    private List<Error> errors = new();

    public async Task<ErrorOr<bool>> Delete(long id)
    {
        try
        {
            var transportToRemove = (Transport) _context.Transports.Where(t => t.Id == id);
            if (transportToRemove == null)
            {
                errors.Add(Errors.TransportDAO.TransportNotFound);
                return errors;
            }
            _context.Transports.Remove(transportToRemove);
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

    public async Task<ErrorOr<List<Transport>>> GetAll(int pageNumber)
    {
        var transports = await _context.Transports
            .Skip(pageNumber - 1)
            .Take(100)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();
            
        if (transports == null)
        {
            errors.Add(Errors.TransportDAO.TransportNotFound);
            return errors;
        }
        return transports;
    }

    public async Task<ErrorOr<Transport>> GetById(long id)
    {
        var transport = await _context.Transports.FindAsync(id);
        if (transport == null)
        {
            errors.Add(Errors.TransportDAO.TransportNotFound);
            return errors;
        }
        return transport;
    }

    public async Task<ErrorOr<bool>> Save(Transport transport)
    {
        await _context.Transports.AddAsync(transport);
        try{
            await _context.SaveChangesAsync();
            return true;
        }catch(DbUpdateException e){
            e = new DbUpdateException("An Error Has Ocurred Transport Cannot be saved");
            errors.Add(Error.Unexpected(code: "Error.Transport.NotSaved", description: e.Message));
            System.Console.WriteLine($"An exception while Updating Transport: {e.ToString}");
            return errors;
        }
        
    }

    public async Task<ErrorOr<Transport>> Upsert(Transport transport)
    {
        _context.Transports.Update(transport);
        await _context.SaveChangesAsync();
        return transport;
    }
}
