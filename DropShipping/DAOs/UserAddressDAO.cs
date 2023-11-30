namespace DropShipping.DAOs;

using DropShipping.Models;
using DropShipping.Data;
using System.Threading.Tasks;
using ErrorOr;
using System.Collections.Generic;
using Errors;
using Microsoft.EntityFrameworkCore;

public class UserAddressDAO : UserAddressDAOInterface
{
    private readonly DropshippingContext context;
    public UserAddressDAO(DropshippingContext _context){
        context = _context;
    }
    private List<Error> errors =new();

    public async Task<ErrorOr<bool>> Delete(long id)
    {
        try{
            var userAddressToRemove = (UserAddress) context.UsersAddress.Where(i => i.Id == id);
            if(userAddressToRemove == null)
            {
                errors.Add(Errors.UserAddressDAO.UserAddressNotFound);
                return errors;
            }
            context.UsersAddress.Remove(userAddressToRemove);
            await context.SaveChangesAsync();
            return true;
        }catch{
            errors.Add(Error.Unexpected(
                    code:"Error.InternalServerError",
                    description:"Internal Server Error Has Ocurred!"));
                return errors;
        }
    }

    public async Task<ErrorOr<List<UserAddress>>> GetAll(int pageNumber)
    {
        var userAddresses = await  context.UsersAddress
            .Skip(pageNumber -1)
            .Take(100)
            .OrderBy(ua => ua.Id)
            .ToListAsync();
        if(userAddresses == null){
            errors.Add(Errors.UserAddressDAO.UserAddressNotFound);
            return errors;
        }
        return userAddresses;
    }

    public async Task<ErrorOr<UserAddress>> GetById(long id)
    {
        var userAddress = await context.UsersAddress.FindAsync(id);
        if(userAddress == null){
            errors.Add(Errors.UserAddressDAO.UserAddressNotFound);
            return errors;
        }
        return userAddress;
    }

    public async Task<ErrorOr<bool>> Save(UserAddress userAddress)
    {
        await context.UsersAddress.AddAsync(userAddress);
        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException e)
        {
            e = new DbUpdateException("An Error Has Ocurred UserAddress Cannot be saved");
            errors.Add(Error.Unexpected(code: "Error.UserAddress.NotSaved", description: e.Message));
            System.Console.WriteLine($"An exception while Updating UserAddress: {e.ToString}");
            return errors;
        }
        catch (Exception e)
        {
            e = new Exception("Unexpected Exception Has Ocurred");
            errors.Add(Error.Unexpected(code: "Error.UserAddress.NotSaved", description:e.Message));
            System.Console.WriteLine($"An exception while Updating UserAddress: {e.ToString}");
            return errors;
        }
    }

    public async Task<ErrorOr<UserAddress>> Upsert(UserAddress userAddress)
    {
        context.UsersAddress.Update(userAddress);
        await context.SaveChangesAsync();
        return userAddress;
    }
}
