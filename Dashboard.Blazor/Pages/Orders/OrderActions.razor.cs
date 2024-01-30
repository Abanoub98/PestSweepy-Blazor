namespace Dashboard.Blazor.Pages.Orders;

public partial class OrderActions
{
    [Parameter][EditorRequired] public OrderDto? Order { get; set; }
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    private DateTime? orderRescheduleDate;
    private ProviderDto? OrderProvider;

    protected override void OnParametersSet()
    {
        if (Order is not null)
        {
            orderRescheduleDate = Order.ReservationDateLocal;
            OrderProvider = Order.Provider;
        }
    }

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
        {
            Order.OrderState = new() { Name = "Assigned to Provider" };
            MudDialog.Close(DialogResult.Ok(true));
        }
        else
        {
            Order.Provider = OrderProvider;
        }

        StopProcessing();
    }

    private async Task RescheduleOrder(int orderId, DateTime? newDate)
    {
        if (newDate is null)
            return;

        StartProcessing();

        var result = await UpdateAsync($"Orders/RescheduleService/{orderId}?newDate={newDate}", Order!);

        if (result.isSuccess)
        {
            Order.OrderState = new() { Name = "Rescheduled" };
            MudDialog.Close(DialogResult.Ok(true));
        }
        else
        {
            Order.ReservationDateLocal = orderRescheduleDate;
        }

        StopProcessing();
    }
}
