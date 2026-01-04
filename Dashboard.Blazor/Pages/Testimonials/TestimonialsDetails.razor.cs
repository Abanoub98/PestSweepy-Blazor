namespace Dashboard.Blazor.Pages.Testimonials
{
    public partial class TestimonialsDetails
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private TestimonialsDto? testimonial;
        private readonly string formUri = "Testimonials/Form";

        protected override async Task OnParametersSetAsync()
        {
            testimonial = await GetByIdAsync<TestimonialsDto>($"Testimonials/{Id}");

            if (testimonial is null)
                return;

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["Testimonials"], href: "/Testimonials", icon: EntityIcons.TestimonialsIcon),
                new($"{testimonial.Name}", href: null, disabled: true),
            });
        }
    }
}
