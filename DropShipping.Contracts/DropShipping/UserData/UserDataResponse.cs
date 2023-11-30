namespace DropShipping.Contracts;

public record UserDataResponse(
    long Id,
    long UserId,
    string PhoneNumber,
    string Password,
    string Email,
    string Name, 
    string IdentificationDocument
);