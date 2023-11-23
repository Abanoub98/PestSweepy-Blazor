namespace Dashboard.Blazor.Pages.Supervisors;

public partial class SupervisorDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private SupervisorDto? supervisor;
    private readonly string formUri = "Supervisors/Form";

    protected override async Task OnParametersSetAsync()
    {
        supervisor = await GetByIdAsync<SupervisorDto>($"Supervisors/{Id}");

        if (supervisor is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Supervisors"], href: "/Supervisors", icon: Icons.Material.TwoTone.SupervisorAccount),
            new(supervisor.Name, href: null, disabled: true),
        });
    }
}

