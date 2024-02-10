namespace Dashboard.Blazor.Pages.ContractClients;

public partial class ContractClientForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ClientDto? contractClientForm;
    private string countryCode = @"^((00|\+)?966)?\d{9}$";

    protected override async Task OnParametersSetAsync()
    {
        contractClientForm = (Id == 0) ? new() : await GetByIdAsync<ClientDto>($"ContractClients/{Id}");

        if (contractClientForm is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Contract Clients"], href: "/ContractClients", icon: Icons.Material.Outlined.Diversity1),
            new(languageContainer.Keys[Id == 0 ? "Add Contract Client" : $"Edit {contractClientForm.FirstName} {contractClientForm.LastName}"], href: null, disabled: true),
        });
    }
    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        contractClientForm!.NationalityId = contractClientForm.Nationality!.Id;
        contractClientForm!.CountryId = contractClientForm.Country!.Id;
        contractClientForm.PhoneNumber = countryCode + contractClientForm.PhoneNumber;

        bool result;
        ClientDto? clientDtoResult;

        if (Id == 0)
            (result, clientDtoResult) = await AddAsync("ContractClients", contractClientForm!);
        else
            (result, clientDtoResult) = await UpdateAsync($"ContractClients/{Id}", contractClientForm!);

        if (result)
        {
            if (Id == 0)
                contractClientForm!.Id = clientDtoResult!.Id;

            if (contractClientForm.UploadedImage is not null)
                await UploadImage("ContractClients", contractClientForm.Id, contractClientForm.UploadedImage);

            NavigationManager.NavigateTo("/ContractClients");
        }

        StopProcessing();
    }

    private void ChangePhoneNumber(string codeNumber)
    {
        contractClientForm!.PhoneNumber = codeNumber;
    }

    private void CaptureUploadedImage(IBrowserFile image) => contractClientForm!.UploadedImage = image;

    private void ClearUploadedImage() => contractClientForm!.UploadedImage = null;

    private async Task<IEnumerable<LookupDto>> GetNationalities(string value)
    {
        if (contractClientForm!.Nationalities is null)
            contractClientForm.Nationalities = await GetAllLookupsAsync("ReferenceData?tableName=Nationalities");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return contractClientForm.Nationalities;

        return contractClientForm.Nationalities.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
    private async Task<IEnumerable<LookupDto>> GetCountries(string value)
    {
        if (contractClientForm!.Countries is null)
            contractClientForm.Countries = await GetAllLookupsAsync("ReferenceData?tableName=Countries");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return contractClientForm.Countries;

        return contractClientForm.Countries.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}
