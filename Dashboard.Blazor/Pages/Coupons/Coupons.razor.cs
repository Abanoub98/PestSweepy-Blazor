namespace Dashboard.Blazor.Pages.Coupons;

public partial class Coupons
{
    private List<CouponDto> coupons = new();

    private readonly string detailsUri = "Coupons/Details";
    private readonly string formUri = "Coupons/Form";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Coupons"], href: null, disabled: true, icon: EntityIcons.CouponIcon),
        };

        coupons = await GetAllAsync<CouponDto>("Coupons?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<CouponDto>($"Coupons/{id}");

        if (isSuccess)
            coupons.Remove(coupons.FirstOrDefault(x => x.Id == id)!);


        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<CouponDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<CouponDto>($"Coupons/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            coupons.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }

    private async Task ToggleStatus(CouponDto coupon)
    {
        StartProcessing();

        var isSuccess = await ShowConfirmation($"Are You Sure That You Will {(coupon.IsActive ? "deactivate" : "activate")} This Coupon");

        if (isSuccess)
            coupon.IsActive = !coupon.IsActive;

        StopProcessing();
    }

    private bool FilterFunc(CouponDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;

        return false;
    }
}
