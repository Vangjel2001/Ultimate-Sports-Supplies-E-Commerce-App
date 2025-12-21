using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDTO
{
    [Required]
    [MaxLength(100, ErrorMessage = "The first name cannot contain more than 100 characters!")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100, ErrorMessage = "The last name cannot contain more than 100 characters!")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
