namespace Dashboard.Blazor.Pages.VisitSheets
{
    public partial class VisitSheetsForm
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private VisitSheetsDto? visitSheetForm;

        // =========================
        // Specific dates helpers
        // =========================

        private bool useSpecificDates;

        private bool UseSpecificDates
        {
            get => useSpecificDates;
            set
            {
                if (useSpecificDates == value)
                    return;

                useSpecificDates = value;

                if (useSpecificDates)
                {
                    // Clear repeated-days data
                    visitSheetForm!.Days = VisitDays.None;
                    visitSheetForm.TotalVisits = 0;
                    visitSheetForm.StartingDate = default;
                }
                else
                {
                    // Clear manually selected dates
                    visitSheetForm!.SpecificDates.Clear();
                    selectedSpecificDate = null;
                    visitSheetForm.TotalVisits = 0;
                }
            }
        }

        private DateTime? selectedSpecificDate;

        protected override async Task OnParametersSetAsync()
        {
            visitSheetForm = (Id == 0)
                ? new()
                : await GetByIdAsync<VisitSheetsDto>($"VisitSheets/{Id}");

            if (visitSheetForm is null)
                return;

            visitSheetForm.SpecificDates ??= new List<DateTime>();

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["VisitSheets"], href: "/VisitSheets", icon: EntityIcons.VisitSheetsIcon),
                new(
                    Id == 0
                        ? $"{languageContainer.Keys["Add"]} {languageContainer.Keys["VisitSheet"]}"
                        : $"{languageContainer.Keys["Edit"]} {visitSheetForm.Contract.ContractClient.FirstName} - {visitSheetForm.Branch.Name}",
                    href: null,
                    disabled: true
                ),
            });
        }

        private async Task OnValidSubmit(EditContext context)
        {
            StartProcessing();

            // =========================
            // Specific dates validation
            // =========================

            if (Id == 0 && UseSpecificDates)
            {
                if (visitSheetForm!.SpecificDates?.Count == 0)
                {
                    Snackbar.Add("You must select at least one visit date.", Severity.Error);
                    StopProcessing();
                    return;
                }

                visitSheetForm.TotalVisits = visitSheetForm.SpecificDates?.Count ?? 0;
                visitSheetForm.StartingDate = visitSheetForm.SpecificDates?.Min().Date ?? default;
                visitSheetForm.Days = VisitDays.None;
            }
            else
            {
                visitSheetForm!.SpecificDates?.Clear();
            }

            // =========================
            // Business logic (same style)
            // =========================
            visitSheetForm!.ContractId = visitSheetForm.Contract!.Id;
            visitSheetForm.BranchId = visitSheetForm.Branch!.Id;
            visitSheetForm.SupervisorId = visitSheetForm.Supervisor!.Id;
            visitSheetForm.VisitTypeId = visitSheetForm.VisitType!.Id;

            var result = (Id == 0)
                ? await AddAsync("VisitSheets", visitSheetForm!)
                : await UpdateAsync($"VisitSheets/{Id}", visitSheetForm!);

            if (result.isSuccess)
            {
                if (Id == 0)
                    visitSheetForm!.Id = result.obj!.Id;

                NavigationManager.NavigateTo("/VisitSheets");
            }

            StopProcessing();
        }

        // =========================
        // Lookups (same pattern)
        // =========================

        private async Task<IEnumerable<ContractBaseDto>> GetContracts(string value)
        {
            if (visitSheetForm!.Contracts is null)
                visitSheetForm.Contracts = await GetAllAsync<ContractBaseDto>("/Contracts");

            if (string.IsNullOrEmpty(value))
                return visitSheetForm.Contracts;

            return visitSheetForm.Contracts.Where(x => x.Title.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<BranchBaseDto>> GetBranches(string value)
        {
            var clientId = visitSheetForm?.Contract?.ContractClient?.Id;

            if (clientId is null)
                return Enumerable.Empty<BranchBaseDto>();

            if (visitSheetForm!.Branches is null)
                visitSheetForm.Branches = await GetAllAsync<BranchBaseDto>($"/Branches?FilterQuery=clientId%3D{clientId}");

            if (string.IsNullOrWhiteSpace(value))
                return visitSheetForm.Branches;

            return visitSheetForm.Branches.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task OnContractChanged(ContractBaseDto contract)
        {
            visitSheetForm!.Contract = contract;

            // clear dependent fields
            visitSheetForm.Branch = null!;
            visitSheetForm.Branches = null!;
            visitSheetForm.BranchId = 0;

            // optional preload
            if (contract?.ContractClient?.Id is not null)
                visitSheetForm.Branches = await GetAllAsync<BranchBaseDto>($"/Branches?FilterQuery=clientId%3D{contract.ContractClient.Id}");

            StateHasChanged();
        }

        private async Task<IEnumerable<SupervisorBaseDto>> GetSupervisors(string value)
        {
            if (visitSheetForm!.Supervisors is null)
                visitSheetForm.Supervisors = await GetAllAsync<SupervisorBaseDto>("/Supervisors");

            if (string.IsNullOrEmpty(value))
                return visitSheetForm.Supervisors;

            return visitSheetForm.Supervisors.Where(x => x.FirstName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        // =========================
        // Days flags helpers
        // =========================

        private bool IsDaySelected(VisitDays day) => (visitSheetForm!.Days & day) == day;

        private void SetDay(VisitDays day, bool isSelected)
        {
            if (isSelected)
                visitSheetForm!.Days |= day;
            else
                visitSheetForm!.Days &= ~day;
        }

        // =========================
        // Specific dates helpers
        // =========================

        private void AddSpecificDate()
        {
            if (selectedSpecificDate is null)
                return;

            var date = selectedSpecificDate.Value.Date;

            if (!visitSheetForm!.SpecificDates.Any(x => x.Date == date))
                visitSheetForm.SpecificDates.Add(date);

            visitSheetForm.SpecificDates = visitSheetForm.SpecificDates.OrderBy(x => x).ToList();
            visitSheetForm.TotalVisits = visitSheetForm.SpecificDates.Count;
            visitSheetForm.StartingDate = visitSheetForm.SpecificDates.Min().Date;
            selectedSpecificDate = null;
        }

        private void RemoveSpecificDate(DateTime date)
        {
            visitSheetForm!.SpecificDates.RemoveAll(x => x.Date == date.Date);
            visitSheetForm.TotalVisits = visitSheetForm.SpecificDates.Count;

            if (visitSheetForm.SpecificDates.Count > 0)
                visitSheetForm.StartingDate = visitSheetForm.SpecificDates.Min().Date;
            else
                visitSheetForm.StartingDate = default;
        }

        // =========================
        // Date/Time proxies
        // =========================

        private DateTime? StartingDateProxy
        {
            get => visitSheetForm!.StartingDate == default ? (DateTime?)null : visitSheetForm.StartingDate.Date;
            set => visitSheetForm!.StartingDate = value?.Date ?? default;
        }

        private TimeSpan? VisitTimeProxy
        {
            get => visitSheetForm!.VisitTime == default ? (TimeSpan?)null : visitSheetForm.VisitTime;
            set => visitSheetForm!.VisitTime = value ?? default;
        }

        private async Task<IEnumerable<LookupDto>> GetVisitTypes(string value)
        {
            if (visitSheetForm!.VisitTypes is null)
                visitSheetForm.VisitTypes = await GetAllLookupsAsync("ReferenceData?tableName=ServiceRequestType");

            if (string.IsNullOrEmpty(value))
                return visitSheetForm.VisitTypes;

            return visitSheetForm.VisitTypes.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}