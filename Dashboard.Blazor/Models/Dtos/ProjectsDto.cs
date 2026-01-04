namespace Dashboard.Blazor.Models.Dtos;

public class ProjectsDto
{
    public int Id { get; set; }
    public string NameEn { get; set; } = null!;
    public string NameAr { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public DateTime? CreatedAtLocal
    {
        get
        {
            if (CreatedAt is null)
                return DateTime.Now;

            return CreatedAt?.ToLocalTime();
        }
        set
        {
            CreatedAt = value?.ToUniversalTime();
        }
    }
    public string? Description { get; set; }
    public List<string>? ImagesGallery { get; set; }
    public string? Link { get; set; }
    public int? OrderIndex { get; set; }
    public string Image { get; set; } = null!;
    public IBrowserFile? UploadedImage { get; set; }
}
