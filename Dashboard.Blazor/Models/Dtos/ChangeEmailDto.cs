namespace Dashboard.Blazor.Models.Dtos;

public class ChangeEmailDto
{
    [EmailAddress]
    [Required]
    public string NewEmail { get; set; } = string.Empty;

    public string UserId { get; set; } = null!;

    public string? Message { get; set; }
}
