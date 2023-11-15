namespace Dashboard.Blazor.Pages.Managers;

public partial class ManagerForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ManagerDto? managerForm;

    protected override async Task OnParametersSetAsync()
    {
        if (Id == 0)
            managerForm = new();
        else
            managerForm = await GetByIdAsync($"Managers/{Id}");

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["Managers"], href: "/Managers", icon: Icons.Material.TwoTone.Diversity3),
            new BreadcrumbItem(languageContainer.Keys[Id == 0 ? "Add Manager" : $"Edit {managerForm.Name}"], href: null, disabled: true),
        });
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        managerForm!.NationalityId = managerForm.Nationality!.Id;

        bool result;
        ManagerDto? managerDtoResult;

        if (Id == 0)
            (result, managerDtoResult) = await AddAsync("Managers", managerForm!);
        else
            (result, managerDtoResult) = await UpdateAsync($"Managers/{Id}", managerForm!);

        if (result)
        {
            if (Id == 0)
                managerForm!.Id = managerDtoResult!.Id;

            if (managerForm.UploadedImage is not null)
                await UploadImage("Managers", managerForm.Id, managerForm.UploadedImage);

            NavigationManager.NavigateTo("/Managers");
        }

        StopProcessing();
    }

    private void CaptureUploadedImage(IBrowserFile image) => managerForm!.UploadedImage = image;

    private void ClearUploadedImage() => managerForm!.UploadedImage = null;

    private async Task<IEnumerable<LookupDto>> GetNationalities(string value)
    {
        if (managerForm!.Nationalities is null)
            managerForm.Nationalities = await GetAllLookupsAsync<LookupDto>("ReferenceData?tableName=Nationalities");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return managerForm.Nationalities;

        return managerForm.Nationalities.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}
