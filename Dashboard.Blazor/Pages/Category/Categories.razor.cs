namespace Dashboard.Blazor.Pages.Category;

public partial class Categories
{
    private List<CategoryDto> categories = new();
    private string searchString = string.Empty;
    private readonly string formUri = "Categories/Form";
    private readonly string detailsUri = "Categories/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["Categories"], href: null, disabled: true, icon: Icons.Material.TwoTone.Category),
        });

        categories = await GetAllAsync("Categories?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync($"Categories/{id}");
        if (isSuccess)
        {
            categories.Remove(categories.FirstOrDefault(x => x.Id == id)!);
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
