namespace Dashboard.Blazor.Pages.Services;

public partial class ServiceForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ServiceDto? serviceForm;
    private int priceNumber;
    private readonly int defaultCurrencyId = 7;

    protected override async Task OnParametersSetAsync()
    {
        if (Id == 0)
        {
            serviceForm = new()
            {
                Prices = new()
                {
                    new ServicePriceType()
                    {
                        CurrencyId = defaultCurrencyId,
                        Currency = await GetByIdAsync<CurrencyDto>($"Currencies/{defaultCurrencyId}")
                    }
                }
            };
        }
        else
        {
            serviceForm = await GetByIdAsync<ServiceDto>($"Services/{Id}");
        }

        if (serviceForm is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Services"], href: "/Services", icon: Icons.Material.TwoTone.Handyman),
            new(languageContainer.Keys[Id == 0 ? "Add Service" : $"Edit {serviceForm.Name}"], href: null, disabled: true),
        });
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        serviceForm!.CategoryId = serviceForm.Category!.Id;

        bool result;
        ServiceDto? serviceDtoResult;

        if (Id == 0)
        {
            (result, serviceDtoResult) = await AddAsync("Services", serviceForm!);
        }
        else
        {

            (result, serviceDtoResult) = await UpdateAsync($"Services/{Id}", serviceForm!);
        }

        if (result)
        {
            if (Id == 0)
                serviceForm!.Id = serviceDtoResult!.Id;

            if (serviceForm.UploadedImage is not null)
                await UploadImage("Services", serviceForm.Id, serviceForm.UploadedImage);

            NavigationManager.NavigateTo("/Services");
        }

        StopProcessing();
    }

    private async Task OpenPriceForm(ServicePriceType? price = null)
    {
        price = price is null ? new() : price;

        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            Position = DialogPosition.Center,
            CloseButton = true
        };

        DialogParameters<ServicePrice> formParameters = new()
        {
            { x => x.Price, price },
            { x => x.DefaultCurrencyId, defaultCurrencyId },
            { x => x.ServiceId, serviceForm!.Id },
            { x => x.IsDefault, (price.Currency?.Id == defaultCurrencyId) },
        };

        var dialog = await DialogService.ShowAsync<ServicePrice>("Service Price", formParameters, dialogOptions);
        var result = await dialog.Result;

        if (result.Canceled)
        {
            StartProcessing();
            serviceForm = await GetByIdAsync<ServiceDto>($"Services/{Id}");
            StopProcessing();
        }
    }

    private async Task AddPrice()
    {
        if (serviceForm!.Id == 0)
            serviceForm!.Prices.Add(new ServicePriceType());
        else
            await OpenPriceForm();
    }

    private async void DeletePrice(ServicePriceType price, int serviceId)
    {
        bool result = true;

        if (serviceId != 0)
            result = await DeleteAsync<ServicePriceType>($"Services/{serviceId}/prices/{price.Id}");

        if (result)
            serviceForm!.Prices.Remove(price);

        StateHasChanged();
    }

    private void CaptureUploadedImage(IBrowserFile image) => serviceForm!.UploadedImage = image;

    private void ClearUploadedImage() => serviceForm!.UploadedImage = null;

    private async Task<IEnumerable<LookupDto>> GetCategories(string value)
    {
        if (serviceForm!.Categories is null)
            serviceForm.Categories = await GetAllLookupsAsync("Categories");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return serviceForm.Categories;

        return serviceForm.Categories.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}
