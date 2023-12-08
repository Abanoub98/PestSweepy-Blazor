namespace Dashboard.Blazor.Models.Dtos;

public class ContractClientDto : BaseUser
{
    public int Id { set; get; }

    [Required]
    [Label(name: "Date")]
    public DateTime? Date { get; set; }

    [Required]
    [Label(name: "Number")]
    public int Number { get; set; }

    [Required]
    [Label(name: "First Party")]
    public string? FirstParty { get; set; }

    [Required]
    [Label(name: "Second Party")]
    public string? SecondParty { get; set; }

    [Required]
    [Label(name: "Second Party Executive Officer")]
    public string? SecondPartyExecutiveOfficer { get; set; }

    [Required]
    [Label(name: "First Party Executive Officer")]
    public string? FirstPartyExecutiveOfficer { get; set; }

    [Required]
    [Label(name: "Terms")]
    public List<Term> Terms { get; set; } = new();

    [Required]
    [Label(name: "Quotation")]
    public LookupDto? Quotation { get; set; }
    public int? QuotationId { get; set; }
    public IEnumerable<LookupDto>? Quotations { get; set; }
}

public class Term
{
    public string? TermContent { get; set; }
}