namespace Dashboard.Blazor.Pages.ContractClients;

public partial class ContractClientForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ContractClientDto? contractClientForm;
    private int TermNumber;

    protected override async Task OnParametersSetAsync()
    {
        contractClientForm = (Id == 0) ? new() { Terms = new() { new Term() } } : contractClientForm = new ContractClientDto
        {
            Id = 1,
            Date = DateTime.Now,
            Number = 56447,
            FirstParty = "John Sam",
            SecondParty = "Doe Corporation",
            SecondPartyExecutiveOfficer = "Jane Doe",
            FirstPartyExecutiveOfficer = "Alice Johnson",
            Terms = new() { new Term() { TermContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit." } },
            Quotation = new LookupDto { Id = 101, Name = "Standard Quotation" },
        };

        if (contractClientForm is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Contract Clients"], href: "/ContractClients", icon: EntityIcons.ContractsIcon),
            new(languageContainer.Keys[Id == 0 ? "Add Contract Clients" : $"Edit {contractClientForm.Number}"], href: null, disabled: true),
        });
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        contractClientForm!.QuotationId = contractClientForm.Quotation!.Id;

        bool result;
        ContractClientDto? contractClientDtoResult;

        if (Id == 0)
            (result, contractClientDtoResult) = await AddAsync("ContractClients", contractClientForm!);
        else
            (result, contractClientDtoResult) = await UpdateAsync($"ContractClients/{Id}", contractClientForm!);

        if (result)
            NavigationManager.NavigateTo("/ContractClients");

        StopProcessing();
    }

    private void AddTerm()
    {
        contractClientForm!.Terms.Add(new());
    }

    private void DeleteTerm(Term term)
    {
        contractClientForm!.Terms.Remove(term);
    }

    private async Task<IEnumerable<LookupDto>> GetQuotations(string value)
    {
        if (contractClientForm!.Quotations is null)
            contractClientForm.Quotations = await GetAllLookupsAsync("/Quotations");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return contractClientForm.Quotations;

        return contractClientForm.Quotations.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}
