namespace Dashboard.Blazor.Pages.Providers;

public partial class ProviderForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ProviderDto? providerForm;

    protected override async Task OnParametersSetAsync()
    {
        providerForm = (Id == 0) ? new() : await GetByIdAsync<ProviderDto>($"Providers/{Id}");

        if (providerForm is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Providers"], href: "/Providers", icon: Icons.Material.TwoTone.Engineering),
            new(languageContainer.Keys[Id == 0 ? "Add Provider" : $"Edit {providerForm.Name}"], href: null, disabled: true),
        });
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        providerForm!.NationalityId = providerForm.Nationality!.Id;
        providerForm!.SupervisorId = providerForm.Supervisor!.Id;

        bool result;
        ProviderDto? providerDtoResult;

        if (Id == 0)
            (result, providerDtoResult) = await AddAsync("Providers", providerForm!);
        else
            (result, providerDtoResult) = await UpdateAsync($"Providers/{Id}", providerForm!);

        if (result)
        {
            if (Id == 0)
                providerForm!.Id = providerDtoResult!.Id;

            if (providerForm.UploadedImage is not null)
                await UploadImage("Providers", providerForm.Id, providerForm.UploadedImage);

            NavigationManager.NavigateTo("/Providers");
        }

        StopProcessing();
    }

    private void CaptureUploadedImage(IBrowserFile image) => providerForm!.UploadedImage = image;

    private void ClearUploadedImage() => providerForm!.UploadedImage = null;

    private async Task<IEnumerable<LookupDto>> GetNationalities(string value)
    {
        if (providerForm!.Nationalities is null)
            providerForm.Nationalities = await GetAllLookupsAsync("ReferenceData?tableName=Nationalities");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return providerForm.Nationalities;

        return providerForm.Nationalities.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task<IEnumerable<LookupDto>> GetManagers(string value)
    {
        if (providerForm!.Supervisors is null)
            providerForm.Supervisors = await GetAllLookupsAsync("/Supervisors");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return providerForm.Supervisors;

        return providerForm.Supervisors.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}
