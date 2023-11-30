namespace DropShipping.Models;

public class Product : Base
{
    public long Id {get; set;}
    public string Name {get; set;} = null!;
    public double Price {get; set;} 
    public long? LinkId{get;set;}
    public Link? Link {get;set;}
    public ICollection<Image>? Images {get;set;} = new List<Image>();
    public ICollection<Item>? Items{get;set;} = new List<Item>();

}