namespace DropShipping.DAOs;

using DropShipping.Data;
using ErrorOr;
using DropShipping.Errors;
using DropShipping.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DropShipping.DTOs;

public class OrderDAO : OrderDAOInterface
{
    private readonly DropshippingContext _context;
    public OrderDAO(DropshippingContext context){
        _context = context;
    }
    private List<Error> errors = new();
    public async Task<ErrorOr<Order>> GetById(long id){
        var order = await _context.Orders
            .Include(o => o.PaymentStatus)
            .SingleAsync(o => o.Id == id);
        if(order == null){
            errors.Add(Errors.OrderDAO.OrderNotFound);
            return errors;
        }
        return order;
    }
    public async Task<ErrorOr<bool>> Save(Order order){
        await _context.Orders.AddAsync(order);
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
    public async Task<ErrorOr<Order>> Upsert(Order order){
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
        return order;
    }
    public async Task<ErrorOr<List<Order>>> GetAll(int pageNumber){
        
        var orders = await _context.Orders
            .Include(o => o.PaymentStatus)
            .AsNoTracking()
            .Include(o => o.Items)
            .AsNoTracking()
            .Include(o => o.Transport)
            .Skip(pageNumber - 1)
            .Take(100)
            .OrderBy(o => o.CreatedAt)
            .ToListAsync();
        if(orders.Count == 0){
            errors.Add(Errors.OrderDAO.OrderNotFound);
            return errors;
        }
        return orders;
    }
    public async Task<ErrorOr<bool>> Delete(long id){
        
            var orderToRemove = await _context.Orders.SingleAsync(o => o.Id == id);
            _context.Orders.Remove(orderToRemove);
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
    public async Task<ErrorOr<Order?>> GetFirstByUserId(long userId){
        var order = await _context.Orders
            .Include(o => o.PaymentStatus)
            .Include(o => o.User)
            .Include(o => o.Items!)
            .ThenInclude(i => i.Product)
            .Where(o => o.User!.Id == userId && o.PaymentStatus!.Payed == false)
            .OrderBy(o => o.CreatedAt)
            .FirstOrDefaultAsync();
        return order;
    }

    public async Task<ErrorOr<List<Order>>> GetOrderPopulated(uint pageNumber){
        List<OrderManagerResponseDTO> orderManagerResponseDTOs = new();
        var orders = await _context.Orders
            .Include(o => o.PaymentStatus)
            .Include(o => o.Items)
            .Include(o => o.User)
            .ThenInclude(u => u!.UserData)
            .Skip((int)pageNumber - 1)
            .Take(100)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
        if(orders.Count == 0){
            errors.Add(Errors.OrderDAO.OrderNotFound);
            return errors;
        }

        return orders;
    }

    public async Task<ErrorOr<List<Order>>> GetAllOrdersWithNullShipmentState()
    {
        try{
            var orders = await _context.Orders            
            .Where(o => o.ShipmentStateId == null)
            .OrderBy(o => o.CreatedAt)
            .ToListAsync();
            return orders;
        }catch(DbUpdateException e){
            e = new DbUpdateException("There was an Error In Database while searching to orders with no Shipment");
            errors.Add(Error.Unexpected("Error.AllOrdersWithNullShipmentId"));
            Console.WriteLine(e.ToString());
            return errors;
        }   
    }
    public async Task<ErrorOr<Order>> GetOrderByItemId(long itemId)
    {
        try{   
            var order = await _context.Orders
                .Include(o => o.PaymentStatus)
                .Include(o => o.User)
                .Include(o => o.Items!)
                .ThenInclude(i => i.Product)
                .Where(o => o.Items!.Single(i => i.Id == itemId).Id == itemId && o.PaymentStatus!.Payed == false)
                .OrderBy(o => o.CreatedAt)
                .FirstAsync();

            return order;
        }catch(DbUpdateException e){
            e = new DbUpdateException("There was an Error while looking for this order, Maybe it doesn't exist");
            errors.Add(Error.NotFound(e.Message));
            Console.WriteLine(e.ToString());
            return errors;
        }
    }
}