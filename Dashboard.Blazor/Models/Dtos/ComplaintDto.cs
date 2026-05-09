namespace Dashboard.Blazor.Models.Dtos;

public class ComplaintDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime CreatedAtLocal { get => CreatedAt.ToLocalTime(); }
    public string Subject { get; set; } = null!;
    public bool IsResolved { get; set; }
    public ClientDto Client { get; set; } = new();
    public int ComplaintTypeId { get; set; }
    public LookupDto ComplaintType { get; set; } = new();
    public IEnumerable<LookupDto> ComplaintTypes { get; set; } = null!;
    public int BranchId { get; set; }
    public BranchBaseDto Branch { get; set; } = null!;
    public IEnumerable<BranchBaseDto> Branches { get; set; } = null!;
    public IBrowserFile? UploadedImage { get; set; }
    public string Image { get; set; } = null!;
}
