
namespace Dashboard.Blazor.Pages.Quotations;

public partial class Quotations
{
    private List<QuotationDto> quotations = new();

    private readonly string formUri = "Quotations/Form";
    private readonly string detailsUri = "Quotations/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Price Quotations"], href: null, disabled: true, icon: EntityIcons.QuotationsIcon),
        };

        quotations = await GetAllAsync<QuotationDto>("Quotations?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task ShowQuotation(int id)
    {
        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            Position = DialogPosition.Center,
            CloseButton = true
        };

        DialogParameters<QuotationReport> Parameters = new()
        {
            { x => x.Id, id }
        };

        await DialogService.ShowAsync<QuotationReport>("Quotation Report", Parameters, dialogOptions);
    }
    private bool FilterFunc(QuotationDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.CreatedAtLocal.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.SerialNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.ClientName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
