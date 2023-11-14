namespace Dashboard.Blazor.Pages.Services;

public partial class Services
{
    private List<ServiceDto> services = new();
    private string searchString = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["Services"], href: null, disabled: true, icon: Icons.Material.TwoTone.MiscellaneousServices),
        });

        services = await GetAllAsync("Services");

        StopProcessing();
    }

    private bool FilterFunc(ServiceDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.DurationInMinutes.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.OrderIndex.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
