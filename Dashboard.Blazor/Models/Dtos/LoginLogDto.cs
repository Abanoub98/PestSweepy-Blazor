namespace Dashboard.Blazor.Models.Dtos;

public class LoginLogDto
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime? LoginDate { get; set; }

    public string Ip { get; set; } = null!;

    public string UserName { get; set; } = null!;
}
