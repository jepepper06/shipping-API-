using DropShipping.DAOs;
using DropShipping.DTOs;
using ErrorOr;

namespace DropShipping.Services;

public class ShipmentStateManagerService : IShipmentStateManagerService
{
    private readonly ShipmentStateDAO _shipmentStateDAO;
    public ShipmentStateManagerService(ShipmentStateDAO shipmentStateDAO){
        _shipmentStateDAO = shipmentStateDAO;
    }
    public async Task<ErrorOr<List<ShipmentStateManagerDTO>>> GetAllShipmentStates(int page)
    {
        List<ShipmentStateManagerDTO> shipmentStateManagerDTOs = new();
        
        var resultShipmentStates = await _shipmentStateDAO.GetAll(page);
        
        if(resultShipmentStates.IsError)
            return resultShipmentStates.FirstError;
        
        var shipmentStates = resultShipmentStates.Value;
        
        foreach(var shipmentState in shipmentStates){
            var shipmentStateManagerDTO = new ShipmentStateManagerDTO(shipmentState);
            shipmentStateManagerDTOs.Add(shipmentStateManagerDTO);
        }
        
        return shipmentStateManagerDTOs;
        
    }
}