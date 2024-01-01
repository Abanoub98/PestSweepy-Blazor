namespace Dashboard.Blazor.Models.Dtos;

public class ContractDto
{
    [Required]
    public string Title { get; set; } = null!;

    public int Id { set; get; }

    public DateTime? CreatedAt { get; set; }

    [Required]
    public DateTime? EffectiveDate { get; set; } = DateTime.Now;

    [Required]
    public string FirstParty { get; set; } = null!;

    [Required]
    public string SecondParty { get; set; } = null!;

    [Required]
    public string SpExecutiveOfficer { get; set; } = null!;

    [Required]
    public string FpExecutiveOfficer { get; set; } = null!;

    [Required]
    public string SpMail { get; set; } = null!;

    [Required]
    public string FpMail { get; set; } = null!;

    [Required]
    public string SpExecutiveOfficerJob { get; set; } = null!;

    [Required]
    public string FpExecutiveOfficerJob { get; set; } = null!;

    [Required]
    public string FpCommercialRegistrationNo { get; set; } = null!;

    [Required]
    public string SpCommercialRegistrationNo { get; set; } = null!;

    [Required]
    public List<TermDto> Terms { get; set; } = new();

    [Required]
    public string ContractConclusion { get; set; } = null!;

    [Required]
    public string Notes { get; set; } = null!;

    [Required]
    public string ContractIntro { get; set; } = null!;

    [Required]
    public LookupDto? Quotation { get; set; }
    public int? QuotationId { get; set; }
    public IEnumerable<LookupDto>? Quotations { get; set; }

    [Required]
    public LookupDto? ContractDuration { get; set; }
    public int? ContractDurationId { get; set; }
    public IEnumerable<LookupDto>? ContractDurations { get; set; }

    [Required]
    public LookupDto? ContractClient { get; set; }
    public int? ContractClientId { get; set; }
    public IEnumerable<LookupDto>? ContractClients { get; set; }

    public IEnumerable<TermDto>? UploadedTerms { get; set; }
}

public class TermDto
{
    public string Title { get; set; } = null!;

    public string Term { get; set; } = null!;

    public string SelectedTerm { get; set; } = null!;
}