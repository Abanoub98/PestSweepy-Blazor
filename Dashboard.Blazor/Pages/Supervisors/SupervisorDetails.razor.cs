namespace Dashboard.Blazor.Pages.Supervisors;

public partial class SupervisorDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private SupervisorDto? supervisor;
    private readonly string formUri = "Supervisors/Form";

    protected override async Task OnParametersSetAsync()
    {
        supervisor = await GetByIdAsync<SupervisorDto>($"Supervisors/{Id}");

        if (supervisor is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Supervisors"], href: "/Supervisors", icon: Icons.Material.Outlined.SupervisorAccount),
            new($"{supervisor.FirstName} {supervisor.LastName}", href: null, disabled: true),
        });
    }

    private void UpdateEmail(string newMail)
    {
        supervisor!.Email = newMail;
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
            { x => x.UserId, supervisor!.UserId },
            { x => x.Email, supervisor!.Email },
            { x => x.EmailUpdated,EventCallback.Factory.Create<string>(this, UpdateEmail) },
        };

        await DialogService.ShowAsync<ChangeEmailDialog>(LanguageContainer.Keys["Change Email"], Parameters, dialogOptions);
    }
}

