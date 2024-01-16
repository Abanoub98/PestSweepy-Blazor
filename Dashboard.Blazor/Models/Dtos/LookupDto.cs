namespace Dashboard.Blazor.Models.Dtos;
public class LookupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public int? OrderIndex { get; set; }
}
