namespace DropShipping.Contracts;

public record TransportRequest(
    long OrderId,
    long OfficeId,
    string ClientDocument
);