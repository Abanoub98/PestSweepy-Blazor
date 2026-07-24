namespace Dashboard.Blazor.Pages.Sales;

public partial class Sales
{
    private List<SalesDto> sales = new();

    private readonly string formUri = "Sales/Form";
    private readonly string detailsUri = "Sales/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Sales"], href: null, disabled: true, icon: Icons.Material.Outlined.Diversity3),
        };

        sales = await GetAllAsync<SalesDto>("Sales?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<SalesDto>($"Sales/{id}");

        if (isSuccess)
        {
            sales.Remove(sales.FirstOrDefault(x => x.Id == id)!);

            if (selectedIds.Contains(id))
                selectedIds.Remove(id);
        }

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<SalesDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<SalesDto>($"Sales/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            sales.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }

    private bool FilterFunc(SalesDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
