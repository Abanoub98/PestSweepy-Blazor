namespace Dashboard.Blazor.Models.Dtos;

public class ReviewDto
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public double Rate { get; set; }

    public string? Feedback { get; set; }

    public bool ShowInApp { get; set; }

    public ClientDto Client { get; set; } = new();

    public OrderDto Order { get; set; } = new();
}
