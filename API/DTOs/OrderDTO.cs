using System;

namespace API.DTOs;

public class OrderDTO
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public required string BuyerEmail { get; set; }
    public required ShippingAddressDTO ShippingAddress { get; set; }
    public required string DeliveryMethod { get; set; }
    public decimal ShippingPrice { get; set; }
    public required PaymentSummaryDTO PaymentSummary { get; set; }
    public required List<OrderItemDTO> OrderItems { get; set; }
    public decimal Subtotal { get; set; }
    public required string Status { get; set; }
    public decimal Total { get; set; }
    public required string PaymentIntentId { get; set; }
}
