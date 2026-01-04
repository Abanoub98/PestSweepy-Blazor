namespace Dashboard.Blazor.Pages.Blogs;

public partial class Blogs
{
    private List<BlogsDto> blogs = new();

    private readonly string formUri = "Blogs/Form";
    private readonly string detailsUri = "Blogs/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Blogs"], href: null, disabled: true, icon: EntityIcons.CategoriesIcon),
        };

        blogs = await GetAllAsync<BlogsDto>("Blogs?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<BlogsDto>($"Blogs/{id}");

        if (isSuccess)
            blogs.Remove(blogs.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<BlogsDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<BlogsDto>($"Blogs/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            blogs.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }
    private bool FilterFunc(BlogsDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Author.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.OrderIndex.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
