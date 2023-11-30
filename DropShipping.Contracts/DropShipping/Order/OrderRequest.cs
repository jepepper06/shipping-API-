namespace DropShipping.Contracts;

public record OrderRequest(
    long UserId,
    long OfficeId,
    long ShipmentStateId,
    double Total,
    long PaymentStatusId
);