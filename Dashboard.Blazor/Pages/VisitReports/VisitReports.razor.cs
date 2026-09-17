namespace Dashboard.Blazor.Pages.VisitReports;

public partial class VisitReports
{
    private List<VisitReportsDto> visitReports = new();

    private readonly string formUri = "VisitReports/Form";
    private readonly string detailsUri = "VisitReports/Details";

    private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();
    private string role = null!;

    private int activeStatusTab;

    private readonly VisitReportStatus?[] visitReportStatusTabs =
    {
        VisitReportStatus.Completed,
        VisitReportStatus.CheckedIn,
        //VisitReportStatus.InProgress,
        //VisitReportStatus.Cancelled,
        null // All
    };

    protected override async Task OnInitializedAsync()
    {

        claims = await GetClaimsPrincipalData();
        role = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value ?? string.Empty;

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["VisitReports"], href: null, disabled: true, icon: EntityIcons.CategoriesIcon),
        };

        await LoadVisitReports();
    }

    private async Task LoadVisitReports()
    {
        StartProcessing();

        try
        {
            var selectedStatus = visitReportStatusTabs[activeStatusTab];

            var url = "VisitReports?OrderBy=id&Asc=false";

            if (selectedStatus.HasValue)
            {
                var filterQuery = Uri.EscapeDataString(
                    $"Status=\"{selectedStatus.Value}\""
                );

                url += $"&FilterQuery={filterQuery}";
            }

            visitReports = await GetAllAsync<VisitReportsDto>(url);

        }
        finally
        {
            StopProcessing();
        }
    }

    private async Task ChangeStatusTab(int index)
    {
        activeStatusTab = index;
        await LoadVisitReports();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<VisitReportsDto>($"VisitReports/{id}");

        if (isSuccess)
            visitReports.Remove(visitReports.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<VisitReportsDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<VisitReportsDto>($"VisitReports/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            visitReports.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }
    private bool FilterFunc(VisitReportsDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Id.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Status!.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.VisitId.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }

    private async Task OpenVisitReportReview(int visitReportId)
    {
        var parameters = new DialogParameters
        {
            ["VisitReportId"] = visitReportId
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        var dialog = DialogService.Show<VisitReportClientReviewDialog>(string.Empty, parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await OnInitializedAsync(); // refresh
        }
    }

}
