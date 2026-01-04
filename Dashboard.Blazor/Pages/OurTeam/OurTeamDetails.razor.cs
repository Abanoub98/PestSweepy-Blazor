namespace Dashboard.Blazor.Pages.OurTeam
{
    public partial class OurTeamDetails
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private OurTeamDto? ourTeam;
        private readonly string formUri = "OurTeam/Form";

        protected override async Task OnParametersSetAsync()
        {
            ourTeam = await GetByIdAsync<OurTeamDto>($"OurTeam/{Id}");

            if (ourTeam is null)
                return;

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["OurTeam"], href: "/OurTeam", icon: EntityIcons.OurTeamIcon),
                new($"{ourTeam.Name}", href: null, disabled: true),
            });
        }
    }
}
