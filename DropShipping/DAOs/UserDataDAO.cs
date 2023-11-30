namespace DropShipping.DAOs;

using DropShipping.Models;
using DropShipping.Data;
using System.Threading.Tasks;
using ErrorOr;
using System.Collections.Generic;
using Errors;
using Microsoft.EntityFrameworkCore;

public class UserDataDAO : UserDataDAOInterface
{
    private DropshippingContext _context;
    public UserDataDAO(DropshippingContext context){
        _context = context;
    }
    private List<Error> errors =new();

    public async Task<ErrorOr<bool>> Delete(long id)
    {
        try{
            var userDataToRemove = (UserData) _context.UsersData.Where(i => i.Id == id);
            if(userDataToRemove == null)
            {
                errors.Add(Errors.UserDataDAO.UserDataNotFound);
                return errors;
            }
            _context.UsersData.Remove(userDataToRemove);
            await _context.SaveChangesAsync();
            return true;
        }catch{
            errors.Add(Error.Unexpected(
                    code:"Error.InternalServerError",
                    description:"Internal Server Error Has Ocurred!"));
                return errors;
        }
    }

    public async Task<ErrorOr<List<UserData>>> GetAll(int pageNumber)
    {
        var userData = await  _context.UsersData
            .Skip(pageNumber -1)
            .Take(100)
            .OrderBy(ud => ud.Id)
            .ToListAsync();
        if(userData == null){
            errors.Add(Errors.UserDataDAO.UserDataNotFound);
            return errors;
        }
        return userData;
    }

    public async Task<ErrorOr<UserData>> GetById(long id)
    {
        var userData = await _context.UsersData.FindAsync(id);
        if(userData == null){
            errors.Add(Errors.UserDataDAO.UserDataNotFound);
            return errors;
        }
        return userData;
    }

    public async Task<ErrorOr<bool>> Save(UserData userData)
    {
        await _context.UsersData.AddAsync(userData);
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException e)
        {
            e = new DbUpdateException("An Error Has Ocurred UserData Cannot be saved");
            errors.Add(Error.Unexpected(code: "Error.UserData.NotSaved", description: e.Message));
            System.Console.WriteLine($"An exception while Updating UserData: {e.ToString}");
            return errors;
        }
        catch (Exception e)
        {
            e = new Exception("Unexpected Exception Has Ocurred");
            errors.Add(Error.Unexpected(code: "Error.UserData.NotSaved", description:e.Message));
            System.Console.WriteLine($"An exception while Updating UserData: {e.ToString}");
            return errors;
        }    
    }
    public async Task<ErrorOr<UserData>> Upsert(UserData userData)
    {
        _context.UsersData.Update(userData);
        await _context.SaveChangesAsync();
        return userData;
    }
    public async Task<bool> IsEmailRegistered(string Email){
        var AlgoVar =  await _context.UsersData.AnyAsync(ud => ud.Email == Email);
        return AlgoVar;
    }
    public async Task<ErrorOr<UserData>> GetUserDataByUserId(long userId){
        var userData = await _context.UsersData.SingleAsync(ud => ud.UserId == userId);
        return userData;
    }
}
