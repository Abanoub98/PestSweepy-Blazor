namespace Dashboard.Blazor.Pages.Orders;

public partial class Orders
{
    private List<OrderDto> orders = new();

    private readonly string detailsUri = "Orders/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Orders"], href: null, disabled: true, icon: EntityIcons.OrderIcon),
        });

        orders = await GetAllAsync<OrderDto>("Orders?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private bool FilterFunc(OrderDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;

        return false;
    }


    private async Task AcceptOrder(OrderDto order)
    {
        StartProcessing();

        var result = await UpdateAsync($"Orders/AcceptOrder/{order.Id}", order);

        if (result.isSuccess)
        {
            order.OrderAccepted = true;
            order.OrderState = new() { Name = "Request Accepted" };
        }

        StopProcessing();
    }

    private async Task CancelOrder(OrderDto order)
    {
        StartProcessing();

        var result = await UpdateAsync($"Orders/CancelService/{order.Id}", order);

        if (result.isSuccess)
        {
            order.OrderAccepted = false;
            order.OrderState = new() { Name = "Cancelled" };
        }

        StopProcessing();
    }
}
