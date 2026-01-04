namespace Dashboard.Blazor.Pages.Partners;

public partial class Partners
{
    private List<PartnersDto> partners = new();

    private readonly string formUri = "Partners/Form";
    private readonly string detailsUri = "Partners/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Partners"], href: null, disabled: true, icon: EntityIcons.CategoriesIcon),
        };

        partners = await GetAllAsync<PartnersDto>("Partners?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<PartnersDto>($"Partners/{id}");

        if (isSuccess)
            partners.Remove(partners.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<PartnersDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<PartnersDto>($"Partners/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            partners.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }
    private bool FilterFunc(PartnersDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.NameAr.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.NameEn.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.OrderIndex.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
