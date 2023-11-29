namespace Dashboard.Blazor.Models.Dtos;

public class CompanyInfoDto
{

    [Required]
    public string? Address { get; set; }

    [Required]
    public string? ContactEmail { get; set; }

    [Required]
    public string? ContactMobile { get; set; }

    public string? Hotline { get; set; }

    [Required]
    public string? VatRegistrationNumber { get; set; }

    [Required]
    public string? ContactPhone { get; set; }


    public string? FacebookUrl { get; set; }

    public string? TwitterUrl { get; set; }

    public string? LinkedInUrl { get; set; }

    public string? InstagramUrl { get; set; }

    public string? TelegramUrl { get; set; }

    public string? YoutubeUrl { get; set; }


    [Required]
    public string? WhoWeAre { get; set; }

    [Required]
    public string? Mission { get; set; }

    [Required]
    public string? Vision { get; set; }


    public int Id { get; set; }

    public string? Image { get; set; }
    public IBrowserFile? UploadedImage { get; set; }
}
