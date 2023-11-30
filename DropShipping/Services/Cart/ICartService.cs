using DropShipping.DTOs;
using ErrorOr;

namespace DropShipping.Services;

public interface ICartService{
    Task<ErrorOr<CartResponseDTO>> GetCartResponseDTO(long userId);
}