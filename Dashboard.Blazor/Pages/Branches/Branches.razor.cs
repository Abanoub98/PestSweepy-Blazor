namespace Dashboard.Blazor.Pages.Branches;

public partial class Branches
{
    private List<BranchDto> branches = new();

    private readonly string formUri = "Branches/Form";
    private readonly string detailsUri = "Branches/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Branches"], href: null, disabled: true, icon: Icons.Material.Outlined.Diversity3),
        };

        branches = await GetAllAsync<BranchDto>("Branches?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<BranchDto>($"Branches/{id}");

        if (isSuccess)
        {
            branches.Remove(branches.FirstOrDefault(x => x.Id == id)!);

            if (selectedIds.Contains(id))
                selectedIds.Remove(id);
        }

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<BranchDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<BranchDto>($"Branches/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            branches.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }

    private bool FilterFunc(BranchDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Manager.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Client.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Client.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.City.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.PhoneNumber!.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Email!.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
