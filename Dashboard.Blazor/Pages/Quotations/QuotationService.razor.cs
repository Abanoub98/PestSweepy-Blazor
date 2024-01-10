namespace Dashboard.Blazor.Pages.Quotations;

public partial class QuotationService
{
    [Parameter][EditorRequired] public QuotationServiceType? Service { get; set; }
    [Parameter] public CurrencyDto? Currency { get; set; }
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

    private async Task<IEnumerable<LookupDto>> GetTypes(string value)
    {
        if (quotationService!.Types is null)
            quotationService.Types = await GetAllLookupsAsync("ReferenceData?tableName=Types");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return quotationService.Types;

        return quotationService.Types.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task<IEnumerable<LookupDto>> GetUnits(string value)
    {
        if (quotationService!.Units is null)
            quotationService.Units = await GetAllLookupsAsync("ReferenceData?tableName=Units");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return quotationService.Units;

        return quotationService.Units.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
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

    private void CalculateTotalPrice()
    {
        if (quotationService!.Type is null)
            return;

        quotationService!.DiscountPrice = quotationService.DiscountRate < 100 ? quotationService.Price * (quotationService.DiscountRate / 100) : 0;

        var variable = quotationService.Type.Name == "Area" ? quotationService.Area : quotationService.Quantity;
        quotationService!.TotalPrice = (quotationService.Price - quotationService.DiscountPrice) * variable;
    }

    private async Task CallBackTotalValue()
    {
        await TotalValueChanged.InvokeAsync();
    }
}
