namespace Dashboard.Blazor.Models.Dtos;

public class AccountProfile
{
    public string Id { get; set; } = null!;
    public int EntityId { get; set; }
    public string? EntityName { get; set; }
    public string? EntityImage { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Role { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool EmailConfirmed { get; set; }
}
