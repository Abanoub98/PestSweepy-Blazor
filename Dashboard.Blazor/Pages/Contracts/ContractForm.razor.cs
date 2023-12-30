namespace Dashboard.Blazor.Pages.Contracts;

public partial class ContractForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ContractDto? contractForm;
    private int TermNumber;

    protected override async Task OnParametersSetAsync()
    {
        contractForm = (Id == 0) ? new() { Terms = new() { new TermDto() } } : contractForm = await GetByIdAsync<ContractDto>($"Contracts/{Id}");

        if (contractForm is null)
            return;

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

        contractForm!.QuotationId = contractForm.Quotation!.Id;
        contractForm!.ContractClientId = contractForm.ContractClient!.Id;


        (bool isSuccess, ContractDto? contractDto) result;

        if (Id == 0)
            result = await AddAsync("Contracts", contractForm!);
        else
            result = await UpdateAsync($"Contracts/{Id}", contractForm!);

        if (result.isSuccess)
            NavigationManager.NavigateTo("/Contracts");

        StopProcessing();
    }

    private void AddTerm()
    {
        contractForm!.Terms.Add(new());
    }

    private void DeleteTerm(TermDto term)
    {
        contractForm!.Terms.Remove(term);
    }

    private void CopyTermToTextArea(TermDto term)
    {
        term.Term = term.SelectedTerm;
    }

    private async Task<IEnumerable<LookupDto>> GetQuotations(string value)
    {
        if (contractForm!.Quotations is null)
            contractForm.Quotations = new List<LookupDto>() { new LookupDto { Id = 0, Name = "Test" } };

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return contractForm.Quotations;

        return contractForm.Quotations.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
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
}
