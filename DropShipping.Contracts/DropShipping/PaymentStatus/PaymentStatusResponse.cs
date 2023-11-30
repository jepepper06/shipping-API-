namespace DropShipping.Contracts;

using DropShipping;

public record PaymentStatusResponse(
    long Id,
    long OrderId,
    string PaymentMethod,
    bool Payed
);