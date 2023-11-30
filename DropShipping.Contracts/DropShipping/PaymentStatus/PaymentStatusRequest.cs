namespace DropShipping.Contracts;

public record PaymentStatusRequest(
    long OrderId,
    string PaymentMethod,
    bool Payed
);