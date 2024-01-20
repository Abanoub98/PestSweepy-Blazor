namespace Dashboard.Blazor.Pages.Reviews;

public partial class ReviewDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ReviewDto? review;

    protected override async Task OnParametersSetAsync()
    {
        review = await GetByIdAsync<ReviewDto>($"Reviews/{Id}");

        if (review is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Reviews"], href: "/Reviews", icon: EntityIcons.ReviewIcon),
            new(review.Id.ToString(), href: null, disabled: true),
        });
    }
}
