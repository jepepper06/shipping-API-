using DropShipping.DTOs;
using DropShipping.DAOs;
using ErrorOr;

namespace DropShipping.Services;

public class OfficeManagerService : IOfficeManagerService
{
    private readonly OfficeDAO _officeDAO;
    public OfficeManagerService(OfficeDAO officeDAO){
        _officeDAO = officeDAO;
    }
    public async Task<ErrorOr<List<OfficeManagerResponseDTO>>> GetAllOfficeManagerResponse(int pageNumber)
    {
        List<OfficeManagerResponseDTO> officeManagerResponses = new();
        var resultOffices = await _officeDAO.GetAll((int)pageNumber);
        if(resultOffices.IsError)
            return resultOffices.FirstError;
        var offices = resultOffices.Value;
        foreach(var office in offices){
            var newOfficeManagerResponse = new OfficeManagerResponseDTO(office);
            officeManagerResponses.Add(newOfficeManagerResponse);
        }
        return officeManagerResponses;

    }
}