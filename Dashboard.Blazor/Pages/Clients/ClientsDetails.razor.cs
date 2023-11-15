namespace Dashboard.Blazor.Pages.Clients;

public partial class ClientsDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ClientDto? client;
    private readonly string formUri = "Clients/Form";

    protected override async Task OnParametersSetAsync()
    {
        client = await GetByIdAsync($"Clients/{Id}");

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["Clients"], href: "/Clients", icon: Icons.Material.TwoTone.Diversity1),
            new BreadcrumbItem(client.Name, href: null, disabled: true),
        });
    }
}
