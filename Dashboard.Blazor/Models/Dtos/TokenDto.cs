namespace Dashboard.Blazor.Models.Dtos;

public class TokenDto
{
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public string RefreshTokenExpiration { get; set; } = null!;
}
