namespace Dashboard.Blazor.Pages.OurTeam
{
    public partial class OurTeamForm
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private OurTeamDto? ourTeamForm;

        protected override async Task OnParametersSetAsync()
        {
            ourTeamForm = (Id == 0)
                ? new()
                : await GetByIdAsync<OurTeamDto>($"OurTeam/{Id}");

            if (ourTeamForm is null)
                return;

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["OurTeam"], href: "/OurTeam", icon: EntityIcons.OurTeamIcon),
                new(
                    Id == 0
                        ? $"{languageContainer.Keys["Add"]} {languageContainer.Keys["Project"]}"
                        : $"{languageContainer.Keys["Edit"]} {ourTeamForm.Name}",
                    href: null,
                    disabled: true
                ),
            });
        }

        private async Task OnValidSubmit(EditContext context)
        {
            StartProcessing();

            var result = (Id == 0)
                ? await AddAsync("OurTeam", ourTeamForm!)
                : await UpdateAsync($"OurTeam/{Id}", ourTeamForm!);

            if (result.isSuccess)
            {
                if (Id == 0)
                    ourTeamForm!.Id = result.obj!.Id;

                if (ourTeamForm!.UploadedImage is not null)
                    await UploadImage("OurTeam", ourTeamForm.Id, ourTeamForm.UploadedImage);

                NavigationManager.NavigateTo("/OurTeam");
            }

            StopProcessing();
        }

        private void CaptureUploadedImage(IBrowserFile image) => ourTeamForm!.UploadedImage = image;

        private void ClearUploadedImage() => ourTeamForm!.UploadedImage = null;
    }
}
