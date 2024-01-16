namespace Dashboard.Blazor.Pages.Supervisors;

public partial class SupervisorForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private SupervisorDto? supervisorForm;

    protected override async Task OnParametersSetAsync()
    {
        supervisorForm = (Id == 0) ? new() : supervisorForm = await GetByIdAsync<SupervisorDto>($"Supervisors/{Id}");

        if (supervisorForm is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Supervisors"], href: "/Supervisors", icon: Icons.Material.Outlined.SupervisorAccount),
            new(languageContainer.Keys[Id == 0 ? "Add Supervisor" : $"Edit {supervisorForm.FirstName} {supervisorForm.LastName}"], href: null, disabled: true),
        });
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        supervisorForm!.NationalityId = supervisorForm.Nationality!.Id;
        supervisorForm!.ManagerId = supervisorForm.Manager!.Id;

        bool result;
        SupervisorDto? supervisorDtoResult;

        if (Id == 0)
            (result, supervisorDtoResult) = await AddAsync("Supervisors", supervisorForm!);
        else
            (result, supervisorDtoResult) = await UpdateAsync($"Supervisors/{Id}", supervisorForm!);

        if (result)
        {
            if (Id == 0)
                supervisorForm!.Id = supervisorDtoResult!.Id;

            if (supervisorForm.UploadedImage is not null)
                await UploadImage("Supervisors", supervisorForm.Id, supervisorForm.UploadedImage);

            NavigationManager.NavigateTo("/Supervisors");
        }

        StopProcessing();
    }

    private void CaptureUploadedImage(IBrowserFile image) => supervisorForm!.UploadedImage = image;

    private void ClearUploadedImage() => supervisorForm!.UploadedImage = null;

    private async Task<IEnumerable<LookupDto>> GetNationalities(string value)
    {
        if (supervisorForm!.Nationalities is null)
            supervisorForm.Nationalities = await GetAllLookupsAsync("ReferenceData?tableName=Nationalities");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return supervisorForm.Nationalities;

        return supervisorForm.Nationalities.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task<IEnumerable<LookupDto>> GetManagers(string value)
    {
        if (supervisorForm!.Managers is null)
            supervisorForm.Managers = await GetAllLookupsAsync("/Managers");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return supervisorForm.Managers;

        return supervisorForm.Managers.Where(x => x.FirstName.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.LastName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}
