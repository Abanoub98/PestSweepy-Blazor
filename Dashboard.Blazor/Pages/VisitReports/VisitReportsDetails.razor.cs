using Dashboard.Blazor.Pages.VisitSheets;

namespace Dashboard.Blazor.Pages.VisitReports
{
    public partial class VisitReportsDetails
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private VisitReportsDto? visitReportForm;

        private VisitReportLegacyDto? legacyReport;

        private readonly string formUri = "VisitReports/Form";

        protected override async Task OnParametersSetAsync()
        {
            visitReportForm = await GetByIdAsync<VisitReportsDto>($"VisitReports/{Id}");

            if (visitReportForm is null)
                return;

            if (visitReportForm.Visit?.VisitTypeId == (int)ServiceRequestTypeEnum.PestControl &&
                (visitReportForm.Pests is null || !visitReportForm.Pests.Any()))
            {
                legacyReport = await GetByIdAsync<VisitReportLegacyDto>($"VisitReports/LegacyReport/{Id}");
            }

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["VisitReports"], href: "/VisitReports", icon: EntityIcons.VisitReportsIcon),
                new($"{visitReportForm.Id} - {visitReportForm.VisitId}", href: null, disabled: true),
            });
        }

        private async Task OpenVisitAction(VisitStatus status)
        {
            if (visitReportForm?.Visit is null)
                return;

            var parameters = new DialogParameters
            {
                ["VisitId"] = visitReportForm.Visit.Id,
                ["Status"] = status,
                ["CurrentScheduledAt"] = visitReportForm.Visit.ScheduledAt
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
            var dialog = DialogService.Show<VisitActionDialog>(string.Empty, parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled)
                visitReportForm = await GetByIdAsync<VisitReportsDto>($"VisitReports/{Id}");
        }
    }
}
