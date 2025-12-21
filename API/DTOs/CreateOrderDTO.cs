using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CreateOrderDTO
{
    [Required]
    public string CartId { get; set; } = string.Empty;

    [Required]
    [Range(minimum: 1, int.MaxValue, ErrorMessage = "Delivery Method ID has to be a positive number!")]
    public int DeliveryMethodId { get; set; }

    [Required]
    public PaymentSummaryDTO PaymentSummaryDTO { get; set; } = null!;

    [Required]
    public ShippingAddressDTO ShippingAddressDTO { get; set; } = null!;
}
