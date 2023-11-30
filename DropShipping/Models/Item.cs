using System.Text.Json.Serialization;

namespace DropShipping.Models;

public class Item : Base
{
    public long Id{get;set;}
    public long ProductId{get;set;}
    public long OrderId{get;set;}
    public uint Quantity{get;set;}
    public Product? Product{get;set;}
    public Order? Order{get;set;}
}