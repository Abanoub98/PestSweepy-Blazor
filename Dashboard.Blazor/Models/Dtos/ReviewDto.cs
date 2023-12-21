namespace Dashboard.Blazor.Models.Dtos;

public class ReviewDto
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public double Rate { get; set; }

    public string? Feedback { get; set; }

    public string? Title { get; set; }

    public bool ShowInHomePage { get; set; }

    public ClientDto Client { get; set; } = null!;

    //public OrderDto Order { get; set; } = null!;
}
