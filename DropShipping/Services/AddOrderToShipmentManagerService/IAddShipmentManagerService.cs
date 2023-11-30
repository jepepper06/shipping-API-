namespace DropShipping.Services;

public interface IAddOrderToShipmentManagerService{
    Task<bool> AddOrdersToShipment(long shipmentAgencyId);
}