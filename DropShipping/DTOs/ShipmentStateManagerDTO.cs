using DropShipping.Models;

namespace DropShipping.DTOs;

public class ShipmentStateManagerDTO{
    public long AgencyId {get;set;}
    public string AgencyName {get;set;}
    public string AgencyEmail {get;set;}
    public string AgencyPhoneNumber {get;set;}
    public DateTime CreatedAt {get;set;}    
    public double Cost {get;set;}
    
    public ShipmentStateManagerDTO(ShipmentState shipmentState){
        AgencyId = shipmentState.ShipmentAgencyId;
        AgencyName = shipmentState.ShipmentAgency!.Name;
        AgencyEmail = shipmentState.ShipmentAgency.Email;
        AgencyPhoneNumber = shipmentState.ShipmentAgency.ContactNumber;
        CreatedAt = shipmentState.CreatedAt;
        Cost = shipmentState.Cost;
    }
}