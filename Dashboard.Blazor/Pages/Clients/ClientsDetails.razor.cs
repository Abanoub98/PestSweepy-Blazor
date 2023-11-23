namespace Dashboard.Blazor.Pages.Clients;

public partial class ClientsDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ClientDto? client;
    private readonly string formUri = "Clients/Form";

    protected override async Task OnParametersSetAsync()
    {
        client = await GetByIdAsync<ClientDto>($"Clients/{Id}");

        if (client is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Clients"], href: "/Clients", icon: Icons.Material.TwoTone.Diversity1),
            new(client.Name, href: null, disabled: true),
        });
    }
}
