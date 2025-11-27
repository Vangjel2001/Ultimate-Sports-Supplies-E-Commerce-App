using System;

namespace Core.Entities.OrderAggregate;

public class OrderedProductItem
{
    public int ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string Picture1Url { get; set; }
}
