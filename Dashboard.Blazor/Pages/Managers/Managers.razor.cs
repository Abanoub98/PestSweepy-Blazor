namespace Dashboard.Blazor.Pages.Managers;

public partial class Managers
{
    private List<ManagerDto> managers = new();

    private readonly string formUri = "Managers/Form";
    private readonly string detailsUri = "Managers/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Managers"], href: null, disabled: true, icon: Icons.Material.Outlined.Diversity3),
        });

        managers = await GetAllAsync<ManagerDto>("Managers?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<ManagerDto>($"Managers/{id}");

        if (isSuccess)
        {
            managers.Remove(managers.FirstOrDefault(x => x.Id == id)!);

            if (selectedIds.Contains(id))
                selectedIds.Remove(id);
        }

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<ManagerDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<ManagerDto>($"Managers/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            managers.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }

    private bool FilterFunc(ManagerDto element)
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
