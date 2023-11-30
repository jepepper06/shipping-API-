namespace DropShipping.Contracts;

public record ShipmentAgencyRequest(
    string Name,
    string Email,
    string ContactNumber
);