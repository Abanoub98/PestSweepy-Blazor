namespace Dashboard.Blazor.Models.Dtos;

public class CurrencyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Symbol { get; set; } = null!;
    public int? OrderIndex { get; set; }
    public int CountryId { get; set; }
    public bool? IsDefault { get; set; }
    public string Image { get; set; } = null!;
}
