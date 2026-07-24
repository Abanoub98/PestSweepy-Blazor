namespace Dashboard.Blazor.Pages.OperationManagers;

public partial class OperationManagersForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private OperationManagerDto? operationManagerForm;

    protected override async Task OnParametersSetAsync()
    {
        operationManagerForm = (Id == 0) ? new() : await GetByIdAsync<OperationManagerDto>($"OperationManagers/{Id}");

        if (operationManagerForm is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["OperationManagers"], href: "/OperationManagers", icon: Icons.Material.Outlined.Diversity3),
            new(Id == 0 ? $"{languageContainer.Keys["Add"]} {languageContainer.Keys["OperationManager"]}" : $"{languageContainer.Keys["Edit"]} {operationManagerForm.FirstName} {operationManagerForm.LastName}", href: null, disabled: true),
        });
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        operationManagerForm!.NationalityId = operationManagerForm.Nationality!.Id;

        bool result;
        OperationManagerDto? operationManagerDtoResult;

        if (Id == 0)
            (result, operationManagerDtoResult) = await AddAsync("OperationManagers", operationManagerForm!);
        else
            (result, operationManagerDtoResult) = await UpdateAsync($"OperationManagers/{Id}", operationManagerForm!);

        if (result)
        {
            if (Id == 0)
                operationManagerForm!.Id = operationManagerDtoResult!.Id;

            if (operationManagerForm.UploadedImage is not null)
                await UploadImage("OperationManagers", operationManagerForm.Id, operationManagerForm.UploadedImage);

            NavigationManager.NavigateTo("/OperationManagers");
        }

        StopProcessing();
    }

    private void CaptureUploadedImage(IBrowserFile image) => operationManagerForm!.UploadedImage = image;

    private void ClearUploadedImage() => operationManagerForm!.UploadedImage = null;

    private async Task<IEnumerable<LookupDto>> GetNationalities(string value)
    {
        if (operationManagerForm!.Nationalities is null)
            operationManagerForm.Nationalities = await GetAllLookupsAsync("ReferenceData?tableName=Nationalities");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return operationManagerForm.Nationalities;

        return operationManagerForm.Nationalities.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}
