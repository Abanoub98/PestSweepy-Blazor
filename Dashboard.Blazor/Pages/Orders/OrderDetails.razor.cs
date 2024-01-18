namespace Dashboard.Blazor.Pages.Orders;

public partial class OrderDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private OrderDto? order;
    private readonly string formUri = "Orders/Form";

    protected override async Task OnParametersSetAsync()
    {
        order = new();

        if (order is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Orders"], href: "/Orders", icon: EntityIcons.OrderIcon),
            new(order.Id.ToString(), href: null, disabled: true),
        });
    }
}
