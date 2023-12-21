namespace Dashboard.Blazor.Models.Dtos;

public class CouponDto
{
    public int Id { get; set; }

    [Required]
    [Label(name: "Title")]
    public string Title { get; set; } = null!;

    public string Image { get; set; } = null!;
    public IBrowserFile? UploadedImage { get; set; }

    public int? NumOfUses { get; set; }

    [Required]
    [Label(name: "Start End Date")]
    public DateRange? StartEndDateRange { get; set; } = new DateRange(DateTime.Now.Date, DateTime.Now.AddDays(1).Date);

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Required]
    [Label(name: "Percentage")]
    public double Percentage { get; set; }

    [Required]
    [Label(name: "Show In Home Page")]
    public bool ShowInHomePage { get; set; }

    [Label(name: "Description")]
    public string? Description { get; set; }

    [Label(name: "Notification Message")]
    public string? NotificationMessage { get; set; }

    [Required]
    [Label(name: "Max Discount")]
    public double MaxDiscount { get; set; }

    [Required]
    [Label(name: "Max Use")]
    public int MaxUse { get; set; }
}
