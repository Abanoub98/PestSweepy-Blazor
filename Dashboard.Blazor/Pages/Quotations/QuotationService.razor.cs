namespace Dashboard.Blazor.Pages.Quotations;

public partial class QuotationService
{
    [Parameter][EditorRequired] public QuotationServiceType? Service { get; set; }
    [Parameter] public EventCallback TotalValueChanged { get; set; }
    [Parameter] public bool IsReadOnly { get; set; }

    private QuotationServiceType? quotationService;

    protected override void OnParametersSet()
    {
        quotationService = Service;
    }

    private async Task<IEnumerable<LookupDto>> GetCategories(string value)
    {
        if (quotationService!.Categories is null)
            quotationService.Categories = await GetAllLookupsAsync("Categories");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return quotationService.Categories;

        return quotationService.Categories.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task<IEnumerable<ServiceDto>?> GetServices(string value)
    {
        if (quotationService!.Category is null)
            return null;

        quotationService.Services = await GetAllAsync<ServiceDto>($"Services?FilterQuery=CategoryId={quotationService.Category.Id}");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return quotationService.Services;

        return quotationService.Services.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private void CalculateDiscountPrice()
    {
        quotationService!.DiscountPrice = quotationService.DiscountRate < 100 ?
            quotationService.Price * (quotationService.DiscountRate / 100) : 0;

        CalculateTotalPrice();
    }

    private void CalculateTotalPrice()
    {
        quotationService!.TotalPrice = (quotationService.Price - quotationService.DiscountPrice) * quotationService.Count;
    }

    private void GetServicePrice(ServiceDto? service)
    {
        quotationService!.Service = service;
        quotationService.Price = quotationService.Service?.Price ?? 1500;
    }

    private async Task CallBackTotalValue()
    {
        await TotalValueChanged.InvokeAsync();
    }
}
