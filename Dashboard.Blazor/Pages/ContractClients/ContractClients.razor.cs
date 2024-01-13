namespace Dashboard.Blazor.Pages.ContractClients;

public partial class ContractClients
{
    private List<ClientDto> contractClients = new();

    private readonly string formUri = "ContractClients/Form";
    private readonly string detailsUri = "ContractClients/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Contract Clients"], href: null, disabled: true, icon: Icons.Material.Outlined.Diversity1),
        });

        contractClients = await GetAllAsync<ClientDto>("ContractClients?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<ClientDto>($"ContractClients/{id}");

        if (isSuccess)
            contractClients.Remove(contractClients.FirstOrDefault(x => x.Id == id)!);

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
