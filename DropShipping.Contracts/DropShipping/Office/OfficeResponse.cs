namespace DropShipping.Contracts;

public record OfficeResponse(
    long Id,
    string Name,
    string CityId,
    int PostalCode
);