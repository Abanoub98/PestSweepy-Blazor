namespace Dashboard.Blazor.Pages.Certificates
{
    public partial class CertificatesDetails
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private CertificatesDto? certificate;
        private readonly string formUri = "Certificates/Form";

        protected override async Task OnParametersSetAsync()
        {
            certificate = await GetByIdAsync<CertificatesDto>($"Certificates/{Id}");

            if (certificate is null)
                return;

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["Certificates"], href: "/Certificates", icon: EntityIcons.CertificatesIcon),
                new($"{certificate.NameEn} - {certificate.NameAr}", href: null, disabled: true),
            });
        }
    }
}
