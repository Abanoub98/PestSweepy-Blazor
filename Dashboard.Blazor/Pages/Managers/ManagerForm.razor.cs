namespace Dashboard.Blazor.Pages.Managers;

public partial class ManagerForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ManagerDto? managerForm;

    protected override async Task OnParametersSetAsync()
    {
        managerForm = (Id == 0) ? new() : await GetByIdAsync<ManagerDto>($"Managers/{Id}");

        if (managerForm is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Managers"], href: "/Managers", icon: Icons.Material.Outlined.Diversity3),
            new(languageContainer.Keys[Id == 0 ? "Add Manager" : $"Edit {managerForm.FirstName} {managerForm.LastName}"], href: null, disabled: true),
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
            managerForm.Nationalities = await GetAllLookupsAsync("ReferenceData?tableName=Nationalities");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return managerForm.Nationalities;

        return managerForm.Nationalities.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}
