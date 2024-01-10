namespace Dashboard.Blazor.Pages.Contracts;

public partial class ContractForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ContractDto? contractForm;

    protected override async Task OnParametersSetAsync()
    {
        contractForm = (Id == 0) ? new() { Terms = new() { new TermDto() } } : await GetByIdAsync<ContractDto>($"Contracts/{Id}");

        if (contractForm is null)
            return;

        foreach (var term in contractForm.Terms.Where(t => t.Quotation is not null))
        {
            term.ShowQuotationsList = true;
        }

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Contracts"], href: "/Contracts", icon: EntityIcons.ContractsIcon),
            new(languageContainer.Keys[Id == 0 ? "Add Contract" : $"Edit {contractForm.ContractClient!.Name} Contract"], href: null, disabled: true),
        });
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        contractForm!.ContractClientId = contractForm.ContractClient!.Id;
        contractForm!.ContractDurationId = contractForm.ContractDuration!.Id;

        foreach (var term in contractForm.Terms.Where(t => t.ShowQuotationsList))
        {
            term.QuotationId = term.Quotation?.Id;
        }


        (bool isSuccess, ContractDto? contractDto) result;

        if (Id == 0)
            result = await AddAsync("Contracts", contractForm!);
        else
            result = await UpdateAsync($"Contracts/{Id}", contractForm!);

        if (result.isSuccess)
            NavigationManager.NavigateTo("/Contracts");

        StopProcessing();
    }

    private void AddTerm() => contractForm!.Terms.Add(new());

    private void DeleteTerm(TermDto term) => contractForm!.Terms.Remove(term);

    private void CopyTermToTextArea(TermDto term) => term.Term = term.SelectedTerm;

    private void DeleteQuotation(bool args, TermDto term)
    {
        term.ShowQuotationsList = args;

        if (!term.ShowQuotationsList)
            term.Quotation = null;
    }

    private async Task<IEnumerable<LookupDto>> GetDurations(string value)
    {
        if (contractForm!.ContractDurations is null)
            contractForm.ContractDurations = await GetAllLookupsAsync("ReferenceData?tableName=Durations");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return contractForm.ContractDurations;

        return contractForm.ContractDurations.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task<IEnumerable<LookupDto>> GetContractClients(string value)
    {
        if (contractForm!.ContractClients is null)
            contractForm.ContractClients = await GetAllLookupsAsync("/ContractClients");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return contractForm.ContractClients;

        return contractForm.ContractClients.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task<IEnumerable<string>> GetTerms(string value)
    {
        if (contractForm!.UploadedTerms is null)
            contractForm.UploadedTerms = await GetAllAsync<TermDto>("/Terms");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return contractForm.UploadedTerms.Select(t => t.Term).Distinct().ToList();

        return contractForm.UploadedTerms.Where(x => x.Term!.Contains(value, StringComparison.InvariantCultureIgnoreCase)).Select(t => t.Term).Distinct().ToList();
    }

    private async Task<IEnumerable<QuotationDto>> GetQuotations(string value)
    {
        if (contractForm!.Quotations is null)
            contractForm.Quotations = await GetAllAsync<QuotationDto>("/Quotations");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return contractForm.Quotations;

        return contractForm.Quotations.Where(x => x.Client?.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase) ?? x.ClientName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}
