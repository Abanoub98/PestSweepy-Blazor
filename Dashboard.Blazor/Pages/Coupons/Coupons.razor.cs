namespace Dashboard.Blazor.Pages.Coupons;

public partial class Coupons
{
    private List<CouponDto> coupons = new();

    private readonly string detailsUri = "Coupons/Details";
    private readonly string formUri = "Coupons/Form";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Coupons"], href: null, disabled: true, icon: EntityIcons.CouponIcon),
        });

        //coupons = await GetAllAsync<CouponDto>("Coupons?OrderBy=id&Asc=false");

        coupons = new()
        {
            new CouponDto()
            {
            Title = "Sweepy50",
            Id = 3,
            ShowInHomePage = true,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(6),
            NumOfUses = 10,
            MaxDiscount = 50,
            MaxUse = 1000,
            Percentage = 20,
            Description = "Facilisis dolor sit dolore nulla diam dolor labore dignissim diam zzril sea lorem feugait sea veniam sed sadipscing illum kasd",
            NotificationMessage = "Sadipscing et feugait consectetuer esse suscipit erat dolor vero dolore"
            },

        };

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = /*await DeleteAsync<CouponDto>($"Coupons/{id}")*/ true;
        if (isSuccess)
        {
            coupons.Remove(coupons.FirstOrDefault(x => x.Id == id)!);
        }

        StopProcessing();
    }

    private async Task ToggleStatus(CouponDto coupon)
    {
        StartProcessing();

        /*await DeleteAsync<CouponDto>($"Coupons/{id}")*/

        var isSuccess = await ShowConfirmation($"Are You Sure That You Will {(coupon.ShowInHomePage ? "Hide" : "Show")} This Coupon");

        if (isSuccess)
            coupon.ShowInHomePage = !coupon.ShowInHomePage;

        StopProcessing();
    }

    private bool FilterFunc(CouponDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;

        return false;
    }
}
