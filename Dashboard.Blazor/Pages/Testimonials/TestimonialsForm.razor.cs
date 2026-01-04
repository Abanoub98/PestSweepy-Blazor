namespace Dashboard.Blazor.Pages.Testimonials
{
    public partial class TestimonialsForm
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private TestimonialsDto? testimonialForm;

        protected override async Task OnParametersSetAsync()
        {
            testimonialForm = (Id == 0)
                ? new()
                : await GetByIdAsync<TestimonialsDto>($"Testimonials/{Id}");

            if (testimonialForm is null)
                return;

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["Testimonials"], href: "/Testimonials", icon: EntityIcons.TestimonialsIcon),
                new(
                    Id == 0
                        ? $"{languageContainer.Keys["Add"]} {languageContainer.Keys["Project"]}"
                        : $"{languageContainer.Keys["Edit"]} {testimonialForm.Name}",
                    href: null,
                    disabled: true
                ),
            });
        }

        private async Task OnValidSubmit(EditContext context)
        {
            StartProcessing();

            var result = (Id == 0)
                ? await AddAsync("Testimonials", testimonialForm!)
                : await UpdateAsync($"Testimonials/{Id}", testimonialForm!);

            if (result.isSuccess)
            {
                if (Id == 0)
                    testimonialForm!.Id = result.obj!.Id;

                if (testimonialForm!.UploadedImage is not null)
                    await UploadImage("Testimonials", testimonialForm.Id, testimonialForm.UploadedImage);

                NavigationManager.NavigateTo("/Testimonials");
            }

            StopProcessing();
        }

        private void CaptureUploadedImage(IBrowserFile image) => testimonialForm!.UploadedImage = image;

        private void ClearUploadedImage() => testimonialForm!.UploadedImage = null;
    }
}
