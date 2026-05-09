namespace Dashboard.Blazor.Pages.Complaints
{
    public partial class ComplaintsForm
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private ComplaintDto? complaintForm;
        private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();


        protected override async Task OnParametersSetAsync()
        {
            claims = await GetClaimsPrincipalData();

            complaintForm = (Id == 0)
                ? new()
                : await GetByIdAsync<ComplaintDto>($"Complaints/{Id}");

            if (complaintForm is null)
                return;

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["Complaints"], href: "/Complaints", icon: EntityIcons.ComplaintIcon),
                new(
                    Id == 0
                        ? $"{languageContainer.Keys["Add"]} {languageContainer.Keys["Complaint"]}"
                        : $"{languageContainer.Keys["Edit"]} {complaintForm.Client.FirstName} - {complaintForm.Client.LastName}",
                    href: null,
                    disabled: true
                ),
            });
        }

        private async Task OnValidSubmit(EditContext context)
        {
            StartProcessing();

            complaintForm!.BranchId = complaintForm.Branch?.Id ?? 0;
            complaintForm!.ComplaintTypeId = complaintForm.ComplaintType?.Id ?? 0;

            var result = (Id == 0)
                ? await AddAsync("Complaints", complaintForm!)
                : await UpdateAsync($"Complaints/{Id}", complaintForm!);

            if (result.isSuccess)
            {
                if (Id == 0)
                    complaintForm!.Id = result.obj!.Id;

                if (complaintForm!.UploadedImage is not null)
                    await UploadImage("Complaints", complaintForm.Id, complaintForm.UploadedImage);

                NavigationManager.NavigateTo("/Complaints");
            }

            StopProcessing();
        }

        private async Task<IEnumerable<BranchBaseDto>> GetBranches(string value)
        {
            if (complaintForm!.Branches is null)
                complaintForm.Branches = await GetAllAsync<BranchBaseDto>($"/Branches?FilterQuery=clientId%3D{claims.FirstOrDefault(x => x.Type == "Id")?.Value}");

            if (string.IsNullOrEmpty(value))
                return complaintForm.Branches;

            return complaintForm.Branches
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<LookupDto>> GetComplaintTypes(string value)
        {
            if (complaintForm!.ComplaintTypes is null)
                complaintForm.ComplaintTypes = await GetAllLookupsAsync("ReferenceData?tableName=ComplaintTypes");

            if (string.IsNullOrEmpty(value))
                return complaintForm.ComplaintTypes;

            return complaintForm.ComplaintTypes
                .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private void CaptureUploadedImage(IBrowserFile image) => complaintForm!.UploadedImage = image;

        private void ClearUploadedImage() => complaintForm!.UploadedImage = null;
    }
}
