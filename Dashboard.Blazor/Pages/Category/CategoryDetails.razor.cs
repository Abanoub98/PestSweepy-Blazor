namespace Dashboard.Blazor.Pages.Category;

public partial class CategoryDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private CategoryDto? category;
    private readonly string formUri = "Categories/Form";

    protected override async Task OnParametersSetAsync()
    {
        category = await GetByIdAsync<CategoryDto>($"Categories/{Id}");

        if (category is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Categories"], href: "/Categories", icon: EntityIcons.CategoriesIcon),
            new(category.Name, href: null, disabled: true),
        });
    }
}
