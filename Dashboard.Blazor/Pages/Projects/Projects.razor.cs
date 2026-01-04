namespace Dashboard.Blazor.Pages.Projects;

public partial class Projects
{
    private List<ProjectsDto> projects = new();

    private readonly string formUri = "Projects/Form";
    private readonly string detailsUri = "Projects/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Projects"], href: null, disabled: true, icon: EntityIcons.CategoriesIcon),
        };

        projects = await GetAllAsync<ProjectsDto>("Projects?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<ProjectsDto>($"Projects/{id}");

        if (isSuccess)
            projects.Remove(projects.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<ProjectsDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<ProjectsDto>($"Projects/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            projects.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }
    private bool FilterFunc(ProjectsDto element)
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
