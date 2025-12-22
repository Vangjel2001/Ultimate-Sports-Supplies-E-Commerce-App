using System;

namespace Core.Entities.OrderAggregate;

public class OrderItem : BaseEntity
{
    public OrderedProductItem OrderedItem { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public required string PictureUrl { get; set; }
}
