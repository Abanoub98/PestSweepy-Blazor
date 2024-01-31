namespace Dashboard.Blazor.Pages.Orders;

public partial class OrderDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private OrderDto? order;

    protected override async Task OnParametersSetAsync()
    {
        order = await GetByIdAsync<OrderDto>($"Orders/{Id}");

        if (order is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Orders"], href: "/Orders", icon: EntityIcons.OrderIcon),
            new(order.OrderNumber, href: null, disabled: true),
        });
    }

    private string CalculateTotal(int quantity, double? servicePrice)
    {
        if (servicePrice is null)
            return string.Empty;

        double total = quantity * (double)servicePrice;

        return total.ToString("F2");
    }

    private string CalculateVatAmount(double vat, int quantity, double? servicePrice)
    {
        if (servicePrice is null)
            return string.Empty;

        double vatAmount = (vat / 100) * (quantity * (double)servicePrice);

        return vatAmount.ToString("F2");
    }

    private async Task ShowOrderActions(OrderDto order)
    {
        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            Position = DialogPosition.TopCenter,
            CloseButton = true
        };

        DialogParameters<OrderActions> Parameters = new()
        {
            { x => x.Order, order }
        };

        var dialog = await DialogService.ShowAsync<OrderActions>(languageContainer.Keys["Order Actions"], Parameters, dialogOptions);

        var result = await dialog.Result;

        if (!result.Canceled)
            StateHasChanged();

    }

    private async Task ShowOrderTracking(OrderDto order)
    {
        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            Position = DialogPosition.TopCenter,
            NoHeader = true,
        };

        DialogParameters<OrderTracking> Parameters = new()
        {
            { x => x.Id, order.Id }
        };

        await DialogService.ShowAsync<OrderTracking>(languageContainer.Keys["Order Tracking"], Parameters, dialogOptions);
    }
}
