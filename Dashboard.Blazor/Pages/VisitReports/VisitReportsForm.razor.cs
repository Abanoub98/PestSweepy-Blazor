using System.Text;

namespace Dashboard.Blazor.Pages.VisitReports
{
    public partial class VisitReportsForm
    {
        [Parameter]
        public int Id { get; set; }

        private VisitReportsDto? visitReportForm;

        public SignatureModel ClientSignature { get; set; } = new();

        public SignatureModel SupervisorSignature { get; set; } = new();


        public class SignatureModel
        {
            public byte[] Signature { get; set; } = Array.Empty<byte>();
            public string SignatureAsBase64 => Encoding.UTF8.GetString(Signature);
        }

        // If you want to support edit later, add Id param same as ServiceRequests
        // [Parameter] public int Id { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            // Create mode only (you can extend it to edit same as ServiceRequests later)
            visitReportForm = (Id == 0)
                ? new()
                : await GetByIdAsync<VisitReportsDto>($"VisitReports/{Id}");

            if (visitReportForm is null)
                return;

            visitReportForm.Pests ??= new List<VisitReportPestDto>();

            //visitReportForm.TargetPestTypeId = visitReportForm.TargetPestType!.Id;
            //visitReportForm.UnitId = visitReportForm!.Unit!.Id;
            //visitReportForm.PestsActivityId = visitReportForm!.PestsActivity!.Id;

