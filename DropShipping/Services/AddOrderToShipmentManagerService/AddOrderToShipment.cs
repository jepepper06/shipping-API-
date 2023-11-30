using DropShipping.DAOs;
using DropShipping.Models;

namespace DropShipping.Services;


public class AddOrderToShipmentManagerService : IAddOrderToShipmentManagerService
{
    private readonly ShipmentStateDAO _shipmentStateDAO;
    private readonly OrderDAO _orderDAO;
    public AddOrderToShipmentManagerService
        (OrderDAO orderDAO,
        ShipmentStateDAO shipmentStateDAO)
        {
            _shipmentStateDAO = shipmentStateDAO;
            _orderDAO = orderDAO;
        }

    public async Task<bool> AddOrdersToShipment(long shipmentAgencyId)
    {
        var result = await _orderDAO.GetAllOrdersWithNullShipmentState();
        if(result.IsError)
            return false;

        var orders = result.Value;

        var shipmentState = new ShipmentState(){
            ShipmentAgencyId = shipmentAgencyId,
            Cost = 0,
            ShipmentStatus = ShipmentStatus.NOT_SHIPPED,
            Orders = orders
        };
        var anotherResult = await _shipmentStateDAO.Upsert(shipmentState);
        if(anotherResult.IsError)
            return false;
        return true;        
    }
}