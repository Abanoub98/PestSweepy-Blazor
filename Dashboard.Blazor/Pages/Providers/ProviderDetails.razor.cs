namespace Dashboard.Blazor.Pages.Providers;

public partial class ProviderDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ProviderDto? provider;
    private readonly string formUri = "Providers/Form";

    protected override async Task OnParametersSetAsync()
    {
        provider = await GetByIdAsync<ProviderDto>($"Providers/{Id}");

        if (provider is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Providers"], href: "/Providers", icon: Icons.Material.Outlined.Engineering),
            new(provider.Name, href: null, disabled: true),
        });
    }
}
