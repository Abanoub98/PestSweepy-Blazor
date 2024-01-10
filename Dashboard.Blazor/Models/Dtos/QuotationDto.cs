namespace Dashboard.Blazor.Models.Dtos;

public class QuotationDto
{
    public int Id { set; get; }
    public string SerialNumber { set; get; } = null!;

    [Required]
    [Label(name: "Date")]
    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    [Required]
    [Label(name: "Notes")]
    public string Notes { get; set; } = null!;

    [Required]
    [EmailAddress]
    [Label(name: "Receiver Email")]
    public string ClientEmail { get; set; } = null!;

    [Label(name: "Client")]
    public LookupDto? Client { get; set; }
    public int? ClientId { get; set; }
    public IEnumerable<LookupDto>? Clients { get; set; }

    [Label(name: "Client Name")]
    public string ClientName { get; set; } = null!;

    [Required]
    public string Ceo { get; set; } = null!;

    [Required]
    [Label(name: "Total Price")]
    public double TotalPrice { get; set; }

    [Required]
    public CurrencyDto? PriceCurrency { get; set; }
    public int PriceCurrencyId { get; set; }
    public IEnumerable<CurrencyDto>? PriceCurrencies { get; set; }

    public List<QuotationServiceType> QuotationBodies { get; set; } = new();
}

public class QuotationServiceType
{
    [Required]
    public LookupDto? Category { get; set; }
    public int CategoryId { get; set; }
    public IEnumerable<LookupDto>? Categories { get; set; }

    [Required]
    [Label(name: "Service")]
    public ServiceDto? Service { get; set; }
    public int ServiceId { get; set; }
    public IEnumerable<ServiceDto>? Services { get; set; }

    [Required]
    [Label(name: "Unit")]
    public LookupDto? Unit { get; set; }
    public int UnitId { get; set; }
    public IEnumerable<LookupDto>? Units { get; set; }

    [Required]
    [Label(name: "Type")]
    public LookupDto? Type { get; set; }
    public int TypeId { get; set; }
    public IEnumerable<LookupDto>? Types { get; set; }

    public int NoOfVisits { get; set; }

    [Required]
    [Label(name: "Area")]
    public double Area { get; set; }

    [Required]
    [Label(name: "Price")]
    public double Price { get; set; }

    [Required]
    [Label(name: "Discount Rate")]
    public double DiscountRate { get; set; }

    [Required]
    [Label(name: "Discount Price")]
    public double DiscountPrice { get; set; }

    [Required]
    [Label(name: "Total Price")]
    public double TotalPrice { get; set; }

    [Required]
    [Label(name: "Quantity")]
    public int Quantity { get; set; }
}