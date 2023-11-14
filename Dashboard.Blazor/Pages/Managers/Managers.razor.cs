namespace Dashboard.Blazor.Pages.Managers;

public partial class Managers
{
    private List<ManagerDto> managers = new();
    private string searchString = string.Empty;
    private readonly string formUri = "Managers/Form";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["Managers"], href: null, disabled: true, icon: Icons.Material.TwoTone.Person3),
        });

        managers = await GetAllAsync("Managers");

        StopProcessing();
    }

    private bool FilterFunc(ManagerDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
