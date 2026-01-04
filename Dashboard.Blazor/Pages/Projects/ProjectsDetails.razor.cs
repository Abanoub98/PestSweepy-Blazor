namespace Dashboard.Blazor.Pages.Projects
{
    public partial class ProjectsDetails
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private ProjectsDto? project;
        private readonly string formUri = "Projects/Form";

        protected override async Task OnParametersSetAsync()
        {
            project = await GetByIdAsync<ProjectsDto>($"Projects/{Id}");

            if (project is null)
                return;

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["Projects"], href: "/Projects", icon: EntityIcons.ProjectsIcon),
                new($"{project.NameEn} - {project.NameAr}", href: null, disabled: true),
            });
        }
    }
}
