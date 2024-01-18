namespace Dashboard.Blazor.Models.Dtos;

public class ComplaintDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Subject { get; set; } = null!;
    public bool IsResolved { get; set; }
    public ClientDto Client { get; set; } = new();
    public LookupDto ComplaintType { get; set; } = new();
    public string Image { get; set; } = null!;
}
