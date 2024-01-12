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

            NavigationManager.NavigateTo("/CompanyInfo");
        }

        StopProcessing();
    }

    private void CaptureUploadedImage(IBrowserFile image) => companyInfoForm!.UploadedImage = image;

    private void ClearUploadedImage() => companyInfoForm!.UploadedImage = null;
}
