namespace DropShipping.Controllers;

using DropShipping.DAOs;
using Microsoft.AspNetCore.Mvc;
using DropShipping.Contracts;
using DropShipping.Models;
using System;
using ErrorOr;

[Route("paymentstatuses")]
[ApiController]
public class PaymentStatusController : Controller
{
    private readonly PaymentStatusDAO paymentStatusDAO;

    public PaymentStatusController(PaymentStatusDAO _paymentStatusDAO){
        paymentStatusDAO = _paymentStatusDAO;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePaymentStatus(PaymentStatusRequest request)
    {
        // TODO VALIDATION
        PaymentStatus paymentStatus = new PaymentStatus
        {
            OrderId = request.OrderId,
            Payed = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await paymentStatusDAO.Save(paymentStatus);
        return result.Match(
            paymentStatus => Ok(paymentStatus), 
            errors => Problem(
                result.FirstError.Description,
                null,
                StatusCodes.Status400BadRequest,
                "Error on Creating Payment Status",
                result.FirstError.Code));
    }

    [HttpGet]
    public async Task<IActionResult> AllPaymentStatuses(int page)
    {
        var result = await paymentStatusDAO.GetAll(page);
        return result.Match(
            paymentStatuses => Ok(paymentStatuses),
            errors => Problem(result.FirstError.Description,null,StatusCodes.Status500InternalServerError,result.FirstError.Code,null));
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetPaymentStatusById(long id)
    {
        ErrorOr<PaymentStatus> result = await paymentStatusDAO.GetById(id);
        Error error = result.FirstError;
        return result.Match(
            paymentStatus => Ok(result),
            errors => Problem(
                error.Description,
                null,
                StatusCodes.Status404NotFound,
                error.Code,
                null
            ));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdatePaymentStatus(long id, PaymentStatusRequest request)
    {
        PaymentStatus paymentStatus = new();
        paymentStatus.Id = id;
        paymentStatus.OrderId = request.OrderId;
        paymentStatus.PaymentMethod = Enum.Parse<PaymentMethod>(request.PaymentMethod);
        paymentStatus.Payed = false;
        paymentStatus.UpdatedAt = DateTime.UtcNow;
        var result = await paymentStatusDAO.Upsert(paymentStatus);

        return result.Match(
            paymentStatus => Ok(result.Value),
            errors => Problem(result.FirstError.Code));
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeletePaymentStatus(long id)
    {
        ErrorOr<bool> result =  await paymentStatusDAO.Delete(id);
        return result.Match(
            correct => Ok(result),
            error => Problem(
                detail:result.FirstError.Description,
                statusCode:StatusCodes.Status500InternalServerError,
                title:result.FirstError.Code));
    }
}