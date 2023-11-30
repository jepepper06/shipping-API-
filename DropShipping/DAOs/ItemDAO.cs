namespace DropShipping.DAOs;

using DropShipping.Models;
using DropShipping.Data;
using System.Threading.Tasks;
using ErrorOr;
using System.Collections.Generic;
using Errors;
using Microsoft.EntityFrameworkCore;
public class ItemDAO : ItemDAOInterface
{
    private readonly DropshippingContext context;
    public ItemDAO(DropshippingContext _context){
        context = _context;
    }
    private List<Error> errors =new();

    public async Task<ErrorOr<bool>> Delete(long id)
    {
        try{
            var itemToRemove = await context.Items.SingleAsync(i => i.Id == id);
            if(itemToRemove == null)
            {
                errors.Add(Errors.ItemDAO.ItemNotFound);
                return errors;
            }
            context.Items.Remove(itemToRemove);
            await context.SaveChangesAsync();
            return true;
        }catch{
            errors.Add(Error.Unexpected(
                    code:"Error.InternalServerError",
                    description:"Internal Server Error Has Ocurred!"));
                return errors;
        }
    }

    public async Task<ErrorOr<List<Item>>> GetAll(int pageNumber)
    {
        var items = await  context.Items
            .Skip(pageNumber -1)
            .Take(100)
            .OrderBy(i => i.CreatedAt)
            .ToListAsync();
        if(items == null){
            errors.Add(Errors.ItemDAO.ItemNotFound);
            return errors;
        }
        return items;
    }

    public async Task<ErrorOr<Item>> GetById(long id)
    {
        var item = await context.Items
            .FindAsync(id);
        if(item == null){
            errors.Add(Errors.ItemDAO.ItemNotFound);
            return errors;
        }
        return item;
    }

    public async Task<ErrorOr<bool>> Save(Item item)
    {
        await context.Items.AddAsync(item);
        try{
            await context.SaveChangesAsync();
            return true;
        }catch(DbUpdateException e){
            e = new DbUpdateException("An Error Has Ocurred Item Cannot be saved");
            errors.Add(Error.Unexpected(code: "Error.Item.NotSaved", description: e.Message));
            System.Console.WriteLine($"An exception while Updating Item: {e.ToString}");
            return errors;
        }catch(Exception e){
            e = new Exception("Unexpected Exception Has Ocurred");
            errors.Add(Error.Unexpected(code: "Error.Item.NotSaved", description:e.Message));
            System.Console.WriteLine($"An exception while Updating City: {e.ToString}");
            return errors;
        }
    }

    public async Task<ErrorOr<Item>> Upsert(Item item)
    {
        context.Items.Update(item);
        await context.SaveChangesAsync();
        return item;
    }
}