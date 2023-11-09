namespace Dashboard.Blazor.Models.Dtos;

public class TwoFactorAuthDto
{
    public string? QrCodeImage { get; set; }
    public string? SecretKey { get; set; }
    public bool isTwoFactorEnabled { get; set; }
}
