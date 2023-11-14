namespace Dashboard.Blazor.Models.Dtos;

public class CategoryDto
{
    public int Id { get; set; }
    public int? OrderIndex { get; set; }
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public string Image { get; set; } = null!;
}
