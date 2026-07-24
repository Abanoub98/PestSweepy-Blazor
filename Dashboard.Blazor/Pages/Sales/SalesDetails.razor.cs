namespace Dashboard.Blazor.Pages.Sales;

public partial class SalesDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private SalesDto? sales;
    private readonly string formUri = "Sales/Form";

    protected override async Task OnParametersSetAsync()
    {
        sales = await GetByIdAsync<SalesDto>($"Sales/{Id}");

        if (sales is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Sales"], href: "/Sales", icon: Icons.Material.Outlined.Diversity3),
            new($"{sales.FirstName} {sales.LastName}", href: null, disabled: true),
        });
    }

    private void UpdateEmail(string newMail)
    {
        sales!.Email = newMail;
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
            { x => x.UserId, sales!.UserId },
            { x => x.Email, sales!.Email },
            { x => x.EmailUpdated,EventCallback.Factory.Create<string>(this, UpdateEmail) },
        };

        await DialogService.ShowAsync<ChangeEmailDialog>(LanguageContainer.Keys["Change Email"], Parameters, dialogOptions);
    }
}
