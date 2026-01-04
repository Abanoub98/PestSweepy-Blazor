namespace Dashboard.Blazor.Pages.Blogs
{
    public partial class BlogsForm
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private BlogsDto? blogForm;

        protected override async Task OnParametersSetAsync()
        {
            blogForm = (Id == 0)
                ? new()
                : await GetByIdAsync<BlogsDto>($"Blogs/{Id}");

            if (blogForm is null)
                return;

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["Blogs"], href: "/Blogs", icon: EntityIcons.BlogsIcon),
                new(
                    Id == 0
                        ? $"{languageContainer.Keys["Add"]} {languageContainer.Keys["Project"]}"
                        : $"{languageContainer.Keys["Edit"]} {blogForm.Title}",
                    href: null,
                    disabled: true
                ),
            });
        }

        private async Task OnValidSubmit(EditContext context)
        {
            StartProcessing();

            var result = (Id == 0)
                ? await AddAsync("Blogs", blogForm!)
                : await UpdateAsync($"Blogs/{Id}", blogForm!);

            if (result.isSuccess)
            {
                if (Id == 0)
                    blogForm!.Id = result.obj!.Id;

                if (blogForm!.UploadedImage is not null)
                    await UploadImage("Blogs", blogForm.Id, blogForm.UploadedImage);

                NavigationManager.NavigateTo("/Blogs");
            }

            StopProcessing();
        }

        private void CaptureUploadedImage(IBrowserFile image) => blogForm!.UploadedImage = image;

        private void ClearUploadedImage() => blogForm!.UploadedImage = null;
    }
}
