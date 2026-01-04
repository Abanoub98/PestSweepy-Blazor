namespace Dashboard.Blazor.Pages.Testimonials;

public partial class Testimonials
{
    private List<TestimonialsDto> testimonials = new();

    private readonly string formUri = "Testimonials/Form";
    private readonly string detailsUri = "Testimonials/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Testimonials"], href: null, disabled: true, icon: EntityIcons.CategoriesIcon),
        };

        testimonials = await GetAllAsync<TestimonialsDto>("Testimonials?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<TestimonialsDto>($"Testimonials/{id}");

        if (isSuccess)
            testimonials.Remove(testimonials.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<TestimonialsDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<TestimonialsDto>($"Testimonials/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            testimonials.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }
    private bool FilterFunc(TestimonialsDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.TestimonialText.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.OrderIndex.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
