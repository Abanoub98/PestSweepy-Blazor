namespace Dashboard.Blazor.Pages.VisitReports
{
    public partial class VisitReportsDetails
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private VisitReportsDto? visitReportForm;
        private readonly string formUri = "VisitReports/Form";

        protected override async Task OnParametersSetAsync()
        {
            visitReportForm = await GetByIdAsync<VisitReportsDto>($"VisitReports/{Id}");

            if (visitReportForm is null)
                return;

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["VisitReports"], href: "/VisitReports", icon: EntityIcons.VisitReportsIcon),
                new($"{visitReportForm.Id} - {visitReportForm.VisitId}", href: null, disabled: true),
            });
        }
    }
}
