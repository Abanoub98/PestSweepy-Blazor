namespace Dashboard.Blazor.Pages.VisitSheets
{
    public partial class VisitSheetsDetails
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private VisitSheetsDto? visitSheet;
        private readonly string formUri = "VisitSheets/Form";

        // ===== Visits table state =====
        private string visitsSearchString = string.Empty;
        private List<int> selectedVisitIds = new();

        // (Optional) if your table buttons use these
        private readonly string visitFormUri = "Visits/Form";
        private readonly string visitDetailsUri = "Visits/Details";
        private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();
        private string role = null!;

        protected override async Task OnParametersSetAsync()
        {
            claims = await GetClaimsPrincipalData();
            role = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value ?? string.Empty;

            visitSheet = await GetByIdAsync<VisitSheetsDto>($"VisitSheets/{Id}");

            if (visitSheet is null)
                return;

            // Ensure visits list is never null
            //if (visitSheet != null && visitSheet.Visits == null)
            //    visitSheet.Visits = new List<VisitBaseDto>();

            // Reset selection when opening / reloading
            selectedVisitIds = new();

            // Breadcrumbs (avoid duplicates if OnParametersSetAsync triggers again)
            breadcrumbItems = new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["VisitSheets"], href: "/VisitSheets", icon: EntityIcons.VisitSheetsIcon),
                new($"{visitSheet?.Contract.ContractClient.FirstName} - {visitSheet?.Branch.Name}", href: null, disabled: true),
            };
        }

        // Called by MudDataGrid SelectedItemsChanged
        private void SelectedVisitItemsChanged(HashSet<VisitBaseDto> items)
            => selectedVisitIds = items.Select(i => i.Id).ToList();

        // Called by MudDataGrid QuickFilter
        private bool VisitsFilterFunc(VisitBaseDto element)
        {
            if (string.IsNullOrWhiteSpace(visitsSearchString))
                return true;

            // Date
            if (element.ScheduledAt.ToString("dd/MM/yyyy").Contains(visitsSearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            // Status
            if (element.Status.ToString().Contains(visitsSearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            // Completion Notes
            if (!string.IsNullOrWhiteSpace(element.Notes) &&
                element.Notes.Contains(visitsSearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            // Completed At
            if (element.CompletedAt.HasValue &&
                element.CompletedAt.Value.ToString("dd/MM/yyyy HH:mm").Contains(visitsSearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        // Optional: refresh details (same API) for the "Refresh" button in Visits tab
        private async Task ReloadDetailsAsync()
        {
            isLoading = true;
            StartProcessing();

            visitSheet = await GetByIdAsync<VisitSheetsDto>($"VisitSheets/{Id}");
            if (visitSheet != null && visitSheet.Visits == null)
                visitSheet.Visits = new List<VisitBaseDto>();

            selectedVisitIds = new();

            StopProcessing();
            isLoading = false;
        }

        // Optional: delete a single visit (if you wire DeleteClicked in the table)
        private async Task DeleteVisit(int visitId)
        {
            StartProcessing();

            var isSuccess = await DeleteAsync<VisitBaseDto>($"Visits/{visitId}");
            if (isSuccess && visitSheet?.Visits != null)
                visitSheet.Visits.RemoveAll(x => x.Id == visitId);

            StopProcessing();
        }

        // Optional: delete multiple visits (if you wire DeleteAllVisits in the toolbar)
        private async Task DeleteAllVisits()
        {
            if (selectedVisitIds is null || selectedVisitIds.Count == 0)
                return;

            StartProcessing();

            //var isSuccess = await DeleteAllAsync<VisitBaseDto>($"Visits/DeleteMultiple", selectedVisitIds);

            //if (isSuccess && visitSheet?.Visits != null)
            //{
            //    visitSheet.Visits.RemoveAll(x => selectedVisitIds.Contains(x.Id));
            //    selectedVisitIds = new();
            //}

            StopProcessing();
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
                await ReloadDetailsAsync(); // أو refresh grid
            }
        }

        private async Task InitVisitReport(int visitId)
        {
            try
            {
                isLoading = true;

                var dto = new VisitReportInitDto
                {
                    VisitId = visitId
                };

                // ✅ Call: POST /VisitReports/Init
                await AddAsync("VisitReports/Init", dto);

                // ✅ reload page data so the button disappears and report appears
                await ReloadDetailsAsync(); // أو الميثود اللي بتعمل refresh للـ visitReportForm عندك
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
    }
}
