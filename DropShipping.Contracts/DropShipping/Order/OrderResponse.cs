namespace DropShipping.Contracts;

public record OrderResponse(
    long Id,
    long UserId,
    long OfficeId,
    long ShipmentStateId,
    double Total,
    long PaymentStatusId
);