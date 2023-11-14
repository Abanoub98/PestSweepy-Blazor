namespace Dashboard.Blazor.Pages.Clients;

public partial class Clients
{
    private List<ClientDto> clients = new();
    private string searchString = string.Empty;
    private readonly string formUri = "Clients/Form";
    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["Clients"], href: null, disabled: true, icon: Icons.Material.TwoTone.Groups),
        });

        clients = await GetAllAsync("Clients");

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
