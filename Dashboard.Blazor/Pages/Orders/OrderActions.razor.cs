namespace Dashboard.Blazor.Pages.Orders;

public partial class OrderActions
{
    [Parameter][EditorRequired] public OrderDto? Order { get; set; }
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    private async Task<IEnumerable<ProviderDto>> GetProviders(string value)
    {
        if (Order!.Providers is null)
            Order.Providers = await GetAllAsync<ProviderDto>("Providers");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return Order.Providers;

        return Order.Providers.Where(x => x.FirstName.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.LastName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task AssignProvider(int orderId, ProviderDto? provider)
    {
        if (provider is null)
            return;

        StartProcessing();

        var result = await UpdateAsync($"Orders/AssignProvider/{orderId}?providerId={provider.Id}", Order!);

        if (result.isSuccess)
            MudDialog.Close(DialogResult.Ok(true));
        else
            provider = null;

        StopProcessing();
    }
}
