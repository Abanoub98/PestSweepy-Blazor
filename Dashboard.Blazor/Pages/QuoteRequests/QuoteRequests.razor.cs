namespace Dashboard.Blazor.Pages.QuoteRequests;

public partial class QuoteRequests
{
    private List<QuoteRequestsDto> quoteRequests = new();

    private readonly string formUri = "QuoteRequests/Form";
    private readonly string detailsUri = "QuoteRequests/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["QuoteRequests"], href: null, disabled: true, icon: EntityIcons.CategoriesIcon),
        };

        quoteRequests = await GetAllAsync<QuoteRequestsDto>("QuoteRequests?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<QuoteRequestsDto>($"QuoteRequests/{id}");

        if (isSuccess)
            quoteRequests.Remove(quoteRequests.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private async Task ToggleStatus(QuoteRequestsDto quote)
    {
        StartProcessing();

        var isSuccess = await ShowConfirmation($"Are you sure that you will complete this Quote Request", true);

        if (isSuccess)
        {
            var result = await UpdateAsync($"QuoteRequests/CompleteQuoteState/{quote.Id}", quote);
        }

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<QuoteRequestsDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<QuoteRequestsDto>($"QuoteRequests/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            quoteRequests.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }
    private bool FilterFunc(QuoteRequestsDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Service.NameEn.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Phone.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
