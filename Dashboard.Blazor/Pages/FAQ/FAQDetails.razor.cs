namespace Dashboard.Blazor.Pages.FAQ
{
    public partial class FAQDetails
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private FAQDto? faq;
        private readonly string formUri = "FAQ/Form";

        protected override async Task OnParametersSetAsync()
        {
            faq = await GetByIdAsync<FAQDto>($"FAQS/{Id}");

            if (faq is null)
                return;

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["FAQ"], href: "/FAQ", icon: EntityIcons.FAQIcon),
                new($"{faq.Question}", href: null, disabled: true),
            });
        }
    }
}
