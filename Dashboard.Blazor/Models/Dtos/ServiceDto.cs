namespace Dashboard.Blazor.Models.Dtos;

public class ServiceDto
{
    public int Id { get; set; }
    public int? OrderIndex { get; set; }
    public int DurationInMinutes { get; set; }
    public string Name { get; set; } = null!;
    public string Image { get; set; } = null!;
    public LookupDto? Category { get; set; }
}
