namespace Dashboard.Blazor.Models.Dtos;

public class PartnersDto
{
    public int Id { get; set; }
    public string NameEn { get; set; } = null!;
    public string NameAr { get; set; } = null!;
    public int? OrderIndex { get; set; }
    public string Image { get; set; } = null!;
    public IBrowserFile? UploadedImage { get; set; }
}
