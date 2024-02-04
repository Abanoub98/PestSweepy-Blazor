namespace Dashboard.Blazor.Pages.Supervisors;

public partial class Supervisors
{
    [Parameter] public int? ManagerId { get; set; }

    private List<SupervisorDto> supervisors = new();

    private readonly string formUri = "Supervisors/Form";
    private readonly string detailsUri = "Supervisors/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Supervisors"], href: null, disabled: true, icon: Icons.Material.Outlined.SupervisorAccount),
        };

        supervisors = ManagerId is not null ?
            await GetAllAsync<SupervisorDto>($"Supervisors?OrderBy=id&Asc=false&FilterQuery={Uri.EscapeDataString($"ManagerId={ManagerId}")}") :
            await GetAllAsync<SupervisorDto>("Supervisors?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<SupervisorDto>($"Supervisors/{id}");

        if (isSuccess)
        {
            supervisors.Remove(supervisors.FirstOrDefault(x => x.Id == id)!);

            if (selectedIds.Contains(id))
                selectedIds.Remove(id);
        }

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<SupervisorDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<SupervisorDto>($"Supervisors/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            supervisors.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }

    private bool FilterFunc(SupervisorDto element)
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
