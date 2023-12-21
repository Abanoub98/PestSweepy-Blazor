namespace Dashboard.Blazor.Pages.Managers;

public partial class Managers
{
    private List<ManagerDto> managers = new();

    private readonly string formUri = "Managers/Form";
    private readonly string detailsUri = "Managers/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Managers"], href: null, disabled: true, icon: Icons.Material.TwoTone.Diversity3),
        });

        managers = await GetAllAsync<ManagerDto>("Managers?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<ManagerDto>($"Managers/{id}");
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
