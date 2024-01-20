namespace Dashboard.Blazor.Pages.Clients;

public partial class ClientForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ClientDto? clientForm;

    protected override async Task OnParametersSetAsync()
    {
        clientForm = (Id == 0) ? new() : await GetByIdAsync<ClientDto>($"Clients/{Id}");

        if (clientForm is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Individual Clients"], href: "/IndividualClients", icon: Icons.Material.Outlined.Diversity1),
            new(languageContainer.Keys[Id == 0 ? "Add Individual Client" : $"Edit {clientForm.FirstName} {clientForm.LastName}"], href: null, disabled: true),
        });
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        clientForm!.NationalityId = clientForm.Nationality!.Id;
        clientForm!.CountryId = clientForm.Country!.Id;

        var result = Id == 0 ?
            await AddAsync("Clients", clientForm!) :
            await UpdateAsync($"Clients/{Id}", clientForm!);

        if (result.isSuccess)
        {
            if (Id == 0)
                clientForm!.Id = result.obj!.Id;

            if (clientForm.UploadedImage is not null)
                await UploadImage("Clients", clientForm.Id, clientForm.UploadedImage);

            NavigationManager.NavigateTo("/Clients");
        }

        StopProcessing();
    }

    private void CaptureUploadedImage(IBrowserFile image) => clientForm!.UploadedImage = image;

    private void ClearUploadedImage() => clientForm!.UploadedImage = null;

    private async Task<IEnumerable<LookupDto>> GetNationalities(string value)
    {
        if (clientForm!.Nationalities is null)
            clientForm.Nationalities = await GetAllLookupsAsync("ReferenceData?tableName=Nationalities");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return clientForm.Nationalities;

        return clientForm.Nationalities.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
    private async Task<IEnumerable<LookupDto>> GetCountries(string value)
    {
        if (clientForm!.Countries is null)
            clientForm.Countries = await GetAllLookupsAsync("ReferenceData?tableName=Countries");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return clientForm.Countries;

        return clientForm.Countries.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}
