using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CreateProductDTO
{
    [Required]
    [MaxLength(100, ErrorMessage = "The product name cannot contain more than 100 characters!")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(500, ErrorMessage = "The product description cannot contain more than 500 characters!")]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "The price has to be a positive number!")]
    public decimal Price { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "The product's stock level has to be greater or equal to 1!")]
    public decimal StockLevel { get; set; }

    // TODO: Brand and Type and picture urls
}
