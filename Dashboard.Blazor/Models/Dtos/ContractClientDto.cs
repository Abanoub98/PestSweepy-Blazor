namespace Dashboard.Blazor.Models.Dtos;

public class ContractClientDto : BaseUser
{
    public int Id { set; get; }

    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? CreatedAtLocal { get => CreatedAt?.ToLocalTime(); }

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

    [Required]
    [StringLength(20)]
    public string? ContactMobile { get; set; }

    [Required]
    [StringLength(20)]
    public string? ContactPhone { get; set; }

    [Required]
    public string? ContactEmail { get; set; }

    [Required]
    public string? Address { get; set; }

    [Required(ErrorMessage = "VAT Registration Number is Required")]
    [RegularExpression(@"^\d+$", ErrorMessage = "VAT Registration Number must contain digits only")]
    [StringLength(15, MinimumLength = 15, ErrorMessage = "VAT Registration Number must be exactly 15 digits")]
    public string? VatRegistrationNumber { get; set; }

    [Required(ErrorMessage = "Commercial Registration Number is Required")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Commercial Registration Number must contain digits only")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Commercial Registration Number must be exactly 10 digits")]
    public string? CommercialRegistrationNo { get; set; } = null!;
}
