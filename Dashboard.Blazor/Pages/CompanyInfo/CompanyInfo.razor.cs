namespace Dashboard.Blazor.Pages.CompanyInfo;

public partial class CompanyInfo
{
    private CompanyInfoDto? companyInfo;

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Company Info"], href: null, disabled: true, icon: EntityIcons.CompanyInfoIcon),
        });

        companyInfo = await GetByIdAsync<CompanyInfoDto>("CompanyInfo/1");

        StopProcessing();
    }
}
