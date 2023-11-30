namespace DropShipping.Contracts;

public record OfficeRequest(
    string Name,
    long CityId,
    int PostalCode,
    string PhoneNumber,
    string Email
);