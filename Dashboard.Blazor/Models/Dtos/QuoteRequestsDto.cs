namespace Dashboard.Blazor.Models.Dtos;

public class QuoteRequestsDto
{
    public int Id { get; set; }

    // Customer Info
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;

    // Linked Service
    public int ServiceId { get; set; }
    public ServiceBaseDto Service { get; set; } = null!;

    // Request Status
    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? CreatedAtLocal
    {
        get => CreatedAt?.ToLocalTime();
        set => CreatedAt = value?.ToUniversalTime();
    }

    public string? Notes { get; set; }
}

public class ServiceBaseDto
{
    public int Id { get; set; }
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public int? OrderIndex { get; set; }
    public string? Image { get; set; }
    public int CategoryId { get; set; }
    public string? Description { get; set; }
    public int? DurationInMinutes { get; set; }
}