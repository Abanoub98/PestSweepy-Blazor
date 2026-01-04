namespace Dashboard.Blazor.Models.Dtos;

public class BlogsDto
{
    public int Id { get; set; }

    public DateTime? Date { get; set; } = DateTime.Now;

    [StringLength(255)]
    public string Author { get; set; } = null!;

    [StringLength(255)]
    public string Title { get; set; } = null!;

    public string BlogIntroduction { get; set; } = null!;

    [StringLength(255)]
    public string Tags { get; set; } = null!;
    public int? OrderIndex { get; set; }
    public string Image { get; set; } = null!;
    public IBrowserFile? UploadedImage { get; set; }
}
