namespace Dashboard.Blazor.Pages.QuoteRequests
{
    public partial class QuoteRequestsDetails
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private QuoteRequestsDto? quoteRequest;
        private readonly string formUri = "QuoteRequests/Form";

        protected override async Task OnParametersSetAsync()
        {
            quoteRequest = await GetByIdAsync<QuoteRequestsDto>($"QuoteRequests/{Id}");

            if (quoteRequest is null)
                return;

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["QuoteRequests"], href: "/QuoteRequests", icon: EntityIcons.QuoteRequests),
                new($"{quoteRequest.Name}", href: null, disabled: true),
            });
        }
    }
}
