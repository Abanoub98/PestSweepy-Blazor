namespace Dashboard.Blazor.Pages.Clients;

public partial class Clients
{
    private List<ClientDto> clients = new();

    private readonly string formUri = "Clients/Form";
    private readonly string detailsUri = "Clients/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Clients"], href: null, disabled: true, icon: Icons.Material.Outlined.Diversity1),
        });

        clients = await GetAllAsync<ClientDto>("Clients?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<ClientDto>($"Clients/{id}");

        if (isSuccess)
            clients.Remove(clients.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<ClientDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<ClientDto>($"Clients/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            clients.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }


        StopProcessing();
    }

    private bool FilterFunc(ClientDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
