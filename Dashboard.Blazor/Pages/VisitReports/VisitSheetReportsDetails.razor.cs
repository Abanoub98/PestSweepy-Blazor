namespace Dashboard.Blazor.Pages.VisitReports;

public partial class VisitSheetReportsDetails
{
    [Parameter]
    [EditorRequired]
    public int VisitSheetId { get; set; }

    private List<VisitReportsDto> visitReports = new();

    protected override async Task OnParametersSetAsync()
    {
        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["VisitReports"], href: "/VisitReports", icon: EntityIcons.VisitReportsIcon),
            new($"{languageContainer.Keys["Visit Sheet"]} #{VisitSheetId}", href: null, disabled: true),
        };

        await LoadReportsAsync();
    }

    private async Task LoadReportsAsync()
    {
        StartProcessing();

        try
        {
            var filter = Uri.EscapeDataString($"Visit.VisitSheetId={VisitSheetId}");
            visitReports = await GetAllAsync<VisitReportsDto>($"VisitReports/GetAllDetails?FilterQuery={filter}&OrderBy=id&Asc=false");
        }
        finally
        {
            StopProcessing();
        }
    }
}