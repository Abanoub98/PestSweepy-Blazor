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

    private async Task ShowOrderActions(OrderDto order)
    {
        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            Position = DialogPosition.TopCenter,
            CloseButton = true
        };

        DialogParameters<OrderActions> Parameters = new()
        {
            { x => x.Order, order }
        };

        await DialogService.ShowAsync<OrderActions>("Order Actions", Parameters, dialogOptions);
    }

    private bool FilterFunc(OrderDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;

        return false;
    }
}
