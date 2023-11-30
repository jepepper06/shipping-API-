namespace DropShipping.Models;

public class Link{
    public long Id {get; set;}
    public long ProductId{get;set;}
    public Product Product{get;set;}
    public string ProductAlibabaURL{get;set;}
}