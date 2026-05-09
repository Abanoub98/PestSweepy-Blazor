namespace Dashboard.Blazor.Pages.ContractClients;

public partial class ContractClientForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ContractClientDto? contractClientForm;
    private string countryCode = @"^((00|\+)?966)?\d{9}$"; // this is PATTERN (keep as-is)

    private readonly Dictionary<string, string> DialCodeByPattern = new()
{
    { @"^((00|\+)?1)?\d{10}$", "+1" },
    { @"^((00|\+)?20)?1\d{9}$", "+20" },
    { @"^((00|\+)?971)?\d{9}$", "+971" },
    { @"^((00|\+)?966)?\d{9}$", "+966" },
};



    protected override async Task OnParametersSetAsync()
    {
        contractClientForm = (Id == 0) ? new() : await GetByIdAsync<ContractClientDto>($"ContractClients/{Id}");

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
        var dialCode = DialCodeByPattern.TryGetValue(countryCode, out var code) ? code : "";
        contractClientForm.PhoneNumber = NormalizePhone(dialCode, contractClientForm.PhoneNumber!);


        bool result;
        ContractClientDto? clientDtoResult;

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

    private string NormalizePhone(string dialCode, string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        var s = input.Trim();
        s = System.Text.RegularExpressions.Regex.Replace(s, @"\s+", "");
        s = s.Replace("-", "").Replace("(", "").Replace(")", "");

        // 00XXX -> +XXX
        if (s.StartsWith("00"))
            s = "+" + s.Substring(2);

        // already international
        if (s.StartsWith("+"))
            return s;

        // remove leading 0 if user typed local format like 056...
        if (s.StartsWith("0"))
            s = s.Substring(1);

        return $"{dialCode}{s}";
    }


    private string? ValidateVatNumber(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (!value.All(char.IsDigit))
            return languageContainer.Keys["VAT Registration Number must contain digits only"];

        if (value.Length != 15)
            return languageContainer.Keys["VAT Registration Number must be exactly 15 digits"];

        return null;
    }

    private string? ValidateCommercialNumber(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (!value.All(char.IsDigit))
            return languageContainer.Keys["Commercial Registration Number must contain digits only"];

        if (value.Length != 10)
            return languageContainer.Keys["Commercial Registration Number must be exactly 10 digits"];

        return null;
    }
}
