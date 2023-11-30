namespace DropShipping.Contracts;

public record ItemResponse(
    long Id,
    long ProductId,
    long OrderId,
    int Quantity
);