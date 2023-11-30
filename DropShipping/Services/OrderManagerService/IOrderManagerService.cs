using DropShipping.DTOs;
using ErrorOr;

namespace DropShipping.Services;

public interface IOrderManagerService{
    Task<ErrorOr<List<OrderManagerResponseDTO>>> GetOrderManagerDTOs(int pageNumber);   
}