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

    [Label(name: "Contact Phone Number")]
    public string? ContactPhone { get; set; }

    [EmailAddress]
    [Label(name: "Contact Email")]
    public string? ContactEmail { get; set; }
}
