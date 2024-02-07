namespace Dashboard.Blazor.Pages.Coupons;

public partial class CouponDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private CouponDto? coupon;
    private readonly string formUri = "Coupons/Form";

    protected override async Task OnParametersSetAsync()
    {
        coupon = await GetByIdAsync<CouponDto>($"Coupons/{Id}");

        if (coupon is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Coupons"], href: "/Coupons", icon: EntityIcons.CouponIcon),
            new(coupon.CouponCode.ToString(), href: null, disabled: true),
        });
    }
}
