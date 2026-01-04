namespace Dashboard.Blazor.Pages.Partners
{
    public partial class PartnersDetails
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private PartnersDto? partner;
        private readonly string formUri = "Partners/Form";

        protected override async Task OnParametersSetAsync()
        {
            partner = await GetByIdAsync<PartnersDto>($"Partners/{Id}");

            if (partner is null)
                return;

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["Partners"], href: "/Partners", icon: EntityIcons.PartnersIcon),
                new($"{partner.NameEn} - {partner.NameAr}", href: null, disabled: true),
            });
        }
    }
}
