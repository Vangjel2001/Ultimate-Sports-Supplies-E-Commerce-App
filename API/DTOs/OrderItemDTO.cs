using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class OrderItemDTO
{
    [Range(minimum: 1, int.MaxValue, ErrorMessage = "Product ID has to be a positive number!")]
    public int ProductId { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "The product name cannot contain more than 100 characters!")]
    public string ProductName { get; set; } = string.Empty;

    [Required]
    public string PictureUrl { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "The price has to be a positive number!")]
    public decimal Price { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "The quantity has to be greater or equal to 1!")]
    public int Quantity { get; set; }
}
