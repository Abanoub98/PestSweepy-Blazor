namespace Dashboard.Blazor.Pages.OurTeam;

public partial class OurTeam
{
    private List<OurTeamDto> ourTeam = new();

    private readonly string formUri = "OurTeam/Form";
    private readonly string detailsUri = "OurTeam/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["OurTeam"], href: null, disabled: true, icon: EntityIcons.CategoriesIcon),
        };

        ourTeam = await GetAllAsync<OurTeamDto>("OurTeam?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<OurTeamDto>($"OurTeam/{id}");

        if (isSuccess)
            ourTeam.Remove(ourTeam.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<OurTeamDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<OurTeamDto>($"OurTeam/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            ourTeam.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }
    private bool FilterFunc(OurTeamDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.OrderIndex.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
