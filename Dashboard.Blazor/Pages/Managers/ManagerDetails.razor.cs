namespace Dashboard.Blazor.Pages.Managers;

public partial class ManagerDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ManagerDto? manager;
    private readonly string formUri = "Managers/Form";

    protected override async Task OnParametersSetAsync()
    {
        manager = await GetByIdAsync<ManagerDto>($"Managers/{Id}");

        if (manager is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["Managers"], href: "/Managers", icon: Icons.Material.TwoTone.Diversity3),
            new BreadcrumbItem(manager.Name, href: null, disabled: true),
        });
    }
}
