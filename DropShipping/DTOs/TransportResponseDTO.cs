namespace DropShipping.DTOs;

public class TransportResponseDTO{
    public long Id{get; set;}
    public long OrderId{get;set;}
    public long OfficeId{get;set;}
    public string ClientDocument{get;set;}
    public bool IsInOffice{get;set;}
    public bool Delivered{get;set;}
}