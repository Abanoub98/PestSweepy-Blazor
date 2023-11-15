namespace Dashboard.Blazor.Pages.Services;

public partial class ServiceDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ServiceDto? service;
    private readonly string formUri = "Services/Form";

    protected override async Task OnParametersSetAsync()
    {
        service = await GetByIdAsync($"Services/{Id}");

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["Services"], href: "/Services", icon: Icons.Material.TwoTone.Handyman),
            new BreadcrumbItem(service.Name, href: null, disabled: true),
        });
    }
}
