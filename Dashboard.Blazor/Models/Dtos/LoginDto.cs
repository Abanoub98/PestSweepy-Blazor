using System.ComponentModel.DataAnnotations;

namespace Dashboard.Blazor.Models.Dtos;

public class LoginDto
{
    [EmailAddress]
    [Required]
    public string? Email { get; set; } = null!;
    [Required]
    public string? Password { get; set; } = null!;
    public string? code { get; set; }
}
