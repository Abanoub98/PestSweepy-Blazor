namespace Dashboard.Blazor.Pages.Projects
{
    public partial class ProjectsForm
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private ProjectsDto? projectForm;

        protected override async Task OnParametersSetAsync()
        {
            projectForm = (Id == 0)
                ? new()
                : await GetByIdAsync<ProjectsDto>($"Projects/{Id}");

            if (projectForm is null)
                return;

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["Projects"], href: "/Projects", icon: EntityIcons.ProjectsIcon),
                new(
                    Id == 0
                        ? $"{languageContainer.Keys["Add"]} {languageContainer.Keys["Project"]}"
                        : $"{languageContainer.Keys["Edit"]} {projectForm.NameEn} - {projectForm.NameAr}",
                    href: null,
                    disabled: true
                ),
            });
        }

        private async Task OnValidSubmit(EditContext context)
        {
            StartProcessing();

            var result = (Id == 0)
                ? await AddAsync("Projects", projectForm!)
                : await UpdateAsync($"Projects/{Id}", projectForm!);

            if (result.isSuccess)
            {
                if (Id == 0)
                    projectForm!.Id = result.obj!.Id;

                if (projectForm!.UploadedImage is not null)
                    await UploadImage("Projects", projectForm.Id, projectForm.UploadedImage);

                NavigationManager.NavigateTo("/Projects");
            }

            StopProcessing();
        }

        private void CaptureUploadedImage(IBrowserFile image) => projectForm!.UploadedImage = image;

        private void ClearUploadedImage() => projectForm!.UploadedImage = null;
    }
}
