namespace Dashboard.Blazor.Pages.FAQ;

public partial class FAQ
{
    private List<FAQDto> faq = new();

    private readonly string formUri = "FAQ/Form";
    private readonly string detailsUri = "FAQ/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["FAQ"], href: null, disabled: true, icon: EntityIcons.CategoriesIcon),
        };

        faq = await GetAllAsync<FAQDto>("FAQS?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<FAQDto>($"FAQS/{id}");

        if (isSuccess)
            faq.Remove(faq.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<FAQDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<FAQDto>($"FAQS/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            faq.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }
    private bool FilterFunc(FAQDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Question.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Answer.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.OrderIndex.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
