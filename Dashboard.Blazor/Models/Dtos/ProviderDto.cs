namespace Dashboard.Blazor.Models.Dtos;

public class ProviderDto
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

    [Required]
    [Label(name: "Supervisor")]
    public LookupDto? Supervisor { get; set; }
    public int SupervisorId { get; set; }
    public IEnumerable<LookupDto>? Supervisors { get; set; }

    [EmailAddress]
    [Label(name: "Email")]
    public string? Email { get; set; }

    [Label(name: "Password")]
    public string? Password { get; set; }

    [Label(name: "Phone Number")]
    public string? PhoneNumber { get; set; }
}
