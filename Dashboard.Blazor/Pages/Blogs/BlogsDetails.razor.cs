namespace Dashboard.Blazor.Pages.Blogs
{
    public partial class BlogsDetails
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private BlogsDto? blog;
        private readonly string formUri = "Blogs/Form";

        protected override async Task OnParametersSetAsync()
        {
            blog = await GetByIdAsync<BlogsDto>($"Blogs/{Id}");

            if (blog is null)
                return;

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["Blogs"], href: "/Blogs", icon: EntityIcons.BlogsIcon),
                new($"{blog.Title}", href: null, disabled: true),
            });
        }
    }
}
