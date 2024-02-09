namespace Dashboard.Blazor.Pages.Clients;

public partial class ClientsDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ClientDto? client;
    private readonly string formUri = "IndividualClients/Form";

    protected override async Task OnParametersSetAsync()
    {
        client = await GetByIdAsync<ClientDto>($"Clients/{Id}");

        if (client is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Individual Clients"], href: "/IndividualClients", icon: EntityIcons.ClientsIcon),
            new($"{client.FirstName} {client.LastName}", href: null, disabled: true),
        });
    }

    private void UpdateEmail(string newMail)
    {
        client!.Email = newMail;
    }

    public async Task ShowChangeEmailAsync()
    {
        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            Position = DialogPosition.Center,
            CloseButton = true
        };

        DialogParameters<ChangeEmailDialog> Parameters = new()
        {
            { x => x.UserId, client!.UserId },
            { x => x.Email, client!.Email },
            { x => x.EmailUpdated,EventCallback.Factory.Create<string>(this, UpdateEmail) },
        };

        await DialogService.ShowAsync<ChangeEmailDialog>(LanguageContainer.Keys["Change Email"], Parameters, dialogOptions);
    }
}
