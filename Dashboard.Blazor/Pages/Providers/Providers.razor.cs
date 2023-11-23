namespace Dashboard.Blazor.Pages.Providers;

public partial class Providers
{
    [Parameter] public int? SupervisorId { get; set; }

    private List<ProviderDto> providers = new();
    private string searchString = string.Empty;
    private readonly string formUri = "Providers/Form";
    private readonly string detailsUri = "Providers/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Providers"], href: null, disabled: true, icon: Icons.Material.TwoTone.Engineering),
        });

        providers = SupervisorId is not null ?
            await GetAllAsync<ProviderDto>($"Providers?OrderBy=id&Asc=false&FilterQuery={Uri.EscapeDataString($"SupervisorId={SupervisorId}")}") :
            await GetAllAsync<ProviderDto>("Providers?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<ProviderDto>($"Providers/{id}");

        if (isSuccess)
            providers.Remove(providers.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private bool FilterFunc(ProviderDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
