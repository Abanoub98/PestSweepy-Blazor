namespace Dashboard.Blazor.Pages.ContractClients;

public partial class ContractClientDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ClientDto? contractClient;
    private readonly string formUri = "ContractClients/Form";

    protected override async Task OnParametersSetAsync()
    {
        contractClient = await GetByIdAsync<ClientDto>($"ContractClients/{Id}");

        if (contractClient is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Contract Clients"], href: "/ContractClients", icon: Icons.Material.Outlined.Diversity1),
            new($"{contractClient.FirstName} {contractClient.LastName}", href: null, disabled: true),
        });
    }
}
