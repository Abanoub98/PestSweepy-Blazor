namespace Dashboard.Blazor.Pages.CompanyInfo;

public partial class CompanyInfoForm
{
    private CompanyInfoDto? companyInfoForm;

    protected override async Task OnInitializedAsync()
    {
        companyInfoForm = await GetByIdAsync<CompanyInfoDto>($"CompanyInfo/1");

        if (companyInfoForm is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Company Info"], href: "/CompanyInfo", icon: EntityIcons.CompanyInfoIcon),
            new(languageContainer.Keys[$"Edit Company Info"], href: null, disabled: true),
        });
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        var result = await UpdateAsync($"CompanyInfo/1", companyInfoForm!);

        if (result.isSuccess)
        {
            if (companyInfoForm!.UploadedImage is not null)
                await UploadImage("CompanyInfo", companyInfoForm.Id, companyInfoForm.UploadedImage);

            if (companyInfoForm!.HeroSectionUploadedImage is not null)
                await UploadImage("CompanyInfoHeroSection", companyInfoForm.Id, companyInfoForm.HeroSectionUploadedImage);

            if (companyInfoForm!.AboutUsUploadedImage is not null)
                await UploadImage("CompanyInfoAboutUs", companyInfoForm.Id, companyInfoForm.AboutUsUploadedImage);

            if (companyInfoForm!.ContactUsUploadedImage is not null)
                await UploadImage("CompanyInfoContactUs", companyInfoForm.Id, companyInfoForm.ContactUsUploadedImage);

            if (companyInfoForm!.FAQUploadedImage is not null)
                await UploadImage("CompanyInfoFAQSection", companyInfoForm.Id, companyInfoForm.FAQUploadedImage);

            NavigationManager.NavigateTo("/CompanyInfo");
        }

        StopProcessing();
    }

    private void CaptureUploadedImage(IBrowserFile image) => companyInfoForm!.UploadedImage = image;
    private void ClearUploadedImage() => companyInfoForm!.UploadedImage = null;


    private void CaptureHeroImage(IBrowserFile image) => companyInfoForm!.HeroSectionUploadedImage = image;
    private void ClearHeroImage() => companyInfoForm!.HeroSectionUploadedImage = null;

    private void CaptureAboutUsImage(IBrowserFile image) => companyInfoForm!.AboutUsUploadedImage = image;
    private void ClearAboutUsImage() => companyInfoForm!.AboutUsUploadedImage = null;

    private void CaptureContactUsImage(IBrowserFile image) => companyInfoForm!.ContactUsUploadedImage = image;
    private void ClearContactUsImage() => companyInfoForm!.ContactUsUploadedImage = null;

    private void CaptureFAQImage(IBrowserFile image) => companyInfoForm!.FAQUploadedImage = image;
    private void ClearFAQImage() => companyInfoForm!.FAQUploadedImage = null;
}
