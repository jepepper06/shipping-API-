namespace DropShipping.DAOs;

using DropShipping.Data;
using DropShipping.Models;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using DropShipping.Errors;

public class UserDAO : UserDAOInterface
{
    private readonly DropshippingContext _context;
    private List<Error> errors = new();
    public UserDAO(DropshippingContext context){
        _context = context;
    }

    public async Task<ErrorOr<User>> GetById(long id)
    {
        var user = await _context.Users.Include(u => u.UserRoles).SingleAsync(u => u.Id == id);
        if (user == null)
        {
            errors.Add(Errors.UserDAO.UserNotFound);
            return errors;
        }
        return user;
    }
    public async Task<ErrorOr<User>> GetUserByUserNameOrEmail(string userName){
        var user = await _context.Users
            .Include(u => u.UserData)
            .SingleAsync(u => u.UserData!.Name == userName || u.UserData.Email == userName);
        if(user == null)
        {
            errors.Add(Errors.UserDAO.UserNotFound);
            return errors;
        }
        return user;
    }

    public async Task<ErrorOr<List<User>>> GetAll(int pageNumber)
    {
        var users = await _context.Users
            .Skip(pageNumber - 1)
            .Take(100)
            .OrderBy(u => u.Id)
            .ToListAsync();
        if (users.Count == 0)
        {
            errors.Add(Errors.UserDAO.UserNotFound);
            return errors;
        }
        return users;
    }

    public async Task<ErrorOr<bool>> Save(User user)
    {
        await _context.Users.AddAsync(user);
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException e)
        {
            e = new DbUpdateException("An Error Has Ocurred User Cannot be saved");
            errors.Add(Error.Unexpected(code: "Error.User.NotSaved", description: e.Message));
            System.Console.WriteLine($"An exception while Updating User: {e.ToString}");
            return errors;
        }
        catch (Exception e)
        {
            e = new Exception("Unexpected Exception Has Ocurred");
            errors.Add(Error.Unexpected(code: "Error.User.NotSaved", description:e.Message));
            System.Console.WriteLine($"An exception while Updating User: {e.ToString}");
            return errors;
        }
    }

    public async Task<ErrorOr<User>> Upsert(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<ErrorOr<bool>> Delete(long id)
    {
        var userToRemove = await _context.Users.FindAsync(id);
        if(userToRemove == null){
            errors.Add(Errors.UserDAO.UserNotFound);
            return errors;
        }
        _context.Users.Remove(userToRemove);
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
