namespace Dashboard.Blazor.Pages.Branches;

public partial class BranchesDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private BranchDto? branch;
    private readonly string formUri = "Branches/Form";

    protected override async Task OnParametersSetAsync()
    {
        branch = await GetByIdAsync<BranchDto>($"Branches/{Id}");

        if (branch is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Branches"], href: "/Branches", icon: Icons.Material.Outlined.Diversity3),
            new($"{branch.Name} {branch.PhoneNumber}", href: null, disabled: true),
        });
    }

    private void UpdateEmail(string newMail)
    {
        branch!.Email = newMail;
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
            { x => x.UserId, branch!.UserId },
            { x => x.Email, branch!.Email },
            { x => x.EmailUpdated,EventCallback.Factory.Create<string>(this, UpdateEmail) },
        };

        await DialogService.ShowAsync<ChangeEmailDialog>(LanguageContainer.Keys["Change Email"], Parameters, dialogOptions);
    }
}
