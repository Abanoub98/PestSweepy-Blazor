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

    // About Us
    public int YearsOfExp { get; set; }
    public string? AboutUsTitle { get; set; }
    public string? AboutUsSubtitle { get; set; }
    public string? AboutUsDescription { get; set; }
    public string? AboutUsImage { get; set; } = string.Empty;
    public IBrowserFile? AboutUsUploadedImage { get; set; }
    public string? ProgressBarTitle1 { get; set; }
    public int ProgressBarValue1 { get; set; }
    public string? ProgressBarTitle2 { get; set; }
    public int ProgressBarValue2 { get; set; }
    public string? ProgressBarTitle3 { get; set; }
    public int ProgressBarValue3 { get; set; }


    // Contact Us
    public string? ContactUsTitle { get; set; }
    public string? ContactUsSubtitle { get; set; }
    public string? ContactUsDescription { get; set; }
    public string? ContactUsImage { get; set; } = string.Empty;
    public IBrowserFile? ContactUsUploadedImage { get; set; }

    // Categories
    public string? CategoriesTitle { get; set; }
    public string? CategoriesSubtitle { get; set; }
    public string? CategoriesDescription { get; set; }

    // Projects
    public string? ProjectsTitle { get; set; }
    public string? ProjectsSubtitle { get; set; }
    public string? ProjectsDescription { get; set; }

    // FAQ
    public string? FAQTitle { get; set; }
    public string? FAQSubtitle { get; set; }
    public string? FAQDescription { get; set; }
    public string? FAQSectionImage { get; set; } = string.Empty;
    public IBrowserFile? FAQUploadedImage { get; set; }

    // Team
    public string? TeamTitle { get; set; }
    public string? TeamSubtitle { get; set; }
    public string? TeamDescription { get; set; }

    // Testimonial
    public string? TestimonialTitle { get; set; }
    public string? TestimonialSubtitle { get; set; }
    public string? TestimonialDescription { get; set; }

    // Blog
    public string? BlogTitle { get; set; }
    public string? BlogSubtitle { get; set; }
    public string? BlogDescription { get; set; }


    // Hero Section
    public string? HeroSectionTitle { get; set; }
    public string? HeroSectionSubtitle { get; set; }
    public string? HeroSectionDescription { get; set; }

    public string? HeroSectionFeature1 { get; set; }
    public string? HeroSectionFeature2 { get; set; }

    public string? HeroSectionService1Title { get; set; }
    public string? HeroSectionService2Title { get; set; }
    public string? HeroSectionService3Title { get; set; }
    public string? HeroSectionService4Title { get; set; }
    public string? HeroSectionImage { get; set; } = string.Empty;
    public IBrowserFile? HeroSectionUploadedImage { get; set; }

    //Counters
    public string? CounterTitle1 { get; set; }
    public int CounterValue1 { get; set; }

    public string? CounterTitle2 { get; set; }
    public int CounterValue2 { get; set; }

    public string? CounterTitle3 { get; set; }
    public int CounterValue3 { get; set; }

    public string? CounterTitle4 { get; set; }
    public int CounterValue4 { get; set; }
}
