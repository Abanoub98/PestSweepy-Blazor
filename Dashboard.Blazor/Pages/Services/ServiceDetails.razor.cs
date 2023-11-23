namespace Dashboard.Blazor.Pages.Services;

public partial class ServiceDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ServiceDto? service;
    private readonly string formUri = "Services/Form";

    protected override async Task OnParametersSetAsync()
    {
        service = await GetByIdAsync<ServiceDto>($"Services/{Id}");

        if (service is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Services"], href: "/Services", icon: Icons.Material.TwoTone.Handyman),
            new(service.Name, href: null, disabled: true),
        });
    }
}
