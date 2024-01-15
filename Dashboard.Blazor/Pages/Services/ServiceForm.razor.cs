namespace Dashboard.Blazor.Pages.Services;

public partial class ServiceForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ServiceDto? serviceForm;
    private int optionNumber;

    private List<ServiceOptionPrice> defaultOptionPrices = new();

    protected override async Task OnParametersSetAsync()
    {
        var currencies = await GetAllAsync<CurrencyDto>($"Currencies");
        defaultOptionPrices = currencies.Select(x => new ServiceOptionPrice { Currency = x, CurrencyId = x.Id }).ToList();

        if (Id == 0)
        {
            serviceForm = new()
            {
                ServiceOptions = new() { new ServiceOption() { Prices = CreateDefaultOptionPrices() } }
            };
        }
        else
        {
            serviceForm = await GetByIdAsync<ServiceDto>($"Services/{Id}");

            foreach (var option in serviceForm.ServiceOptions)
            {
                foreach (var defaultPrice in CreateDefaultOptionPrices())
                {
                    if (!option.Prices.Select(p => p.Currency!.Name).ToList().Contains(defaultPrice.Currency!.Name))
                        option.Prices.Add(defaultPrice);
                }

                foreach (var price in option.Prices)
                {
                    if (price.CurrencyId == 0)
                        price.CurrencyId = price.Currency!.Id;
                }
            }
        }

        if (serviceForm is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Services"], href: "/Services", icon: Icons.Material.Outlined.Handyman),
            new(languageContainer.Keys[Id == 0 ? "Add Service" : $"Edit {serviceForm.NameEn} - {serviceForm.NameAr}"], href: null, disabled: true),
        });
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        foreach (var option in serviceForm!.ServiceOptions)
        {
            option.Prices = option.Prices.Where(p => p.Amount is not null || p.Amount > 0).ToList();
        }

        serviceForm!.ServiceOptions = serviceForm.ServiceOptions.Where(o => o.Prices.Any()).ToList();

        serviceForm.CategoryId = serviceForm.Category!.Id;

        var result = Id == 0 ?
            await AddAsync("Services", serviceForm) :
            await UpdateAsync($"Services/{Id}", serviceForm);

        if (result.isSuccess)
        {
            if (Id == 0)
                serviceForm!.Id = result.obj!.Id;

            if (serviceForm.UploadedImage is not null)
                await UploadImage("Services", serviceForm.Id, serviceForm.UploadedImage);

            NavigationManager.NavigateTo("/Services");
        }

        StopProcessing();
    }

    private void AddOption()
    {
        serviceForm!.ServiceOptions.Add(new ServiceOption()
        {
            Prices = CreateDefaultOptionPrices()
        });
    }

    private void DeleteOption(ServiceOption option) => serviceForm!.ServiceOptions.Remove(option);

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

    private List<ServiceOptionPrice> CreateDefaultOptionPrices()
    {
        var optionPricesList = defaultOptionPrices.Select(d => new ServiceOptionPrice()
        {
            Currency = d.Currency,
            CurrencyId = d.CurrencyId
        }).ToList();

        return optionPricesList;
    }
}
