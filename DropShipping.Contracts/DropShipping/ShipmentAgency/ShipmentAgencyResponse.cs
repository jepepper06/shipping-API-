namespace DropShipping.Contracts;

public record ShipmentAgencyResponse(
    long Id,
    string Name,
    string Email,
    string ContactNumber
);