namespace Dashboard.Blazor.Pages.Coupons;

public partial class CouponForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private CouponDto? couponForm;

    protected override async Task OnParametersSetAsync()
    {
        couponForm = (Id == 0) ? new() : new()
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

        if (couponForm is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Coupons"], href: "/Coupons", icon: EntityIcons.CouponIcon),
            new(languageContainer.Keys[Id == 0 ? "Add Coupon" : $"Edit {couponForm.Title}"], href: null, disabled: true),
        });
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        bool result;
        CouponDto? couponDtoResult;

        if (Id == 0)
            (result, couponDtoResult) = await AddAsync("Coupons", couponForm!);
        else
            (result, couponDtoResult) = await UpdateAsync($"Coupons/{Id}", couponForm!);

        if (result)
        {
            if (Id == 0)
                couponForm!.Id = couponDtoResult!.Id;

            if (couponForm.UploadedImage is not null)
                await UploadImage("Coupons", couponForm.Id, couponForm.UploadedImage);

            NavigationManager.NavigateTo("/Coupons");
        }

        StopProcessing();
    }

    private void CaptureUploadedImage(IBrowserFile image) => couponForm!.UploadedImage = image;

    private void ClearUploadedImage() => couponForm!.UploadedImage = null;
}
