namespace Dashboard.Blazor.Pages.Category;

public partial class CategoryDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private CategoryDto? category;
    private readonly string formUri = "Categories/Form";

    protected override async Task OnParametersSetAsync()
    {
        category = await GetByIdAsync($"Categories/{Id}");

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["Categories"], href: "/Categories", icon: Icons.Material.TwoTone.Category),
            new BreadcrumbItem(category.Name, href: null, disabled: true),
        });
    }
}
