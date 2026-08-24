namespace Dashboard.Blazor.Pages.VisitSheets;

public partial class VisitSheetReportForm
{
    [Parameter] public int Id { get; set; }

    [Inject] private IDialogService DialogService { get; set; } = default!;

    private VisitSheetReportDto? visitSheetReport;
    private bool isProcessing;
    private List<BreadcrumbItem> breadcrumbItems = new();
    private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();
    private string role = null!;

    private bool IsCompleted => string.Equals(visitSheetReport?.Status, "Completed", StringComparison.OrdinalIgnoreCase);
    private bool CanEdit => !IsCompleted && (role == Roles.AdminRole || role == Roles.ManagerRole || role == Roles.SupervisorRole || role == Roles.OperationManagerRole);

    protected override async Task OnParametersSetAsync()
    {
        claims = await GetClaimsPrincipalData();
        role = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value ?? string.Empty;

        await LoadReportAsync();
        breadcrumbItems = new List<BreadcrumbItem> { new(languageContainer.Keys["Home"], href: "/"), new(languageContainer.Keys["Visit Sheets"], href: "/VisitSheets"), new(languageContainer.Keys["Compilation Report"], href: null, disabled: true) };
    }

    private async Task LoadReportAsync()
    {
        visitSheetReport = await GetByIdAsync<VisitSheetReportDto>($"VisitSheetReports/{Id}");
        if (visitSheetReport is not null)
            visitSheetReport.UploadedImages = new List<IBrowserFile>();
    }

    private async Task OnValidSubmit()
    {
        await SaveReportAsync();
    }

    private async Task<bool> SaveReportAsync(bool reload = true)
    {
        if (visitSheetReport is null || !CanEdit)
            return false;

        isProcessing = true;
        var dto = new VisitSheetReportUpdateDto { ReportSummary = visitSheetReport.ReportSummary, Recommendations = visitSheetReport.Recommendations };
        var result = await UpdateAsync($"VisitSheetReports/{Id}", dto);

        if (result.isSuccess && visitSheetReport.UploadedImages.Any())
            await UploadImages("VisitSheetReports", visitSheetReport.Id, visitSheetReport.UploadedImages);

        if (result.isSuccess && reload)
            await LoadReportAsync();

        isProcessing = false;
        return result.isSuccess;
    }

    private async Task CompleteReport()
    {
        if (visitSheetReport is null || !CanEdit)
            return;

        var confirmed = await DialogService.ShowMessageBox(languageContainer.Keys["Complete Report"], languageContainer.Keys["After completing this report, it cannot be edited."], yesText: languageContainer.Keys["Complete"], cancelText: languageContainer.Keys["Cancel"]);
        if (confirmed != true)
            return;

        var saved = await SaveReportAsync(false);
        if (!saved)
            return;

        isProcessing = true;
        var result = await UpdateAsync($"VisitSheetReports/Complete/{Id}", new { });
        isProcessing = false;

        if (result.isSuccess)
            await LoadReportAsync();
    }

    private void CaptureUploadedImages(List<IBrowserFile> images)
    {
        visitSheetReport!.UploadedImages = images;
    }

    private void ClearUploadedImages()
    {
        visitSheetReport!.UploadedImages = new List<IBrowserFile>();
    }

    private void OpenPrint()
    {
        var parameters = new DialogParameters { ["Id"] = Id };
        var options = new DialogOptions { CloseButton = true, FullWidth = true, MaxWidth = MaxWidth.ExtraLarge };
        DialogService.Show<VisitSheetReportPrint>("", parameters, options);
    }
}
