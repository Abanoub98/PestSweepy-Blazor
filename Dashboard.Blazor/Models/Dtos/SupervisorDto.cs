namespace Dashboard.Blazor.Models.Dtos;

public class SupervisorDto
{
    public int Id { set; get; }

    [Required]
    [Label(name: "Name")]
    public string Name { get; set; } = null!;

    public string Image { get; set; } = null!;
    public IBrowserFile? UploadedImage { get; set; }

    [Required]
    [Label(name: "Gender")]
    public string? Gender { get; set; }

    [Required]
    [Label(name: "Nationality")]
    public LookupDto? Nationality { get; set; }
    public int NationalityId { get; set; }
    public IEnumerable<LookupDto>? Nationalities { get; set; }

    [EmailAddress]
    [Label(name: "Email")]
    public string? Email { get; set; }

    [Label(name: "Password")]
    public string? Password { get; set; }

    [Label(name: "Phone Number")]
    public string? PhoneNumber { get; set; }
}
