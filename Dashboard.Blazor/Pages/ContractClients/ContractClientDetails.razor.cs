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

    private void UpdateEmail(string newMail)
    {
        contractClient!.Email = newMail;
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
            { x => x.UserId, contractClient!.UserId },
            { x => x.Email, contractClient!.Email },
            { x => x.EmailUpdated,EventCallback.Factory.Create<string>(this, UpdateEmail) },
        };

        await DialogService.ShowAsync<ChangeEmailDialog>(LanguageContainer.Keys["Change Email"], Parameters, dialogOptions);
    }
}
