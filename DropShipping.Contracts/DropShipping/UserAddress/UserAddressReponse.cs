namespace DropShipping.Contracts;

public record UserAddressResponse(
    long Id,
    long UserId,
    string Address,
    int Number,
    long CityId,
    int PostalCode
);