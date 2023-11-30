namespace DropShipping.DAOs;

using DropShipping.Data;
using DropShipping.Models;
using ErrorOr;
using DropShipping.Errors;
using Microsoft.EntityFrameworkCore;

public class ProductDAO : ProductDAOInterface
{
    private readonly DropshippingContext _context;
    public ProductDAO(DropshippingContext context){
        _context = context;
    }

    private List<Error> errors = new();

    public async Task<ErrorOr<Product>> GetById(long id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            errors.Add(Errors.ProductDAO.ProductNotFound);
            return errors;
        }
        return product;
    }

    public async Task<ErrorOr<List<Product>>> GetAll(int pageNumber)
    {
        var products = await _context.Products
            .Skip(pageNumber - 1)
            .Take(100)
            .OrderBy(p => p.Id)
            .ToListAsync();
            
        if (products.Count == 0)
        {
            errors.Add(Errors.ProductDAO.ProductNotFound);
            return errors;
        }
        return products;
    }

    public async Task<ErrorOr<bool>> Save(Product product)
    {
        await _context.Products.AddAsync(product);
        try{
            await _context.SaveChangesAsync();
            return true;
        }catch(DbUpdateException e){
            e = new DbUpdateException("An Error Has Ocurred Product Cannot be saved");
            errors.Add(Error.Unexpected(code: "Error.Product.NotSaved", description:e.Message));
            System.Console.WriteLine($"An exception while Updating Product: {e.ToString}");
            return errors;
        }catch(Exception e){
            e = new Exception("Unexpected Exception Has Ocurred");
            errors.Add(Error.Unexpected(code: "Error.Product.NotSaved", description:e.Message));
            System.Console.WriteLine($"An exception while Updating Product: {e.ToString}");
            return errors;
        }
    }

    public async Task<ErrorOr<Product>> Upsert(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<ErrorOr<bool>> Delete(long id)
    {
        var productToRemove = (Product) _context.Products.Where(o => o.Id == id);
        _context.Products.Remove(productToRemove);
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException e)
        {
            e = new DbUpdateException("Internal Server Error");
            errors.Add(
                Error.Unexpected(code: "Error.InternalServerError",
                description:$"{e.Message}")
            );
            var exceptionString = e.ToString();
            System.Console.WriteLine($"An Exception updating the DB: ${exceptionString} ");
            return errors;
        }
        catch (Exception e)
        {
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