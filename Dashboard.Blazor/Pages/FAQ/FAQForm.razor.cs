namespace Dashboard.Blazor.Pages.FAQ
{
    public partial class FAQForm
    {
        [Parameter]
        [EditorRequired]
        public int Id { get; set; }

        private FAQDto? faqForm;

        protected override async Task OnParametersSetAsync()
        {
            faqForm = (Id == 0)
                ? new()
                : await GetByIdAsync<FAQDto>($"FAQS/{Id}");

            if (faqForm is null)
                return;

            breadcrumbItems.AddRange(new List<BreadcrumbItem>
            {
                new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(languageContainer.Keys["FAQ"], href: "/FAQ", icon: EntityIcons.FAQIcon),
                new(
                    Id == 0
                        ? $"{languageContainer.Keys["Add"]} {languageContainer.Keys["Project"]}"
                        : $"{languageContainer.Keys["Edit"]} {faqForm.Question}",
                    href: null,
                    disabled: true
                ),
            });
        }

        private async Task OnValidSubmit(EditContext context)
        {
            StartProcessing();

            var result = (Id == 0)
                ? await AddAsync("FAQS", faqForm!)
                : await UpdateAsync($"FAQS/{Id}", faqForm!);

            if (result.isSuccess)
            {
                if (Id == 0)
                    faqForm!.Id = result.obj!.Id;

                NavigationManager.NavigateTo("/FAQ");
            }

            StopProcessing();
        }
    }
}
