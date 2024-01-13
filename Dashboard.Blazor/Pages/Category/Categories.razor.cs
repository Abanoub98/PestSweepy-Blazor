namespace Dashboard.Blazor.Pages.Category;

public partial class Categories
{
    private List<CategoryDto> categories = new();

    private readonly string formUri = "Categories/Form";
    private readonly string detailsUri = "Categories/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Categories"], href: null, disabled: true, icon: EntityIcons.CategoriesIcon),
        });

        categories = await GetAllAsync<CategoryDto>("Categories?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<CategoryDto>($"Categories/{id}");

        if (isSuccess)
            categories.Remove(categories.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<CategoryDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<ClientDto>($"Categories/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            categories.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }
    private bool FilterFunc(CategoryDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Color.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.OrderIndex.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
