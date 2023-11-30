using DropShipping.DTOs;
using ErrorOr;

namespace DropShipping.Services;

public interface IShipmentStateManagerService
{
    Task<ErrorOr<List<ShipmentStateManagerDTO>>> GetAllShipmentStates(int page);
}