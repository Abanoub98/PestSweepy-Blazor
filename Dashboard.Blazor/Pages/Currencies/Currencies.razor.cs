namespace Dashboard.Blazor.Pages.Currencies;

public partial class Currencies
{
    private List<CurrencyDto> currencies = new();
    private string searchString = string.Empty;
    private readonly string detailsUri = "Currencies/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Currencies"], href: null, disabled: true, icon: EntityIcons.CurrencyIcon),
        });

        currencies = await GetAllAsync<CurrencyDto>("Currencies?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private bool FilterFunc(CurrencyDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Symbol.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Code.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.OrderIndex.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
