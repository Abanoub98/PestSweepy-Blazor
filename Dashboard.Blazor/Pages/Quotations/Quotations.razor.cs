namespace Dashboard.Blazor.Pages.Quotations;

public partial class Quotations
{
    private List<QuotationDto> quotations = new();

    private readonly string formUri = "Quotations/Form";
    private readonly string detailsUri = "Quotations/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Price Quotations"], href: null, disabled: true, icon: EntityIcons.QuotationsIcon),
        });

        quotations = await GetAllAsync<QuotationDto>("Quotations?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<QuotationDto>($"Quotations/{id}");

        if (isSuccess)
            quotations.Remove(quotations.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private bool FilterFunc(QuotationDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.CreatedAt.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.SerialNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.ClientName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
