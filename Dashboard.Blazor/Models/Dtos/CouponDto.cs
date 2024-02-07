namespace Dashboard.Blazor.Models.Dtos;

public class CouponDto
{
    public int Id { get; set; }

    [Required]
    public string CouponCode { get; set; } = null!;

    public string Image { get; set; } = null!;
    public IBrowserFile? UploadedImage { get; set; }

    [Required]
    [Label(name: "Start End Date")]
    public DateRange? StartEndDateRange { get; set; } = new DateRange(DateTime.Now.Date, DateTime.Now.AddDays(1).Date);

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int MaxUses { get; set; }

    public int NumberOfUses { get; set; }

    public double DiscountAmount { get; set; }

    public bool IsActive { get; set; }

    public List<ServiceDto> Services { get; set; } = new();
    public IEnumerable<int> SelectedServices { get; set; } = new List<int>();
    public List<CouponService> CouponServices { get; set; } = new();

    [Required]
    public CurrencyDto? Currency { get; set; }
    public IEnumerable<CurrencyDto>? Currencies { get; set; }
    public int CurrencyId { get; set; }

}

public class CouponService
{
    public int ServiceId { get; set; }
    public ServiceDto Service { get; set; } = new();
}
