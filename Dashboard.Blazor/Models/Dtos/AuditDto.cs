namespace Dashboard.Blazor.Models.Dtos;

public class AuditDto
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string TableName { get; set; } = null!;

    public DateTime DateTime { get; set; }

    public DateTime DateTimeLocal { get => DateTime.ToLocalTime(); }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public string AffectedColumns { get; set; } = null!;

    public string PrimaryKey { get; set; } = null!;
}
