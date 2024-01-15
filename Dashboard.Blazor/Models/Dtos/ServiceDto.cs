namespace Dashboard.Blazor.Models.Dtos;

public class ServiceDto
{
    public int Id { get; set; }

    public int? OrderIndex { get; set; }

    [Required]
    public int? DurationInMinutes { get; set; }

    [Required]
    public string NameAr { get; set; } = null!;

    [Required]
    public string NameEn { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    [Required]
    public LookupDto? Category { get; set; }
    public int CategoryId { get; set; }
    public IEnumerable<LookupDto>? Categories { get; set; }

    public string Image { get; set; } = null!;
    public IBrowserFile? UploadedImage { get; set; }

    public List<ServiceOption> ServiceOptions { get; set; } = new();
}

public class ServiceOption
{
    [Required]
    public string Name { get; set; } = null!;

    public int? OrderIndex { get; set; }

    public bool Disabled { get; set; }

    public List<ServiceOptionPrice> Prices { get; set; } = new();
}


public class ServiceOptionPrice
{
    public int Id { get; set; }

    public double? Amount { get; set; }

    public int CurrencyId { get; set; }
    public CurrencyDto? Currency { get; set; }
}
