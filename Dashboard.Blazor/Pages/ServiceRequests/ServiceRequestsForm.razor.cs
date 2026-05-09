namespace Dashboard.Blazor.Pages.ServiceRequests
{
    public partial class ServiceRequestsForm
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private ServiceRequestsDto? serviceRequestForm;

        protected override async Task OnParametersSetAsync()
        {
            serviceRequestForm = (Id == 0)
                ? new()
                : await GetByIdAsync<ServiceRequestsDto>($"ServiceRequests/{Id}");

            if (serviceRequestForm is null)
                return;

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["ServiceRequests"], href: "/ServiceRequests", icon: EntityIcons.ServiceRequestsIcon),
                new(
                    Id == 0
                        ? $"{languageContainer.Keys["Add"]} {languageContainer.Keys["ServiceRequest"]}"
                        : $"{languageContainer.Keys["Edit"]} {serviceRequestForm.EntityName}",
                    href: null,
                    disabled: true
                ),
            });
        }

        private async Task OnValidSubmit(EditContext context)
        {
            StartProcessing();

            serviceRequestForm!.ServiceRequestTypeId = serviceRequestForm.ServiceRequestType!.Id;

            // =========================
            // General properties
            // =========================
            serviceRequestForm.UnitId = serviceRequestForm.Unit!.Id;

            // =========================
            // Conditional lookups by SR Type
            // =========================
            var srTypeId = serviceRequestForm.ServiceRequestTypeId;

            if (srTypeId == (int)ServiceRequestTypeEnum.PestControl)
            {
                serviceRequestForm.BuildingTypeId = serviceRequestForm.BuildingType!.Id;

                serviceRequestForm.PestsTypeId =
                    serviceRequestForm.PestsType is null ? null : serviceRequestForm.PestsType.Id;

                serviceRequestForm.PestsTreatmentTypeId =
                    serviceRequestForm.PestsTreatmentType is null ? null : serviceRequestForm.PestsTreatmentType.Id;
            }
            else
            {
                // clear pest fields (prevents saving wrong data if user changed type)
                serviceRequestForm.BuildingTypeId = null;
                serviceRequestForm.BuildingType = null;

                serviceRequestForm.PestsTypeId = null;
                serviceRequestForm.PestsType = null;

                serviceRequestForm.PestsTreatmentTypeId = null;
                serviceRequestForm.PestsTreatmentType = null;

                serviceRequestForm.InfectionPercent = null;
                serviceRequestForm.RoomsCount = null;
                serviceRequestForm.SuitesCount = null;
                serviceRequestForm.KitchensCount = null;
                serviceRequestForm.RestuarntsCount = null;
            }

            if (srTypeId == (int)ServiceRequestTypeEnum.DeepCleaning)
            {
                serviceRequestForm.CleaningTypeId =
                    serviceRequestForm.CleaningType is null ? null : serviceRequestForm.CleaningType.Id;
            }
            else
            {
                serviceRequestForm.CleaningTypeId = null;
                serviceRequestForm.CleaningType = null;
                serviceRequestForm.EntityCount = null;
            }

            if (srTypeId == (int)ServiceRequestTypeEnum.BuildingSurface)
            {
                serviceRequestForm.SurfaceTypeId =
                    serviceRequestForm.SurfaceType is null ? null : serviceRequestForm.SurfaceType.Id;

                serviceRequestForm.WorkWayId =
                    serviceRequestForm.WorkWay is null ? null : serviceRequestForm.WorkWay.Id;
            }
            else
            {
                serviceRequestForm.SurfaceTypeId = null;
                serviceRequestForm.SurfaceType = null;

                serviceRequestForm.WorkWayId = null;
                serviceRequestForm.WorkWay = null;
            }

            var result = (Id == 0)
                ? await AddAsync("ServiceRequests", serviceRequestForm!)
                : await UpdateAsync($"ServiceRequests/{Id}", serviceRequestForm!);

            if (result.isSuccess)
            {
                if (Id == 0)
                    serviceRequestForm!.Id = result.obj!.Id;

                //if (serviceRequestForm!.UploadedImage is not null)
                //    await UploadImage("ServiceRequests", serviceRequestForm.Id, serviceRequestForm.UploadedImage);

                if (serviceRequestForm!.UploadedImages?.Any() == true)
                    await UploadImages("ServiceRequestAttachments", serviceRequestForm.Id, serviceRequestForm.UploadedImages);

                NavigationManager.NavigateTo("/ServiceRequests");
            }

            StopProcessing();
        }

        // =========================
        // Lookups (Service Requests Form)
        // Same pattern as GetCountries
        // =========================

        private async Task<IEnumerable<LookupDto>> GetServiceRequestTypes(string value)
        {
            if (serviceRequestForm!.ServiceRequestTypes is null)
                serviceRequestForm.ServiceRequestTypes = await GetAllLookupsAsync("ReferenceData?tableName=ServiceRequestType");

            if (string.IsNullOrEmpty(value))
                return serviceRequestForm.ServiceRequestTypes;

            return serviceRequestForm.ServiceRequestTypes
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<LookupDto>> GetBuildingTypes(string value)
        {
            if (serviceRequestForm!.BuildingTypes is null)
                serviceRequestForm.BuildingTypes = await GetAllLookupsAsync("ReferenceData?tableName=BuildingType");

            if (string.IsNullOrEmpty(value))
                return serviceRequestForm.BuildingTypes;

            return serviceRequestForm.BuildingTypes
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<LookupDto>> GetPestsTypes(string value)
        {
            if (serviceRequestForm!.PestsTypes is null)
                serviceRequestForm.PestsTypes = await GetAllLookupsAsync("ReferenceData?tableName=PestsType");

            if (string.IsNullOrEmpty(value))
                return serviceRequestForm.PestsTypes;

            return serviceRequestForm.PestsTypes
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<LookupDto>> GetPestsTreatmentTypes(string value)
        {
            if (serviceRequestForm!.PestsTreatmentTypes is null)
                serviceRequestForm.PestsTreatmentTypes = await GetAllLookupsAsync("ReferenceData?tableName=PestsTreatmentType");

            if (string.IsNullOrEmpty(value))
                return serviceRequestForm.PestsTreatmentTypes;

            return serviceRequestForm.PestsTreatmentTypes
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<LookupDto>> GetCleaningTypes(string value)
        {
            if (serviceRequestForm!.CleaningTypes is null)
                serviceRequestForm.CleaningTypes = await GetAllLookupsAsync("ReferenceData?tableName=CleaningType");

            if (string.IsNullOrEmpty(value))
                return serviceRequestForm.CleaningTypes;

            return serviceRequestForm.CleaningTypes
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<LookupDto>> GetSurfaceTypes(string value)
        {
            if (serviceRequestForm!.SurfaceTypes is null)
                serviceRequestForm.SurfaceTypes = await GetAllLookupsAsync("ReferenceData?tableName=SurfaceType");

            if (string.IsNullOrEmpty(value))
                return serviceRequestForm.SurfaceTypes;

            return serviceRequestForm.SurfaceTypes
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<LookupDto>> GetWorkWays(string value)
        {
            if (serviceRequestForm!.WorkWays is null)
                serviceRequestForm.WorkWays = await GetAllLookupsAsync("ReferenceData?tableName=WorkWay");

            if (string.IsNullOrEmpty(value))
                return serviceRequestForm.WorkWays;

            return serviceRequestForm.WorkWays
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<LookupDto>> GetUnits(string value)
        {
            if (serviceRequestForm!.Units is null)
                serviceRequestForm.Units = await GetAllLookupsAsync("ReferenceData?tableName=Units");

            if (string.IsNullOrEmpty(value))
                return serviceRequestForm.Units;

            return serviceRequestForm.Units
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }


        private void CaptureUploadedImages(List<IBrowserFile> images)
        {
            serviceRequestForm!.UploadedImages = images;
        }

        private void ClearUploadedImages()
        {
            serviceRequestForm!.UploadedImages = new List<IBrowserFile>();
        }

        //private void CaptureUploadedImage(IBrowserFile image) => serviceRequestForm!.UploadedImage = image;

        //private void ClearUploadedImage() => serviceRequestForm!.UploadedImage = null;
    }
}
