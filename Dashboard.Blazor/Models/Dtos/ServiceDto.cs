namespace Dashboard.Blazor.Models.Dtos;

public class ServiceDto
{
    public int Id { get; set; }

    [Label(name: "Order Index")]
    public int? OrderIndex { get; set; }

    [Required]
    [Label(name: "Duration")]
    public int? DurationInMinutes { get; set; }

    [Required]
    [Label(name: "Name")]
    public string Name { get; set; } = null!;

    [Required]
    [Label(name: "Description")]
    public string Description { get; set; } = null!;

    [Required]
    [Label(name: "Category")]
    public LookupDto? Category { get; set; }
    public int CategoryId { get; set; }
    public IEnumerable<LookupDto>? Categories { get; set; }

    public string Image { get; set; } = null!;
    public IBrowserFile? UploadedImage { get; set; }
}
