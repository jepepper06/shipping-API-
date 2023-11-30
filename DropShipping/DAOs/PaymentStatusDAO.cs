namespace DropShipping.DAOs;

using DropShipping.Models;
using DropShipping.Data;
using System.Threading.Tasks;
using ErrorOr;
using System.Collections.Generic;
using Errors;
using Microsoft.EntityFrameworkCore;

public class PaymentStatusDAO : PaymentStatusDAOInterface
{
    private readonly DropshippingContext context;
    public PaymentStatusDAO(DropshippingContext _context){
        context = _context;
    }
    private List<Error> errors = new();

    public async Task<ErrorOr<bool>> Delete(long id)
    {
        try
        {
            var paymentStatusToRemove = (PaymentStatus)context.Payments.Where(ps => ps.Id == id);
            if (paymentStatusToRemove == null)
            {
                errors.Add(Errors.PaymentStatusDAO.PaymentNotFound);
                return errors;
            }
            context.Payments.Remove(paymentStatusToRemove);
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

    public async Task<ErrorOr<List<PaymentStatus>>> GetAll(int pageNumber)
    {
        var paymentStatuses = await context.Payments
            .Skip(pageNumber - 1)
            .Take(100)
            .OrderBy(p => p.CreatedAt)
            .ToListAsync();
        if (paymentStatuses == null)
        {
            errors.Add(Errors.PaymentStatusDAO.PaymentNotFound);
            return errors;
        }
        return paymentStatuses;
    }

    public async Task<ErrorOr<PaymentStatus>> GetById(long id)
    {
        var paymentStatus = await context.Payments.FindAsync(id);
        if (paymentStatus == null)
        {
            errors.Add(Errors.PaymentStatusDAO.PaymentNotFound);
            return errors;
        }
        return paymentStatus;
    }

    public async Task<ErrorOr<bool>> Save(PaymentStatus paymentStatus)
    {
        await context.Payments.AddAsync(paymentStatus);
        try{
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException e){
            e = new DbUpdateException("An Error Has Ocurred PaymentStatus Cannot be saved");
            errors.Add(Error.Unexpected(code: "Error.PaymentStatus.NotSaved", description: e.Message));
            System.Console.WriteLine($"An exception while Updating PaymentStatus: {e.ToString}");
            return errors;
        }
        catch (Exception e){
            e = new Exception("Unexpected Exception Has Ocurred");
            errors.Add(Error.Unexpected(code: "Error.PaymentStatus.NotSaved", description:e.Message));
            System.Console.WriteLine($"An exception while Updating PaymentStatus: {e.ToString}");
            return errors;
        }
    }

    public async Task<ErrorOr<PaymentStatus>> Upsert(PaymentStatus paymentStatus)
    {
        context.Payments.Update(paymentStatus);
        await context.SaveChangesAsync();
        return paymentStatus;
    }
}
