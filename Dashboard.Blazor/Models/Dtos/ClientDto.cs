namespace Dashboard.Blazor.Models.Dtos;

public class ClientDto : BaseUser
{
    public int Id { set; get; }

    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public string Image { get; set; } = null!;
    public IBrowserFile? UploadedImage { get; set; }

    [Required]
    public string? Gender { get; set; }

    [Required]
    public LookupDto? Nationality { get; set; }
    public int NationalityId { get; set; }
    public IEnumerable<LookupDto>? Nationalities { get; set; }

    [Required]
    public LookupDto? Country { get; set; }
    public int? CountryId { get; set; }
    public IEnumerable<LookupDto>? Countries { get; set; }

    public bool IsContractClient { get; set; }
}
