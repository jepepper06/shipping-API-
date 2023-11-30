namespace DropShipping.DAOs;

using DropShipping.Models;
using DropShipping.Data;
using System.Threading.Tasks;
using ErrorOr;
using System.Collections.Generic;
using Errors;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class RoleDAO : RoleDAOInterface
{
    private readonly DropshippingContext context;
    public RoleDAO(DropshippingContext _context){
        context = _context;
    }
    private List<Error> errors = new();
    
    public async Task<ErrorOr<bool>> Delete(long id)
    {
        try
        {
            var roleToRemove = (Role)context.Roles.Where(r => r.Id == id);
            if (roleToRemove == null)
            {
                errors.Add(Errors.RoleDAO.RoleNotFound);
                return errors;
            }
            context.Roles.Remove(roleToRemove);
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

    public async Task<ErrorOr<List<Role>>> GetAll(int pageNumber)
    {
        var roles = await context.Roles.Skip(pageNumber - 1).Take(100).ToListAsync();
        if (roles == null)
        {
            errors.Add(Errors.RoleDAO.RoleNotFound);
            return errors;
        }
        return roles;
    }

    public async Task<ErrorOr<Role>> GetById(long id)
    {
        var role = await context.Roles.FindAsync(id);
        if (role == null)
        {
            errors.Add(Errors.RoleDAO.RoleNotFound);
            return errors;
        }
        return role;
    }

    public async Task<ErrorOr<bool>> Save(Role role)
    {
        await context.Roles.AddAsync(role);
        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException e)
        {
            e = new DbUpdateException("An Error Has Ocurred Role Cannot be saved");
            errors.Add(Error.Unexpected(code: "Error.Role.NotSaved", description: e.Message));
            System.Console.WriteLine($"An exception while Updating Role: {e.ToString}");
            return errors;
        }
        catch (Exception e)
        {
            e = new Exception("Unexpected Exception Has Ocurred");
            errors.Add(Error.Unexpected(code: "Error.Role.NotSaved", description:e.Message));
            System.Console.WriteLine($"An exception while Updating Role: {e.ToString}");
            return errors;
        }    
    }

    public async Task<ErrorOr<Role>> Upsert(Role role)
    {
        context.Roles.Update(role);
        await context.SaveChangesAsync();
        return role;
    }
}
