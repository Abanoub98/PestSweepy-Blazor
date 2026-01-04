namespace Dashboard.Blazor.Models.Dtos;

public class TestimonialsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Profession { get; set; }
    public string TestimonialText { get; set; } = null!;
    public int Rating { get; set; }
    public int? OrderIndex { get; set; }
    public string Image { get; set; } = null!;
    public IBrowserFile? UploadedImage { get; set; }
}
