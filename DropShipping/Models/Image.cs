namespace DropShipping.Models;
public class Image{
    public long Id{get;set;}
    public long ProductId{get;set;}
    public string ImageURL{ get; set;}
    public Product Product{get;set;}
}