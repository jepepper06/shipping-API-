namespace DropShipping.Models;

public class Base
{
    public DateTime CreatedAt{get; set;}
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public void Update(){
        UpdatedAt = DateTime.UtcNow;
    }
    public void Create(){
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    public Base(){
        this.Create();
    }
}