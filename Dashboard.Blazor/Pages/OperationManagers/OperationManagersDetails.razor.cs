namespace Dashboard.Blazor.Pages.OperationManagers;

public partial class OperationManagersDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private OperationManagerDto? operationManager;
    private readonly string formUri = "OperationManagers/Form";

    protected override async Task OnParametersSetAsync()
    {
        operationManager = await GetByIdAsync<OperationManagerDto>($"OperationManagers/{Id}");

        if (operationManager is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["OperationManagers"], href: "/OperationManagers", icon: Icons.Material.Outlined.Diversity3),
            new($"{operationManager.FirstName} {operationManager.LastName}", href: null, disabled: true),
        });
    }

    private void UpdateEmail(string newMail)
    {
        operationManager!.Email = newMail;
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
            { x => x.UserId, operationManager!.UserId },
            { x => x.Email, operationManager!.Email },
            { x => x.EmailUpdated,EventCallback.Factory.Create<string>(this, UpdateEmail) },
        };

        await DialogService.ShowAsync<ChangeEmailDialog>(LanguageContainer.Keys["Change Email"], Parameters, dialogOptions);
    }
}
