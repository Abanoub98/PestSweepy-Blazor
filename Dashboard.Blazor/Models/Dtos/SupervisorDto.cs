namespace Dashboard.Blazor.Models.Dtos;

public class SupervisorDto : BaseUser
{
    public int Id { set; get; }

    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string NationalId { get; set; } = null!;

    public string Image { get; set; } = null!;
    public IBrowserFile? UploadedImage { get; set; }

    [Required]
    public string? Gender { get; set; }

    [Required]
    public LookupDto? Nationality { get; set; }
    public int NationalityId { get; set; }
    public IEnumerable<LookupDto>? Nationalities { get; set; }

    [Required]
    public LookupDto? Manager { get; set; }
    public int ManagerId { get; set; }
    public IEnumerable<LookupDto>? Managers { get; set; }

    public List<ProviderDto> ProvidersList { get; set; } = new();
}
