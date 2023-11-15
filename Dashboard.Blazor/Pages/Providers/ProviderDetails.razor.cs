namespace Dashboard.Blazor.Pages.Providers;

public partial class ProviderDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ProviderDto? provider;
    private readonly string formUri = "Providers/Form";

    protected override async Task OnParametersSetAsync()
    {
        provider = await GetByIdAsync($"Providers/{Id}");

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["Providers"], href: "/Providers", icon: Icons.Material.TwoTone.Engineering),
            new BreadcrumbItem(provider.Name, href: null, disabled: true),
        });
    }
}
