namespace Dashboard.Blazor.Pages.OperationManagers;

public partial class OperationManagers
{
    private List<OperationManagerDto> operationManagers = new();

    private readonly string formUri = "OperationManagers/Form";
    private readonly string detailsUri = "OperationManagers/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["OperationManagers"], href: null, disabled: true, icon: Icons.Material.Outlined.Diversity3),
        };

        operationManagers = await GetAllAsync<OperationManagerDto>("OperationManagers?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<OperationManagerDto>($"OperationManagers/{id}");

        if (isSuccess)
        {
            operationManagers.Remove(operationManagers.FirstOrDefault(x => x.Id == id)!);

            if (selectedIds.Contains(id))
                selectedIds.Remove(id);
        }

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<OperationManagerDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<OperationManagerDto>($"OperationManagers/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            operationManagers.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }

    private bool FilterFunc(OperationManagerDto element)
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
