namespace Dashboard.Blazor.Pages.Payments;

public partial class Payments
{
    private List<PaymentBaseDto> payments = new();
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

        payments = await GetAllAsync<PaymentBaseDto>("Payments?OrderBy=CreatedAt&Asc=false");

        StopProcessing();
    }

    private async Task RefundPayment(decimal amount, string paymentId)
    {
        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            Position = DialogPosition.TopCenter,
            CloseButton = true
        };

        DialogParameters<RefundDialog> formParameters = new()
        {
            { x => x.Amount, amount },
            { x => x.PaymentId, paymentId }
        };

        var dialog = await DialogService.ShowAsync<RefundDialog>("Refund Payment", formParameters, dialogOptions);
        var result = await dialog.Result;

        if (!result.Canceled)
            await OnInitializedAsync();

    }

    private async Task VoidPayment(string paymentId)
    {
        StartProcessing();

        var isConfirmed = await ShowConfirmation("You want to avoid this payment?");

        if (isConfirmed)
        {
            var result = await PostAsync<PaymentDto>($"/Payments/VoidPayment/{paymentId}");

            if (result.isSucces)
                await OnInitializedAsync();
        }

        StopProcessing();
    }

    private bool FilterFunc(PaymentBaseDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.CreatedAt.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Status.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Company.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Amount.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Id.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
