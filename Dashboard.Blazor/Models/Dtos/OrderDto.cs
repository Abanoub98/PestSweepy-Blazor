namespace Dashboard.Blazor.Models.Dtos;

public class OrderDto
{
    public int Id { get; set; }

    public string OrderNumber { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime ReservationDate { get; set; }

    public bool OrderAccepted { get; set; }

    public ServiceOption ServiceOption { get; set; } = new();

    public ClientDto Client { get; set; } = new();

    public PaymentDto? Payment { get; set; }

    public LookupDto OrderState { get; set; } = new();

    public LookupDto PaymentMethod { get; set; } = new();

    public int Quantity { get; set; }

    public double TotalAmount { get; set; }

    public double VAT { get; set; }

    public string Notes { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string ApartmentNumber { get; set; } = null!;

    public string BuildingNumber { get; set; } = null!;

    public double Longitude { get; set; }

    public double Latitude { get; set; }

    public string AddressUrl { get; set; } = null!;

    public ProviderDto? Provider { get; set; }
    public int? ProviderId { get; set; }
    public IEnumerable<ProviderDto>? Providers { get; set; }
}