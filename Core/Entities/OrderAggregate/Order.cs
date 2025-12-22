using System;

namespace Core.Entities.OrderAggregate;

public class Order : BaseEntity
{
    public OrderStatus Status { get; set; } = OrderStatus.PaymentPending;
    public PaymentSummary PaymentSummary { get; set; } = null!;
    public ShippingAddress ShippingAddress { get; set; } = null!;
    public DeliveryMethod DeliveryMethod { get; set; } = null!;
    public List<OrderItem> OrderItems { get; set; } = [];
    public required string PaymentIntentId { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public required string BuyerEmail { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
}
