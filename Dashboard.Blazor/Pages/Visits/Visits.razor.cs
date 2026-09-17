using System.Text;
using System.Text.Json;

using Dashboard.Blazor.Pages.VisitSheets;

using Newtonsoft.Json.Linq;

namespace Dashboard.Blazor.Pages.Visits;

public partial class Visits
{
    private List<VisitBaseDto> visits = new();

    private readonly string formUri = "Visits/Form";
    private readonly string detailsUri = "Visits/Details";
    private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();
    private string role = null!;

    [Inject] private HttpClient HttpClient { get; set; } = default!;


    private List<VisitBaseDto> filteredVisits = new();

    private int activeStatusTab;

    private readonly VisitStatus?[] visitStatusTabs =
    {
        VisitStatus.Scheduled,
        VisitStatus.Completed,
        VisitStatus.ReScheduled,
        VisitStatus.Missed,
        VisitStatus.Cancelled,
        null // All
    };

    private VisitReportFilter _visitReportFilter = VisitReportFilter.ReadyForReview;

    private enum VisitReportFilter
    {
        ReadyForReview,
        ReportInProgress,
        NoReport,
        All
    }

    private bool IsScheduledTab => visitStatusTabs[activeStatusTab] == VisitStatus.Scheduled;

    private VisitsDateFilter _dateFilter = VisitsDateFilter.All;

    private enum VisitsDateFilter
    {
        All,
        Today,
        ThisWeek,
        ThisMonth
    }

    protected override async Task OnInitializedAsync()
    {
        claims = await GetClaimsPrincipalData();
        role = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value ?? string.Empty;

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Visits"], href: null, disabled: true, icon: EntityIcons.CategoriesIcon),
        };

        await LoadVisits();
    }

    private async Task LoadVisits()
    {
        StartProcessing();

        try
        {
            var selectedStatus = visitStatusTabs[activeStatusTab];

            var url = "Visits?OrderBy=scheduledAt&Asc=true";

            if (selectedStatus.HasValue)
            {
                var filterQuery = Uri.EscapeDataString(
                    $"Status=\"{selectedStatus.Value}\""
                );

                url += $"&FilterQuery={filterQuery}";
            }

            visits = await GetAllAsync<VisitBaseDto>(url) ?? new List<VisitBaseDto>();

            ApplyDateFilter();
        }
        finally
        {
            StopProcessing();
        }
    }

    private IEnumerable<VisitBaseDto> DisplayedVisits =>
    !IsScheduledTab
        ? filteredVisits
        : _visitReportFilter switch
        {
            VisitReportFilter.ReadyForReview => filteredVisits.Where(x => x.VisitReport != null && x.VisitReport.Status == VisitReportStatus.Completed.ToString()),
            VisitReportFilter.ReportInProgress => filteredVisits.Where(x => x.VisitReport != null && x.VisitReport.Status != VisitReportStatus.Completed.ToString()),
            VisitReportFilter.NoReport => filteredVisits.Where(x => x.VisitReport == null),
            _ => filteredVisits
        };

    private async Task ChangeStatusTab(int index)
    {
        activeStatusTab = index;

        if (visitStatusTabs[index] == VisitStatus.Scheduled)
            _visitReportFilter = VisitReportFilter.ReadyForReview;

        await LoadVisits();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<VisitBaseDto>($"Visits/{id}");

        if (isSuccess)
            visits.Remove(visits.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<VisitBaseDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        //var isSuccess = await DeleteAllAsync<VisitBaseDto>($"Visits/DeleteMultiple", selectedIds);

        //if (isSuccess)
        //{
        //    visits.RemoveAll(x => selectedIds.Contains(x.Id));
        //    selectedIds = new();
        //}

        StopProcessing();
    }
    private bool FilterFunc(VisitBaseDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Id.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Status.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.BranchName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.ClientName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }


    private async Task OpenVisitAction(VisitBaseDto visit, VisitStatus status)
    {
        var parameters = new DialogParameters
        {
            ["VisitId"] = visit.Id,
            ["Status"] = status,
            ["CurrentScheduledAt"] = visit.ScheduledAt
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

        var dialog = DialogService.Show<VisitActionDialog>(string.Empty, parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await OnInitializedAsync(); // أو refresh grid
        }
    }





    private async Task InitVisitReport(int visitId)
    {
        try
        {
            isLoading = true;

            var dto = new VisitReportInitDto { VisitId = visitId };

            var body = JsonSerializer.Serialize(dto);
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await HttpClient.PostAsync("VisitReports/Init", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                JObject jsonResponse = JObject.Parse(result);

                int reportId = (int)jsonResponse["id"]!;

                NavigationManager.NavigateTo($"/VisitReports/Form/{reportId}");
            }
            else
            {
                var result = await response.Content.ReadAsStringAsync();
                JObject jsonResponse = JObject.Parse(result);

                string errorMessage = (string)jsonResponse["message"]!;
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ShowError(errorMessage);
                }
            }
        }
        finally
        {
            isLoading = false;
        }
    }





    private void OpenVisitReport(VisitBaseDto v)
    {
        NavigationManager.NavigateTo($"/VisitReports/Form/{v.VisitReport?.Id}");
    }

    private void SetDateFilter(VisitsDateFilter filter)
    {
        _dateFilter = filter;
        ApplyDateFilter();
    }

    private void ApplyDateFilter()
    {
        var now = DateTime.Now;
        var today = now.Date;

        IEnumerable<VisitBaseDto> query = visits;

        if (_dateFilter == VisitsDateFilter.Today)
        {
            query = query.Where(v => v.ScheduledAt.Date == today);
        }
        else if (_dateFilter == VisitsDateFilter.ThisWeek)
        {
            // week start: Sunday (0) -> Saturday
            int diff = (int)today.DayOfWeek;
            var weekStart = today.AddDays(-diff);
            var weekEndExclusive = weekStart.AddDays(7);

            query = query.Where(v => v.ScheduledAt >= weekStart && v.ScheduledAt < weekEndExclusive);
        }
        else if (_dateFilter == VisitsDateFilter.ThisMonth)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1);
            var nextMonthStart = monthStart.AddMonths(1);

            query = query.Where(v => v.ScheduledAt >= monthStart && v.ScheduledAt < nextMonthStart);
        }

        filteredVisits = query.ToList();
    }

}