            HydrateSelectedFromDto();

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["VisitReports"], href: "/VisitReports", icon: EntityIcons.VisitReportsIcon),
                new($"{languageContainer.Keys["Add"]} {languageContainer.Keys["Visit Report"]}", href: null, disabled: true),
            });

            await Task.CompletedTask;
        }

        private async Task CheckOut()
        {
            StartProcessing();

            // set the flag, call same update endpoint
            visitReportForm!.IsCheckOut = true;

            if (ClientSignature.SignatureAsBase64.Length > 0)
            {
                visitReportForm!.ClientSignatureBase64 = ClientSignature.SignatureAsBase64.Substring(22);
            }

            if (SupervisorSignature.SignatureAsBase64.Length > 0)
            {
                visitReportForm!.SupervisorSignatureBase64 = SupervisorSignature.SignatureAsBase64.Substring(22);
            }
            if (visitReportForm!.UploadedImage is not null)
            {
                visitReportForm!.Image = "Image";
            }

            ApplySelectedToDto();
            ApplySingleLookupsIds();

            var result = await UpdateAsync($"VisitReports/{Id}", visitReportForm!);

            if (result.isSuccess)
            {

                if (visitReportForm!.UploadedImage is not null)
                    await UploadImage("VisitReports", visitReportForm.Id, visitReportForm.UploadedImage);

                if (visitReportForm!.UploadedImages?.Any() == true)
                    await UploadImages("VisitReportsAdditionalImages", visitReportForm.Id, visitReportForm.UploadedImages);

                NavigationManager.NavigateTo("/VisitReports");
            }
            StopProcessing();
        }

        private async Task OnValidSubmit(EditContext context)
        {
            StartProcessing();

            if (ClientSignature.SignatureAsBase64.Length > 0)
            {
                visitReportForm!.ClientSignatureBase64 = ClientSignature.SignatureAsBase64.Substring(22);
            }

            if (SupervisorSignature.SignatureAsBase64.Length > 0)
            {
                visitReportForm!.SupervisorSignatureBase64 = SupervisorSignature.SignatureAsBase64.Substring(22);
            }

            ApplySelectedToDto();
            ApplySingleLookupsIds();

            // =========================
            var result = (Id == 0)
                     ? await AddAsync("VisitReports", visitReportForm!)
                     : await UpdateAsync($"VisitReports/{Id}", visitReportForm!);

            if (result.isSuccess)
            {
                if (Id == 0)
                    visitReportForm!.Id = result.obj!.Id;


                if (visitReportForm!.UploadedImage is not null)
                    await UploadImage("VisitReports", visitReportForm.Id, visitReportForm.UploadedImage);

                if (visitReportForm!.UploadedImages?.Any() == true)
                    await UploadImages("VisitReportsAdditionalImages", visitReportForm.Id, visitReportForm.UploadedImages);

                await OnParametersSetAsync();
            }
            StopProcessing();
        }

        // =========================
        // Lookups (same pattern as ServiceRequestsForm)
        // =========================

        // =========================
        // M:M Lookups (Visit Reports Form)
        // Same exact pattern as GetProviders
        // =========================

        private async Task<IEnumerable<LookupDto>> GetProviders(string value)
        { // If you decide to add Providers list in VisitReportsDto later, use visitReportForm.ProvidersList // For now: if you still have endpoint, you can create a Providers property in DTO like ServiceRequests // Example below assumes you added: public IEnumerable<LookupDto>? ProvidersLookup { get; set; }
            if (visitReportForm!.ProvidersLookup is null)
                visitReportForm.ProvidersLookup = await GetAllLookupsAsync("Providers/all");

            if (string.IsNullOrEmpty(value))
                return visitReportForm.ProvidersLookup;

            return visitReportForm.ProvidersLookup
              .Where(x =>
                  (!string.IsNullOrWhiteSpace(x.FirstName) && x.FirstName.Contains(value, StringComparison.InvariantCultureIgnoreCase)) ||
                  (!string.IsNullOrWhiteSpace(x.LastName) && x.LastName.Contains(value, StringComparison.InvariantCultureIgnoreCase)) ||
                  (!string.IsNullOrWhiteSpace(x.EmployeeId) && x.EmployeeId.Contains(value, StringComparison.InvariantCultureIgnoreCase)));
        }

        // 1) PestsTreatmentTypes
        private async Task<IEnumerable<LookupDto>> GetPestsTreatmentTypes(string value)
        {
            if (visitReportForm!.PestsTreatmentTypesLookup is null)
                visitReportForm.PestsTreatmentTypesLookup =
                    await GetAllLookupsAsync("ReferenceData?tableName=PestsTreatmentType");

            if (string.IsNullOrEmpty(value))
                return visitReportForm.PestsTreatmentTypesLookup;

            return visitReportForm.PestsTreatmentTypesLookup
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        // 2) PestsTypes
        private async Task<IEnumerable<LookupDto>> GetPestsTypes(string value)
        {
            if (visitReportForm!.PestsTypesLookup is null)
                visitReportForm.PestsTypesLookup =
                    await GetAllLookupsAsync("ReferenceData?tableName=PestsType");

            if (string.IsNullOrEmpty(value))
                return visitReportForm.PestsTypesLookup;

            return visitReportForm.PestsTypesLookup
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        // 3) Chemicals
        private async Task<IEnumerable<LookupDto>> GetChemicals(string value)
        {
            if (visitReportForm!.ChemicalsLookup is null)
                visitReportForm.ChemicalsLookup =
                    await GetAllLookupsAsync("ReferenceData?tableName=Chemicals");

            if (string.IsNullOrEmpty(value))
                return visitReportForm.ChemicalsLookup;

            return visitReportForm.ChemicalsLookup
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        // 4) Infections
        private async Task<IEnumerable<LookupDto>> GetInfections(string value)
        {
            if (visitReportForm!.InfectionsLookup is null)
                visitReportForm.InfectionsLookup =
                    await GetAllLookupsAsync("ReferenceData?tableName=Infections");

            if (string.IsNullOrEmpty(value))
                return visitReportForm.InfectionsLookup;

            return visitReportForm.InfectionsLookup
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        // 5) MaintenanceTypes
        private async Task<IEnumerable<LookupDto>> GetMaintenanceTypes(string value)
        {
            if (visitReportForm!.MaintenanceTypesLookup is null)
                visitReportForm.MaintenanceTypesLookup =
                    await GetAllLookupsAsync("ReferenceData?tableName=MaintenanceTypes");

            if (string.IsNullOrEmpty(value))
                return visitReportForm.MaintenanceTypesLookup;

            return visitReportForm.MaintenanceTypesLookup
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }


        // Pest Control lookups
        private async Task<IEnumerable<LookupDto>> GetPestsActivities(string value)
        {
            if (visitReportForm!.PestsActivities is null)
                visitReportForm.PestsActivities = await GetAllLookupsAsync("ReferenceData?tableName=PestsActivity");

            if (string.IsNullOrEmpty(value))
                return visitReportForm.PestsActivities;

            return visitReportForm.PestsActivities
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        // Deep Cleaning lookups
        private async Task<IEnumerable<LookupDto>> GetCleaningTypes(string value)
        {
            if (visitReportForm!.CleaningTypes is null)
                visitReportForm.CleaningTypes = await GetAllLookupsAsync("ReferenceData?tableName=CleaningType");

            if (string.IsNullOrEmpty(value))
                return visitReportForm.CleaningTypes;

            return visitReportForm.CleaningTypes
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<LookupDto>> GetUnits(string value)
        {
            if (visitReportForm!.Units is null)
                visitReportForm.Units = await GetAllLookupsAsync("ReferenceData?tableName=Units");

            if (string.IsNullOrEmpty(value))
                return visitReportForm.Units;

            return visitReportForm.Units
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        // Building Surface lookups
        private async Task<IEnumerable<LookupDto>> GetSurfaceTypes(string value)
        {
            if (visitReportForm!.SurfaceTypes is null)
                visitReportForm.SurfaceTypes = await GetAllLookupsAsync("ReferenceData?tableName=SurfaceType");

            if (string.IsNullOrEmpty(value))
                return visitReportForm.SurfaceTypes;

            return visitReportForm.SurfaceTypes
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<LookupDto>> GetWorkWays(string value)
        {
            if (visitReportForm!.WorkWays is null)
                visitReportForm.WorkWays = await GetAllLookupsAsync("ReferenceData?tableName=WorkWay");

            if (string.IsNullOrEmpty(value))
                return visitReportForm.WorkWays;

            return visitReportForm.WorkWays
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }


        private LookupDto? _providerToAdd;
        private readonly List<LookupDto> _selectedProviders = new();

        private LookupDto? _maintenanceTypeToAdd;
        private readonly List<LookupDto> _selectedMaintenanceTypes = new();

        // -------------------------
        // ✅ 2) Your Add/Remove helpers (same style)
        // -------------------------

        private void RemoveSelected(List<LookupDto> list, LookupDto item)
        {
            if (item is null)
                return;

            list.RemoveAll(x => x.Id == item.Id);
        }

        private void AddSelectedProvider()
        {
            if (_providerToAdd is null)
                return;

            if (_selectedProviders.Any(x => x.Id == _providerToAdd.Id))
                return;

            _selectedProviders.Add(_providerToAdd);
            _providerToAdd = null;
        }

        private void AddSelectedMaintenanceType()
        {
            if (_maintenanceTypeToAdd is null)
                return;

            if (_selectedMaintenanceTypes.Any(x => x.Id == _maintenanceTypeToAdd.Id))
                return;

            _selectedMaintenanceTypes.Add(_maintenanceTypeToAdd);
            _maintenanceTypeToAdd = null;
        }

        private void AddPest()
        {
            visitReportForm!.Pests ??= new List<VisitReportPestDto>();
            visitReportForm.Pests.Add(new VisitReportPestDto());
        }

        private void RemovePest(VisitReportPestDto pest)
        {
            visitReportForm?.Pests?.Remove(pest);
        }

        // -------------------------
        // ✅ 3) When opening the form (Edit mode) you MUST hydrate chip lists from DTO bridges
        // -------------------------

        private void HydrateSelectedFromDto()
        {
            _selectedProviders.Clear();
            _selectedMaintenanceTypes.Clear();

            if (visitReportForm!.Providers is not null)
            {
                foreach (var item in visitReportForm.Providers)
                {
                    if (item.Provider is not null &&
                        _selectedProviders.All(x => x.Id != item.ProviderId))
                    {
                        _selectedProviders.Add(item.Provider);
                    }
                }
            }

            if (visitReportForm.MaintenanceTypes is not null)
            {
                foreach (var item in visitReportForm.MaintenanceTypes)
                {
                    if (item.MaintenanceType is not null &&
                        _selectedMaintenanceTypes.All(x => x.Id != item.MaintenanceTypeId))
                    {
                        _selectedMaintenanceTypes.Add(item.MaintenanceType);
                    }
                }
            }

            visitReportForm.Pests ??= new List<VisitReportPestDto>();
        }

        // -------------------------
        // ✅ 4) BEFORE submit: convert selected chips => bridges on DTO
        // -------------------------

        private void ApplySelectedToDto()
        {
            visitReportForm!.Providers = _selectedProviders
                .Select(x => new VisitReportsProviderDto
                {
                    VisitReportId = visitReportForm.Id,
                    ProviderId = x.Id,
                    Provider = x
                })
                .ToList();

            visitReportForm.MaintenanceTypes = _selectedMaintenanceTypes
                .Select(x => new VisitReportsMaintenanceTypeDto
                {
                    VisitReportId = visitReportForm.Id,
                    MaintenanceTypeId = x.Id,
                    MaintenanceType = x
                })
                .ToList();

            ApplyPestLookupIds();
        }

        private void ApplyPestLookupIds()
        {
            if (visitReportForm?.Pests is null)
                return;

            foreach (var pest in visitReportForm.Pests)
            {
                pest.PestTypeId = pest.PestType?.Id ?? 0;
                pest.PestsActivityId = pest.PestsActivity?.Id;
                pest.InfectionId = pest.Infection?.Id;
                pest.PestsTreatmentTypeId = pest.PestsTreatmentType?.Id;
                pest.ChemicalId = pest.Chemical?.Id;
            }
        }

        // -------------------------
        // ✅ 5) 1:M fix: set IDs from selected LookupDto BEFORE submit
        // -------------------------

        private void ApplySingleLookupsIds()
        {
            visitReportForm!.CleaningTypeId =
                visitReportForm.CleaningType?.Id;

            visitReportForm.UnitId =
                visitReportForm.Unit?.Id;

            visitReportForm.SurfaceTypeId =
                visitReportForm.SurfaceType?.Id;

            visitReportForm.WorkWayId =
                visitReportForm.WorkWay?.Id;
        }

        private void CaptureUploadedImage(IBrowserFile image) => visitReportForm!.UploadedImage = image;

        private void ClearUploadedImage() => visitReportForm!.UploadedImage = null;

        private void CaptureUploadedImages(List<IBrowserFile> images)
        {
            visitReportForm!.UploadedImages = images;
        }

        private void ClearUploadedImages()
        {
            visitReportForm!.UploadedImages = new List<IBrowserFile>();
        }
    }
}