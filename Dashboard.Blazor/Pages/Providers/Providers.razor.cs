namespace Dashboard.Blazor.Pages.Providers;

public partial class Providers
{
    [Parameter] public int? SupervisorId { get; set; }

    private List<ProviderDto> providers = new();

    private readonly string formUri = "Providers/Form";
    private readonly string detailsUri = "Providers/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Providers"], href: null, disabled: true, icon: Icons.Material.Outlined.Engineering),
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
        {
            providers.Remove(providers.FirstOrDefault(x => x.Id == id)!);

            if (selectedIds.Contains(id))
                selectedIds.Remove(id);
        }

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<ProviderDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<ProviderDto>($"Providers/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            providers.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }

    private bool FilterFunc(ProviderDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
