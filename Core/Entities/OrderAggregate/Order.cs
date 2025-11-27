using System;

namespace Core.Entities.OrderAggregate;

public class Order : BaseEntity
{
    public OrderStatus Status { get; set; } = OrderStatus.PaymentPending;
    public PaymentSummary PaymentSummary { get; set; }
    public ShippingAddress ShippingAddress { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public List<OrderItem> OrderItems { get; set; } = [];
    public int PaymentIntentId { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public required string BuyerEmail { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
}
