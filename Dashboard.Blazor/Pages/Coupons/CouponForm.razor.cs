namespace Dashboard.Blazor.Pages.Coupons;

public partial class CouponForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private CouponDto? couponForm;

    protected override async Task OnParametersSetAsync()
    {
        couponForm = (Id == 0) ? new() : await GetByIdAsync<CouponDto>($"Coupons/{Id}");

        if (couponForm is null)
            return;

        couponForm.Services = await GetAllAsync<ServiceDto>("Services");

        if (Id != 0)
        {
            couponForm.StartEndDateRange!.Start = couponForm.StartDate;
            couponForm.StartEndDateRange.End = couponForm.EndDate;

            couponForm.SelectedServices = couponForm.CouponServices.Select(s => s.Service.Id);
        }

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Coupons"], href: "/Coupons", icon: EntityIcons.CouponIcon),
            new(Id == 0 ? $"{languageContainer.Keys["Add"]} {languageContainer.Keys["Coupons"]}" : $"{languageContainer.Keys["Edit"]} {couponForm.CouponCode}", href: null, disabled: true),
        });
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        couponForm!.StartDate = couponForm.StartEndDateRange!.Start!.Value;
        couponForm!.EndDate = couponForm.StartEndDateRange!.End!.Value;
        couponForm.CurrencyId = couponForm.Currency!.Id;
        couponForm.CouponServices = couponForm.SelectedServices.Select(s => new CouponService() { ServiceId = s }).ToList();

        bool result;
        CouponDto? couponDtoResult;

        if (Id == 0)
            (result, couponDtoResult) = await AddAsync("Coupons", couponForm);
        else
            (result, couponDtoResult) = await UpdateAsync($"Coupons/{Id}", couponForm);

        if (result)
        {
            if (Id == 0)
                couponForm.Id = couponDtoResult!.Id;

            if (couponForm.UploadedImage is not null)
                await UploadImage("Coupons", couponForm.Id, couponForm.UploadedImage);

            NavigationManager.NavigateTo("/Coupons");
        }

        StopProcessing();
    }

    private void CaptureUploadedImage(IBrowserFile image) => couponForm!.UploadedImage = image;

    private void ClearUploadedImage() => couponForm!.UploadedImage = null;

    private async Task<IEnumerable<CurrencyDto>> GetCurrencies(string value)
    {
        if (couponForm!.Currencies is null)
            couponForm.Currencies = await GetAllAsync<CurrencyDto>("Currencies");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return couponForm.Currencies;

        return couponForm.Currencies.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private string GetMultiSelectionText(List<string> selectedValues)
    {
        return $"{selectedValues.Count} service{(selectedValues.Count > 1 ? "s have" : " has")} been selected";
    }
}
