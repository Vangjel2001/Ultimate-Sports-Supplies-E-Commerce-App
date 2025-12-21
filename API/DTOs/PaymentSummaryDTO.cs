using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class PaymentSummaryDTO
{
    [Range(0, 9999, ErrorMessage = "Please provide exactly 4 digits!")]
    public int Last4 { get; set; }

    [Range(1, 12, ErrorMessage = "The expiry month has to be between 1 and 12!")]
    public int ExpMonth { get; set; }

    //TODO: If you want, provide data annotation for min number
    public int ExpYear { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "The credit card brand cannot contain more than 100 characters!")]
    public string Brand { get; set; } = string.Empty;
}
