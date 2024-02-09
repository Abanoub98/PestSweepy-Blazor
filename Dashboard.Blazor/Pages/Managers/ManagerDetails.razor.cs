namespace Dashboard.Blazor.Pages.Managers;

public partial class ManagerDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ManagerDto? manager;
    private readonly string formUri = "Managers/Form";

    protected override async Task OnParametersSetAsync()
    {
        manager = await GetByIdAsync<ManagerDto>($"Managers/{Id}");

        if (manager is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Managers"], href: "/Managers", icon: Icons.Material.Outlined.Diversity3),
            new($"{manager.FirstName} {manager.LastName}", href: null, disabled: true),
        });
    }

    private void UpdateEmail(string newMail)
    {
        manager!.Email = newMail;
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
            { x => x.UserId, manager!.UserId },
            { x => x.Email, manager!.Email },
            { x => x.EmailUpdated,EventCallback.Factory.Create<string>(this, UpdateEmail) },
        };

        await DialogService.ShowAsync<ChangeEmailDialog>(LanguageContainer.Keys["Change Email"], Parameters, dialogOptions);
    }
}
