namespace DropShipping.Contracts;

public record UserDataRequest(
    long UserId,
    string PhoneNumber,
    string Password,
    string Email,
    string Name,
    string IdentificationDocument
);