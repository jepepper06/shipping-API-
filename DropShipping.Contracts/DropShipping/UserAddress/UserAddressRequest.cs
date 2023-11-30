namespace DropShipping.Contracts;

public record UserAddressRequest(
    long UserId,
    string Address,
    int Number,
    long CityId,
    int PostalCode
);