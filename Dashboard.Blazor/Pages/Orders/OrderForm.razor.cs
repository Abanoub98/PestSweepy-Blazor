namespace Dashboard.Blazor.Pages.Orders;

partial class OrderForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private OrderDto? orderForm;

    protected override async Task OnParametersSetAsync()
    {
        orderForm = (Id == 0) ? new() : await GetByIdAsync<OrderDto>($"Orders/{Id}");

        if (orderForm is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Orders"], href: "/Orders", icon: EntityIcons.OrderIcon),
            new(languageContainer.Keys[Id == 0 ? "Add Order" : $"Edit {orderForm.OrderNumber}"], href: null, disabled: true),
        });
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        orderForm!.ServiceOptionId = orderForm.ServiceOption!.Id;
        orderForm!.ClientId = orderForm.Client!.Id;

        bool result;
        OrderDto? orderDtoResult;

        if (Id == 0)
            (result, orderDtoResult) = await AddAsync("Orders/CreateCashOrder", orderForm!);
        else
            (result, orderDtoResult) = await UpdateAsync($"Orders/{Id}", orderForm!);

        StopProcessing();

        if (result)
            NavigationManager.NavigateTo("/Orders");
    }

    private async Task<IEnumerable<LookupDto>> GetCategories(string value)
    {
        if (orderForm!.Categories is null)
            orderForm.Categories = await GetAllLookupsAsync("Categories");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return orderForm.Categories;

        return orderForm.Categories.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task<IEnumerable<ClientDto>> GetClients(string value)
    {
        if (orderForm!.Clients is null)
            orderForm.Clients = await GetAllAsync<ClientDto>("ContractClients");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return orderForm.Clients;

        return orderForm.Clients.Where(x =>
        x.FirstName.Contains(value, StringComparison.InvariantCultureIgnoreCase) ||
        x.LastName.Contains(value, StringComparison.InvariantCultureIgnoreCase) ||
        x.PhoneNumber.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }


    private async Task<IEnumerable<ServiceDto>?> GetServices(string value)
    {
        if (orderForm!.Category is null)
            return null;

        orderForm.Services = await GetAllAsync<ServiceDto>($"Services?FilterQuery=CategoryId={orderForm.Category.Id}");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return orderForm.Services;

        return orderForm.Services.Where(x =>
            x.NameAr.Contains(value, StringComparison.InvariantCultureIgnoreCase) ||
            x.NameEn.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task<IEnumerable<ServiceOption>?> GetServiceOptions(string value)
    {
        if (orderForm!.Service is null)
            return null;

        var service = await GetByIdAsync<ServiceDto>($"Services/{orderForm.Service.Id}");

        orderForm.ServiceOptions = service.ServiceOptions;

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return orderForm.ServiceOptions;

        return orderForm.ServiceOptions.Where(x =>
            x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}
