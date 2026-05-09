namespace Dashboard.Blazor.Pages.Branches;

public partial class BranchesForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private BranchDto? branchForm;

    protected override async Task OnParametersSetAsync()
    {
        branchForm = (Id == 0) ? new() : await GetByIdAsync<BranchDto>($"Branches/{Id}");

        if (branchForm is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Branches"], href: "/Branches", icon: Icons.Material.Outlined.Diversity3),
            new(Id == 0 ? $"{languageContainer.Keys["Add"]} {languageContainer.Keys["Branch"]}" : $"{languageContainer.Keys["Edit"]} {branchForm.Name}", href: null, disabled: true),
        });
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();
        branchForm!.ClientId = branchForm.Client!.Id;
        branchForm!.CityId = branchForm.City!.Id;

        bool result;
        BranchDto? branchDtoResult;

        if (Id == 0)
            (result, branchDtoResult) = await AddAsync("Branches", branchForm!);
        else
            (result, branchDtoResult) = await UpdateAsync($"Branches/{Id}", branchForm!);

        if (result)
        {
            if (Id == 0)
                branchForm!.Id = branchDtoResult!.Id;

            if (branchForm.UploadedImage is not null)
                await UploadImage("Branches", branchForm.Id, branchForm.UploadedImage);

            NavigationManager.NavigateTo("/Branches");
        }

        StopProcessing();
    }

    private void CaptureUploadedImage(IBrowserFile image) => branchForm!.UploadedImage = image;

    private void ClearUploadedImage() => branchForm!.UploadedImage = null;

    private async Task<IEnumerable<LookupDto>> GetCities(string value)
    {
        if (branchForm!.Cities is null)
            branchForm.Cities = await GetAllLookupsAsync("ReferenceData?tableName=Cities");

        if (string.IsNullOrEmpty(value))
            return branchForm.Cities;

        return branchForm.Cities
            .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task<IEnumerable<LookupDto>> GetContractClients(string value)
    {
        if (branchForm!.Clients is null)
            branchForm.Clients = await GetAllLookupsAsync("/ContractClients");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return branchForm.Clients;

        return branchForm.Clients.Where(x => (x.FirstName?.Contains(value, StringComparison.InvariantCultureIgnoreCase) ?? false)
            || (x.LastName?.Contains(value, StringComparison.InvariantCultureIgnoreCase) ?? false));
    }
}
