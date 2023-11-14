using System.Drawing;
namespace Dashboard.Blazor.Pages.Category;

public partial class Categories
{
    private List<CategoryDto> categories = new();
    private string searchString = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["Categories"], href: null, disabled: true, icon: Icons.Material.TwoTone.Category),
        });

        categories = await GetAllAsync("Categories");

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

    private string FindComplementaryColor(string color)
    {
        var originalColor = ColorTranslator.FromHtml(color);

        int red = 255 - originalColor.R;
        int green = 255 - originalColor.G;
        int blue = 255 - originalColor.B;

        return ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(red, green, blue));
    }
}
