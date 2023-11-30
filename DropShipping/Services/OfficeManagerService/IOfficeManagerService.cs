using DropShipping.DTOs;
using ErrorOr;

namespace DropShipping.Services;

public interface IOfficeManagerService{
    Task<ErrorOr<List<OfficeManagerResponseDTO>>> GetAllOfficeManagerResponse(int pageNumber);
}