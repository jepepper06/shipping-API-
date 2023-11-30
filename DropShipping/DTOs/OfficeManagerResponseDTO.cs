using DropShipping.Models;

namespace DropShipping.DTOs;

public class OfficeManagerResponseDTO{
    public OfficeManagerResponseDTO(Office office){
        Id = office.Id;
        OfficeName = office.Name;
        PhoneNumber = office.PhoneNumber;
        CityId = office.CityId;
        CityName = office.City!.Name;
        PostalCode = office.PostalCode;
    }
    public long Id {get;}
    public string OfficeName {get;}
    public string PhoneNumber{get;} 
    public long CityId {get;}
    public string CityName {get;}
    public int PostalCode {get;}
}