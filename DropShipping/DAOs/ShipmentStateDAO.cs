namespace DropShipping.DAOs;

using DropShipping.Data;
using ErrorOr;
using DropShipping.Errors;
using DropShipping.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class ShipmentStateDAO : ShipmentStateDAOInterface
{
    private readonly DropshippingContext _context;
    public ShipmentStateDAO(DropshippingContext context){
        _context = context;
    }
    private List<Error> errors = new();
    public async Task<ErrorOr<ShipmentState>> GetById(long id){
        var shipmentState = await _context.ShipmentStates
            .Include(o => o.ShipmentAgency)
            .SingleAsync(o => o.Id == id);
        if(shipmentState == null){
            errors.Add(Errors.ShipmentStatusDAO.ShipmentStateNotFound);
            return errors;
        }
        return shipmentState;
    }
    public async Task<ErrorOr<bool>> Save(ShipmentState shipmentState){ 
        await _context.ShipmentStates.AddAsync(shipmentState);
        try{
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException e){
            e = new DbUpdateException("An Error Has Ocurred Order Cannot be saved");
            errors.Add(Error.Unexpected(code: "Error.Order.NotSaved", description: e.Message));
            System.Console.WriteLine($"An exception while Updating Order: {e.ToString}");
            return errors;
        }
        catch (Exception e){
            e = new Exception("Unexpected Exception Has Ocurred");
            errors.Add(Error.Unexpected(code: "Error.Order.NotSaved", description:e.Message));
            System.Console.WriteLine($"An exception while Updating Order: {e.ToString}");
            return errors;
        }    
    }
    public async Task<ErrorOr<ShipmentState>> Upsert(ShipmentState shipmentState){
        _context.ShipmentStates.Update(shipmentState);
        await _context.SaveChangesAsync();
        return shipmentState;
    }
    public async Task<ErrorOr<List<ShipmentState>>> GetAll(int pageNumber){
        var shipmentStates = await _context.ShipmentStates
            .Include(ss => ss.ShipmentAgency)
            .Skip(pageNumber - 1)
            .Take(100)
            .OrderBy(ss => ss.CreatedAt)
            .ToListAsync();
            
        if(shipmentStates.Count is 0){
            errors.Add(Errors.ShipmentStatusDAO.ShipmentStateNotFound);
            return errors;
        }
        return shipmentStates;
    }
    public async Task<ErrorOr<bool>> Delete(long id){
        
            var shipmentStateToRemove = await _context.ShipmentStates.SingleAsync(o => o.Id == id);
            _context.ShipmentStates.Remove(shipmentStateToRemove);
        try{
            await _context.SaveChangesAsync();
            return true;
        }catch(DbUpdateException e){
            e = new DbUpdateException("Internal Server Error");
            errors.Add(
                Error.Unexpected(code: "Error.InternalServerError",
                description:$"{e.Message}")
            );
            var exceptionString = e.ToString();
            System.Console.WriteLine($"An Exception updating the DB: ${exceptionString} ");
            return errors;
        }catch(Exception e){
            e = new Exception("Internal Server Error Has Ocurred!");
            errors.Add(
                Error.Unexpected(
                    code:"Error.InternalServerError",
                    description:e.Message));
                var exceptionString = e.ToString();
                System.Console.WriteLine($"An Exception has ocurred: ${exceptionString}");
            return errors;
        }
    }
}
