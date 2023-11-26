namespace Dashboard.Blazor.Pages.Payments;

public partial class Payments
{
    private List<PaymentDto> payments = new();
    private string searchString = string.Empty;
    private readonly string detailsUri = "Payments/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();
        StateHasChanged();

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Payments"], href: null, disabled: true, icon: Icons.Material.Filled.Payment),
        };

        payments = await GetAllAsync<PaymentDto>("Payments?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task OpenPaymentDialog()
    {
        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            Position = DialogPosition.Center,
            CloseButton = true
        };

        var dialog = await DialogService.ShowAsync<MoyasarDialog>("Create Payment", dialogOptions);
        await dialog.Result;

        await OnInitializedAsync();
    }

    private bool FilterFunc(PaymentDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Created_at.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Status.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Source.Type.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Amount.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Id.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
