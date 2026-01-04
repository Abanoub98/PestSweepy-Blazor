namespace Dashboard.Blazor.Models.Dtos;

public class OurTeamDto
{
    public int Id { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string Title { get; set; } = null!;

    public int? YearsOfExperience { get; set; }
    public int? OrderIndex { get; set; }
    public string Image { get; set; } = null!;
    public IBrowserFile? UploadedImage { get; set; }
}
