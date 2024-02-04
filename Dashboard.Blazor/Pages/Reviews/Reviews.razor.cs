namespace Dashboard.Blazor.Pages.Reviews;

public partial class Reviews
{
    private List<ReviewDto> reviews = new();

    private readonly string detailsUri = "Reviews/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Reviews"], href: null, disabled: true, icon: EntityIcons.ReviewIcon),
        };

        reviews = await GetAllAsync<ReviewDto>("Reviews?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<ReviewDto>($"Reviews/{id}");

        if (isSuccess)
        {
            reviews.Remove(reviews.FirstOrDefault(x => x.Id == id)!);

            if (selectedIds.Contains(id))
                selectedIds.Remove(id);
        }

        StopProcessing();
    }
    private void SelectedItemsChanged(HashSet<ReviewDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<ReviewDto>($"Reviews/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            reviews.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }

    private async Task ToggleStatus(ReviewDto review)
    {
        StartProcessing();

        var isSuccess = await ShowConfirmation(confirmationMessage: $"Are you sure that you will {(review.ShowInApp ? "hide" : "show")} this review", isWarning: true);

        if (isSuccess)
        {
            var result = await UpdateAsync($"Reviews/ShowReviewInApp/{review.Id}", review);

            if (result.isSuccess)
            {
                review.ShowInApp = !review.ShowInApp;
            }
        }

        StopProcessing();
    }

    private bool FilterFunc(ReviewDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;

        return false;
    }
}
