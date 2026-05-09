namespace Dashboard.Blazor.Pages.VisitSheets;

public partial class VisitSheets
{
    private List<VisitSheetsDto> visitSheets = new();

    private readonly string formUri = "VisitSheets/Form";
    private readonly string detailsUri = "VisitSheets/Details";
    private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();
    private string role = null!;

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        claims = await GetClaimsPrincipalData();
        role = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value ?? string.Empty;

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["VisitSheets"], href: null, disabled: true, icon: EntityIcons.CategoriesIcon),
        };

        visitSheets = await GetAllAsync<VisitSheetsDto>("VisitSheets?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<VisitSheetsDto>($"VisitSheets/{id}");

        if (isSuccess)
            visitSheets.Remove(visitSheets.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<VisitSheetsDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<VisitSheetsDto>($"VisitSheets/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            visitSheets.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }
    private bool FilterFunc(VisitSheetsDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Contract.ContractClient.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Contract.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Branch.Name.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
