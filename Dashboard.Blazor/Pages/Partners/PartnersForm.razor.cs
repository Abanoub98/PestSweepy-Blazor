namespace Dashboard.Blazor.Pages.Partners
{
    public partial class PartnersForm
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private PartnersDto? partnerForm;

        protected override async Task OnParametersSetAsync()
        {
            partnerForm = (Id == 0)
                ? new()
                : await GetByIdAsync<PartnersDto>($"Partners/{Id}");

            if (partnerForm is null)
                return;

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["Partners"], href: "/Partners", icon: EntityIcons.PartnersIcon),
                new(
                    Id == 0
                        ? $"{languageContainer.Keys["Add"]} {languageContainer.Keys["Partner"]}"
                        : $"{languageContainer.Keys["Edit"]} {partnerForm.NameEn} - {partnerForm.NameAr}",
                    href: null,
                    disabled: true
                ),
            });
        }

        private async Task OnValidSubmit(EditContext context)
        {
            StartProcessing();

            var result = (Id == 0)
                ? await AddAsync("Partners", partnerForm!)
                : await UpdateAsync($"Partners/{Id}", partnerForm!);

            if (result.isSuccess)
            {
                if (Id == 0)
                    partnerForm!.Id = result.obj!.Id;

                if (partnerForm!.UploadedImage is not null)
                    await UploadImage("Partners", partnerForm.Id, partnerForm.UploadedImage);

                NavigationManager.NavigateTo("/Partners");
            }

            StopProcessing();
        }

        private void CaptureUploadedImage(IBrowserFile image) => partnerForm!.UploadedImage = image;

        private void ClearUploadedImage() => partnerForm!.UploadedImage = null;
    }
}
