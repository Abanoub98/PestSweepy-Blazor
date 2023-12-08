namespace Dashboard.Blazor.Models.Dtos;

public class ClientDto : BaseUser
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
}
