using System;

namespace Core.Entities;

public class Product : BaseEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public int StockLevel { get; set; }
    public DateTime ArrivalDate { get; set; } = DateTime.UtcNow;
    public Brand Brand { get; set; }
    public Type Type { get; set; }
}
