namespace Dashboard.Blazor.Pages.ServiceRequests
{
    public partial class ServiceRequestsDetails
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private ServiceRequestsDto? serviceRequest;
        private readonly string formUri = "ServiceRequests/Form";

        protected override async Task OnParametersSetAsync()
        {
            serviceRequest = await GetByIdAsync<ServiceRequestsDto>($"ServiceRequests/{Id}");

            if (serviceRequest is null)
                return;

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["ServiceRequests"], href: "/ServiceRequests", icon: EntityIcons.ServiceRequestsIcon),
                new($"{serviceRequest.EntityName}", href: null, disabled: true),
            });
        }
    }
}
