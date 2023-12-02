namespace Dashboard.Blazor.Pages.Services;

public partial class ServicePrice
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;
    [Parameter][EditorRequired] public ServicePriceType Price { get; set; } = null!;
    [Parameter][EditorRequired] public int DefaultCurrencyId { get; set; }
    [Parameter] public bool IsReadOnly { get; set; }
    [Parameter] public int? ServiceId { get; set; }
    [Parameter] public bool IsDefault { get; set; }

    private ServicePriceType servicePrice = new();


    protected override void OnParametersSet()
    {
        servicePrice = Price;
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        bool result;
        ServicePriceType? servicePriceDtoResult;

        if (Price.Id == 0)
        {
            (result, servicePriceDtoResult) = await AddAsync($"Services/{ServiceId}/prices", servicePrice);
        }
        else
        {
            (result, servicePriceDtoResult) = await UpdateAsync($"Services/{ServiceId}/prices/{Price!.Id}", servicePrice);
        }

        if (result)
        {
            MudDialog.Close(DialogResult.Ok(servicePrice));
        }

        StopProcessing();
    }

    private void SetCurrencyId()
    {
        servicePrice.CurrencyId = servicePrice.Currency?.Id ?? 0;
    }

    private async Task<IEnumerable<CurrencyDto>> GetCurrencies(string value)
    {
        if (servicePrice!.Currencies is null)
            servicePrice.Currencies = await GetAllAsync<CurrencyDto>($"Currencies{(IsDefault ? $"?FilterQuery=Id%3D{DefaultCurrencyId}" : $"?FilterQuery=Id%21%3D{DefaultCurrencyId}")}");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return servicePrice.Currencies;

        return servicePrice.Currencies.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}
