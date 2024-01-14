namespace Dashboard.Blazor.Models.Dtos;

public class CategoryDto
{
    public int Id { get; set; }

    [Label(name: "Order")]
    public int? OrderIndex { get; set; }

    [Required]
    public string NameAr { get; set; } = null!;

    [Required]
    public string NameEn { get; set; } = null!;

    [Required]
    [Label(name: "Color")]
    public string Color { get; set; } = null!;

    public string Image { get; set; } = null!;
    public IBrowserFile? UploadedImage { get; set; }
}
