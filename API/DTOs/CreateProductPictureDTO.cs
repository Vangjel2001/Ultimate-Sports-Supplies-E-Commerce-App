using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CreateProductPictureDTO
{
    [Range(minimum: 1, int.MaxValue, ErrorMessage = "Product ID has to be a positive number!")]
    public int ProductId { get; set; }

    [Required]
    public string PictureUrl { get; set; } = string.Empty;
}
