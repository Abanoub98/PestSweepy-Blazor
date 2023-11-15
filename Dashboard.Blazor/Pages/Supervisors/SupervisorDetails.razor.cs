namespace Dashboard.Blazor.Pages.Supervisors;

public partial class SupervisorDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private SupervisorDto? supervisor;
    private readonly string formUri = "Supervisors/Form";

    protected override async Task OnParametersSetAsync()
    {
        supervisor = await GetByIdAsync($"Supervisors/{Id}");

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["Supervisors"], href: "/Supervisors", icon: Icons.Material.TwoTone.Diversity1),
            new BreadcrumbItem(supervisor.Name, href: null, disabled: true),
        });
    }
}

