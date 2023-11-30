using DropShipping.DAOs;
using DropShipping.DTOs;
using ErrorOr;

namespace DropShipping.Services;

public class OrderManagerService : IOrderManagerService
{
    private readonly UserDataDAO _usersDataDAO;
    private readonly CityDAO _cityDAO;
    private readonly OrderDAO _orderDAO;
    public OrderManagerService(
        UserDataDAO usersDataDAO,
        CityDAO cityDAO,
        OrderDAO orderDAO){
        _usersDataDAO = usersDataDAO;
        _cityDAO = cityDAO;
        _orderDAO = orderDAO;
    }

    public async Task<ErrorOr<List<OrderManagerResponseDTO>>> GetOrderManagerDTOs(int pageNumber)
    {
        var orderListResult = await _orderDAO.GetOrderPopulated((uint) pageNumber);
        if(orderListResult.IsError)
            return orderListResult.FirstError;
        var orderList = orderListResult.Value;
        
        List<OrderManagerResponseDTO> orderManagerResponseDTOs = new();

        foreach(var order in orderList){
            var result = new OrderManagerResponseDTO(order);
            orderManagerResponseDTOs.Add(result);
        }
        return orderManagerResponseDTOs;
    }
}