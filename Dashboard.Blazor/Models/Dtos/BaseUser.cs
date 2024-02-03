namespace Dashboard.Blazor.Models.Dtos;

public class BaseUser
{
    [EmailAddress]
    [Label(name: "Email")]
    public string? Email { get; set; }

    [Label(name: "Password")]
    public string? Password { get; set; }

    [Label(name: "Phone Number")]
    public string? PhoneNumber { get; set; }
}
