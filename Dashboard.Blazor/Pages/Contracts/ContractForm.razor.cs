namespace Dashboard.Blazor.Pages.Contracts;

public partial class ContractForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ContractDto? contractForm;

    protected override async Task OnParametersSetAsync()
    {
        if (Id == 0)
        {
            contractForm = new();
        }
        else
        {
            contractForm = await GetByIdAsync<ContractDto>($"Contracts/{Id}");

            if (contractForm is null)
                return;
        }


        if (contractForm is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Contracts"], href: "/Contracts", icon: EntityIcons.ContractsIcon),
            new(languageContainer.Keys[Id == 0 ? "Add Contract" : $"Edit {contractForm.ContractClient!.FirstName} {contractForm.ContractClient!.LastName} Contract"], href: null, disabled: true),
        });
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        contractForm!.ContractClientId = contractForm.ContractClient!.Id;
        contractForm!.ContractDurationId = contractForm.ContractDuration!.Id;
        contractForm!.QuotationID = contractForm.Quotation!.Id;
        contractForm!.PaymentMethodId = contractForm.PaymentMethod!.Id;

        (bool isSuccess, ContractDto? contractDto) result;

        if (Id == 0)
            result = await AddAsync("Contracts", contractForm!);
        else
            result = await UpdateAsync($"Contracts/{Id}", contractForm!);

        if (result.isSuccess)
            NavigationManager.NavigateTo("/Contracts");

        StopProcessing();
    }

    private void AddTerm() => contractForm!.Terms?.Add(new());

    private void DeleteTerm(TermDto term) => contractForm!.Terms?.Remove(term);

    private void OnTermSelected(TermDto term, string? value)
    {
        term.SelectedTerm = value ?? string.Empty;

        if (string.IsNullOrWhiteSpace(term.SelectedTerm))
            return;

        var selected = contractForm!.UploadedTerms?
            .FirstOrDefault(x => x.Term == term.SelectedTerm);

        if (selected is null)
        {
            term.Term = term.SelectedTerm;
            return;
        }

        term.Title = selected.Title ?? string.Empty;
        term.Term = selected.Term ?? string.Empty;
        term.TitleAr = selected.TitleAr ?? string.Empty;
        term.TermAr = selected.TermAr ?? string.Empty;
    }

    private void CopyTermToTextArea(TermDto term) => term.Term = term.SelectedTerm;


    private async Task<IEnumerable<LookupDto>> GetDurations(string value)
    {
        if (contractForm!.ContractDurations is null)
            contractForm.ContractDurations = await GetAllLookupsAsync("ReferenceData?tableName=Durations");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return contractForm.ContractDurations;

        return contractForm.ContractDurations.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task<IEnumerable<LookupDto>> GetPaymentMethods(string value)
    {
        if (contractForm!.PaymentMethods is null)
            contractForm.PaymentMethods = await GetAllLookupsAsync("ReferenceData?tableName=PaymentMethods");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return contractForm.PaymentMethods;

        return contractForm.PaymentMethods
            .Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task<IEnumerable<ContractClientDto>> GetContractClients(string value)
    {
        if (contractForm!.ContractClients is null)
            contractForm.ContractClients = await GetAllAsync<ContractClientDto>("/ContractClients");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return contractForm.ContractClients;

        return contractForm.ContractClients.Where(x => x.FirstName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task<IEnumerable<string>> GetTerms(string value)
    {
        contractForm ??= new();

        contractForm.UploadedTerms ??= await GetAllAsync<TermDto>("/Terms") ?? new List<TermDto>();

        var terms = contractForm.UploadedTerms
            .Where(x => x is not null);

        if (string.IsNullOrWhiteSpace(value))
        {
            return terms
                .Select(t => t.Term)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList()!;
        }

        return terms
            .Where(x =>
                (!string.IsNullOrWhiteSpace(x.Term) && x.Term.Contains(value, StringComparison.InvariantCultureIgnoreCase)) ||
                (!string.IsNullOrWhiteSpace(x.Title) && x.Title.Contains(value, StringComparison.InvariantCultureIgnoreCase)) ||
                (!string.IsNullOrWhiteSpace(x.TermAr) && x.TermAr.Contains(value, StringComparison.InvariantCultureIgnoreCase)) ||
                (!string.IsNullOrWhiteSpace(x.TitleAr) && x.TitleAr.Contains(value, StringComparison.InvariantCultureIgnoreCase))
            )
            .Select(t => t.Term)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToList()!;
    }

    private async Task<IEnumerable<QuotationDto>> GetQuotations(string value)
    {
        if (contractForm?.ContractClient?.Id is null)
            return Enumerable.Empty<QuotationDto>();

        if (contractForm!.Quotations is null)
            contractForm.Quotations = await GetAllAsync<QuotationDto>(
                $"/Quotations?FilterQuery=clientId%3D{contractForm.ContractClient.Id}"
            );

        if (string.IsNullOrWhiteSpace(value))
            return contractForm.Quotations;

        return contractForm.Quotations.Where(x =>
            (x.Client?.FirstName?.Contains(value, StringComparison.InvariantCultureIgnoreCase) ?? false)
            || (x.ClientName?.Contains(value, StringComparison.InvariantCultureIgnoreCase) ?? false)
        );
    }

    private async Task OnContractClientChanged(ContractClientDto client)
    {
        contractForm!.ContractClient = client;

        contractForm.Quotations = null!;
        contractForm.Quotation = null!;
        contractForm.QuotationID = 0;

        if (client is not null)
            contractForm.Quotations = await GetAllAsync<QuotationDto>(
                $"/Quotations?FilterQuery=clientId%3D{client.Id}"
            );

        StateHasChanged();
    }
}
