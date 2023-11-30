namespace DropShipping.Controllers;

using DropShipping.DAOs;
using Microsoft.AspNetCore.Mvc;
using DropShipping.Contracts;
using DropShipping.Models;
using System;
using ErrorOr;

[ApiController]
[Route("transports")]
public class TransportController : Controller
{
    private readonly TransportDAO _transportDAO;

    public TransportController(TransportDAO transportDAO)
    {
        _transportDAO = transportDAO;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransport(TransportRequest request)
    {
        var transport = new Transport(){
            OrderId = request.OrderId,
            OfficeId = request.OfficeId,
            ClientDocument = request.ClientDocument,
            IsInOffice = false,
            Delivered = false
        };
        transport.Create();
        var result = await _transportDAO.Save(transport);

        return result.Match(
            porsi => Ok(result.Value),
            oporno => Problem(
                statusCode: 500,
                title: result.FirstError.Code,
                type: "Error creating Transport")
        );
    }
}