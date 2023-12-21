namespace Dashboard.Blazor.Pages.Coupons;

public partial class CouponDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private CouponDto? coupon;
    private readonly string formUri = "Coupons/Form";

    protected override async Task OnParametersSetAsync()
    {
        //coupon = await GetByIdAsync<CouponDto>($"Coupons/{Id}");

        coupon = new CouponDto()
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
        };

        if (coupon is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Coupons"], href: "/Coupons", icon: EntityIcons.CouponIcon),
            new(coupon.Id.ToString(), href: null, disabled: true),
        });
    }
}
