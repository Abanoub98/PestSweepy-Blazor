namespace Dashboard.Blazor.Pages.Reviews;

public partial class Reviews
{
    private List<ReviewDto> reviews = new();

    private readonly string detailsUri = "Reviews/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Reviews"], href: null, disabled: true, icon: EntityIcons.ReviewIcon),
        });

        //reviews = await GetAllAsync<ReviewDto>("Reviews?OrderBy=id&Asc=false");

        reviews = new()
        {
            new ReviewDto()
            {
                Id = 1,
                Date = DateTime.Now,
                Feedback = "Awful experience, everything stuck, cooked evenly. The only problem is that it's not really a good service. I put paper in one corner of the cabinet and put a piece of paper in the other corner.",
                Title = "Very Bad Service",
                Rate = 2,
                ShowInHomePage = false,
                Client = await GetByIdAsync<ClientDto>("Clients/52")
            },

            new ReviewDto()
            {
                Id = 2,
                Date = DateTime.Now.AddDays(-5),
                Feedback = "Love this! Well made, sturdy, and very comfortable. I love it!Very pretty, Just as expected! Looks great and has the design to make it a nice place for the baby to",
                Title = "Good Service",
                Rate = 4.5,
                ShowInHomePage = true,
                Client =  await GetByIdAsync<ClientDto>("Clients/52")
            }
        };

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = /*await DeleteAsync<ReviewDto>($"Reviews/{id}")*/ true;
        if (isSuccess)
        {
            reviews.Remove(reviews.FirstOrDefault(x => x.Id == id)!);
        }

        StopProcessing();
    }

    private async Task ToggleStatus(ReviewDto review)
    {
        StartProcessing();

        /*await DeleteAsync<ReviewDto>($"Reviews/{id}")*/

        var isSuccess = await ShowConfirmation($"Are You Sure That You Will {(review.ShowInHomePage ? "Hide" : "Hide")} This Review");

        if (isSuccess)
            review.ShowInHomePage = !review.ShowInHomePage;

        StopProcessing();
    }

    private bool FilterFunc(ReviewDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;

        return false;
    }
}
