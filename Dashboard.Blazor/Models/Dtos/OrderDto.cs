namespace Dashboard.Blazor.Models.Dtos;

public class OrderDto
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public ServiceDto Service { get; set; } = new();

    public ClientDto Client { get; set; } = new();

    public ProviderDto? Provider { get; set; }

    public PaymentDto? Payment { get; set; }

    public LookupDto PaymentMethod { get; set; } = new();

    public int Quantity { get; set; }

    public double TotalAmount { get; set; }

    public double VAT { get; set; }
}
