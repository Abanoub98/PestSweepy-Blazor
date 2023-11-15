namespace Dashboard.Blazor.Pages.Managers;

public partial class Managers
{
    private List<ManagerDto> managers = new();
    private string searchString = string.Empty;

    private readonly string formUri = "Managers/Form";
    private readonly string detailsUri = "Managers/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["Managers"], href: null, disabled: true, icon: Icons.Material.TwoTone.Diversity3),
        });

        managers = await GetAllAsync("Managers");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync($"Managers/{id}");
        if (isSuccess)
        {
            managers.Remove(managers.FirstOrDefault(x => x.Id == id)!);
        }

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
