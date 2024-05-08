namespace Dashboard.Blazor.Models.Dtos;

public class OrderDto
{
    public int Id { get; set; }

    public string OrderNumber { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime CreatedAtLocal { get => CreatedAt.ToLocalTime(); }

    public DateTime? ReservationDate { get; set; }

    [Required]
    public DateTime? ReservationDateLocal
    {
        get
        {
            if (ReservationDate is null)
                return DateTime.Now;

            return ReservationDate?.ToLocalTime();
        }
        set
        {
            ReservationDate = value;
        }
    }
    public int CouponId { get; set; }

    public bool OrderAccepted { get; set; }

    public PaymentDto? Payment { get; set; }

    public LookupDto OrderState { get; set; } = new();

    public LookupDto PaymentMethod { get; set; } = new();

    [Required]
    public int Quantity { get; set; }

    public double TotalAmount { get; set; }

    public double VAT { get; set; }

    [Required]
    public string Notes { get; set; } = null!;

    [Required]
    public string Address { get; set; } = null!;

    [Required]
    public string ApartmentNumber { get; set; } = null!;

    [Required]
    public string BuildingNumber { get; set; } = null!;

    [Required]
    public double Longitude { get; set; }

    [Required]
    public double Latitude { get; set; }

    public string AddressUrl { get; set; } = null!;

    public ProviderDto? Provider { get; set; }
    public int? ProviderId { get; set; }
    public IEnumerable<ProviderDto>? Providers { get; set; }

    [Required]
    public ServiceOption? ServiceOption { get; set; }
    public int ServiceOptionId { get; set; }
    public IEnumerable<ServiceOption>? ServiceOptions { get; set; }

    [Required]
    [Label(name: "Service")]
    public ServiceDto? Service { get; set; }
    public int ServiceId { get; set; }
    public IEnumerable<ServiceDto>? Services { get; set; }

    [Required]
    public LookupDto? Category { get; set; }
    public int CategoryId { get; set; }
    public IEnumerable<LookupDto>? Categories { get; set; }

    [Required]
    public ClientDto? Client { get; set; }
    public int ClientId { get; set; }
    public IEnumerable<ClientDto>? Clients { get; set; }
}