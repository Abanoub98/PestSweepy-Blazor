namespace Dashboard.Blazor.Pages.Certificates
{
    public partial class CertificatesForm
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private CertificatesDto? certificateForm;

        protected override async Task OnParametersSetAsync()
        {
            certificateForm = (Id == 0)
                ? new()
                : await GetByIdAsync<CertificatesDto>($"Certificates/{Id}");

            if (certificateForm is null)
                return;

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["Certificates"], href: "/Certificates", icon: EntityIcons.CertificatesIcon),
                new(
                    Id == 0
                        ? $"{languageContainer.Keys["Add"]} {languageContainer.Keys["Certificate"]}"
                        : $"{languageContainer.Keys["Edit"]} {certificateForm.NameEn} - {certificateForm.NameAr}",
                    href: null,
                    disabled: true
                ),
            });
        }

        private async Task OnValidSubmit(EditContext context)
        {
            StartProcessing();

            var result = (Id == 0)
                ? await AddAsync("Certificates", certificateForm!)
                : await UpdateAsync($"Certificates/{Id}", certificateForm!);

            if (result.isSuccess)
            {
                if (Id == 0)
                    certificateForm!.Id = result.obj!.Id;

                if (certificateForm!.UploadedImage is not null)
                    await UploadImage("Certificates", certificateForm.Id, certificateForm.UploadedImage);

                NavigationManager.NavigateTo("/Certificates");
            }

            StopProcessing();
        }

        private void CaptureUploadedImage(IBrowserFile image) => certificateForm!.UploadedImage = image;

        private void ClearUploadedImage() => certificateForm!.UploadedImage = null;
    }
}
