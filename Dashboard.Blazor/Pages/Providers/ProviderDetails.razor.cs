namespace Dashboard.Blazor.Pages.Providers;

public partial class ProviderDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ProviderDto? provider;
    private readonly string formUri = "Providers/Form";

    protected override async Task OnParametersSetAsync()
    {
        provider = await GetByIdAsync<ProviderDto>($"Providers/{Id}");

        if (provider is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Providers"], href: "/Providers", icon: Icons.Material.Outlined.Engineering),
            new($"{provider.FirstName} {provider.LastName}", href: null, disabled: true),
        });
    }
    private void UpdateEmail(string newMail)
    {
        provider!.Email = newMail;
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
            { x => x.UserId, provider!.UserId },
            { x => x.Email, provider!.Email },
            { x => x.EmailUpdated,EventCallback.Factory.Create<string>(this, UpdateEmail) },
        };

        await DialogService.ShowAsync<ChangeEmailDialog>(LanguageContainer.Keys["Change Email"], Parameters, dialogOptions);
    }
}
