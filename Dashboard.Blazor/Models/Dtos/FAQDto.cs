namespace Dashboard.Blazor.Models.Dtos;

public class FAQDto
{
    public int Id { get; set; }

    public string Question { get; set; } = null!;

    public string Answer { get; set; } = null!;

    public int? OrderIndex { get; set; }
}
