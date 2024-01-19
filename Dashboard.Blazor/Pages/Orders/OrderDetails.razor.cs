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
}
