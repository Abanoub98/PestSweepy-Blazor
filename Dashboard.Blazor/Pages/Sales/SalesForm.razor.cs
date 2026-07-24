namespace Dashboard.Blazor.Pages.Sales;

public partial class SalesForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private SalesDto? salesForm;

    protected override async Task OnParametersSetAsync()
    {
        salesForm = (Id == 0) ? new() : await GetByIdAsync<SalesDto>($"Sales/{Id}");

        if (salesForm is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Sales"], href: "/Sales", icon: Icons.Material.Outlined.Diversity3),
            new(Id == 0 ? $"{languageContainer.Keys["Add"]} {languageContainer.Keys["Sales"]}" : $"{languageContainer.Keys["Edit"]} {salesForm.FirstName} {salesForm.LastName}", href: null, disabled: true),
        });
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        salesForm!.NationalityId = salesForm.Nationality!.Id;

        bool result;
        SalesDto? salesDtoResult;

        if (Id == 0)
            (result, salesDtoResult) = await AddAsync("Sales", salesForm!);
        else
            (result, salesDtoResult) = await UpdateAsync($"Sales/{Id}", salesForm!);

        if (result)
        {
            if (Id == 0)
                salesForm!.Id = salesDtoResult!.Id;

            if (salesForm.UploadedImage is not null)
                await UploadImage("Sales", salesForm.Id, salesForm.UploadedImage);

            NavigationManager.NavigateTo("/Sales");
        }

        StopProcessing();
    }

    private void CaptureUploadedImage(IBrowserFile image) => salesForm!.UploadedImage = image;

    private void ClearUploadedImage() => salesForm!.UploadedImage = null;

    private async Task<IEnumerable<LookupDto>> GetNationalities(string value)
    {
        if (salesForm!.Nationalities is null)
            salesForm.Nationalities = await GetAllLookupsAsync("ReferenceData?tableName=Nationalities");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return salesForm.Nationalities;

        return salesForm.Nationalities.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}
