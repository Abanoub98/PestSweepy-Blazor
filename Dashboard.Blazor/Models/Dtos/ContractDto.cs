namespace Dashboard.Blazor.Models.Dtos;

public class ContractDto
{
    [Required]
    public string Title { get; set; } = null!;

    public int Id { set; get; }

    public DateTime CreatedAt { get; set; }

    public DateTime CreatedAtLocal { get => CreatedAt.ToLocalTime(); }

    [Required]
    public DateTime? EffectiveDate { get; set; }

    [Required]
    public DateTime? EffectiveDateLocal
    {
        get
        {
            if (EffectiveDate is null)
                return DateTime.Now;

            return EffectiveDate?.ToLocalTime();
        }
        set
        {
            EffectiveDate = value?.ToUniversalTime();
        }
    }

    public DateTime? EndDate { get; set; }

    public DateTime? EndDateLocal
    {
        get
        {
            if (EndDate is null)
                return DateTime.Now;

            return EndDate?.ToLocalTime();
        }
        set
        {
            EndDate = value?.ToUniversalTime();
        }
    }


    public string FpExecutiveOfficer { get; set; } = null!;
    public string FpExecutiveOfficerJob { get; set; } = null!;

    [Required]
    public string SpExecutiveOfficer { get; set; } = null!;

    [Required]
    public string SpExecutiveOfficerJob { get; set; } = null!;

    public List<TermDto>? Terms { get; set; } = new();

    public string ContractConclusion { get; set; } = null!;

    public string Notes { get; set; } = null!;

    public string? ContractIntro { get; set; }

    [Required]
    public LookupDto? ContractDuration { get; set; }
    public int? ContractDurationId { get; set; }
    public IEnumerable<LookupDto>? ContractDurations { get; set; }

    [Required]
    public ContractClientDto ContractClient { get; set; } = null!;
    public int? ContractClientId { get; set; }
    public IEnumerable<ContractClientDto>? ContractClients { get; set; }

    public IEnumerable<TermDto>? UploadedTerms { get; set; }

    public int QuotationID { get; set; }
    public QuotationDto Quotation { get; set; } = null!;
    public IEnumerable<QuotationDto> Quotations { get; set; } = null!;

    public int PaymentMethodId { get; set; }
    public LookupDto PaymentMethod { get; set; } = null!;
    public IEnumerable<LookupDto> PaymentMethods { get; set; } = null!;

    public string? SerialNumber { get; set; }

    public CompanyInfoDto CompanyInfo { get; set; } = null!;

    public string? UploadedContract { get; set; }
}

public class TermDto
{
    public string? Title { get; set; }

    public string? Term { get; set; }
    public string? TitleAr { get; set; }
    public string? TermAr { get; set; }

    public string? SelectedTerm { get; set; }

    //public QuotationDto? Quotation { get; set; }
    //public int? QuotationId { get; set; }

    //public bool ShowQuotationsList { get; set; }
}