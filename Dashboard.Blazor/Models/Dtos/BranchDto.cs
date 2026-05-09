namespace Dashboard.Blazor.Models.Dtos;

public class BranchDto : BaseUser
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Manager { get; set; } = null!;

    // Location Details
    public int CityId { get; set; }
    public LookupDto City { get; set; } = null!;
    public IEnumerable<LookupDto> Cities { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string LocationURL { get; set; } = null!;
    // End Of Location Details
    public int ClientId { get; set; }
    [Required]
    public LookupDto Client { get; set; } = null!;
    public IEnumerable<LookupDto> Clients { get; set; } = null!;
    public string Image { get; set; } = null!;
    public IBrowserFile? UploadedImage { get; set; }
    public string? Notes { get; set; }
}
