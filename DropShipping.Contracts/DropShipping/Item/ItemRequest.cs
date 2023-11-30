namespace DropShipping.Contracts;

public record ItemRequest(
    long ProductId,
    long OrderId,
    int Quantity
);