namespace Dashboard.Blazor.Pages.Reviews;

public partial class ReviewDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ReviewDto? review;
    private readonly string formUri = "Reviews/Form";

    protected override async Task OnParametersSetAsync()
    {
        //review = await GetByIdAsync<ReviewDto>($"Reviews/{Id}");

        review = new ReviewDto()
        {
            Id = 1,
            Date = DateTime.Now,
            Feedback = "Awful experience, everything stuck, cooked evenly. The only problem is that it's not really a good service. I put paper in one corner of the cabinet and put a piece of paper in the other corner.",
            Title = "Very Bad Service",
            Rate = 2.8,
            ShowInHomePage = false,
            Client = await GetByIdAsync<ClientDto>("Clients/52")
        };

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
