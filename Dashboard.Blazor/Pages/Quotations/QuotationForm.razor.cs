namespace Dashboard.Blazor.Pages.Quotations;

public partial class QuotationForm
{
    [Parameter][EditorRequired] public int Id { get; set; }
    private QuotationDto? quotationForm;
    private bool IsExistClient = true;
    private int ServiceNumber;

    protected override async Task OnParametersSetAsync()
    {
        if (Id == 0)
        {
            quotationForm = new();
            AddService();
        }
        else
        {
            quotationForm = await GetByIdAsync<QuotationDto>($"Quotations/{Id}");

            if (quotationForm is null)
                return;
        }

        //if (quotationForm.Terms == null || quotationForm.Terms.Count == 0)
        //    quotationForm.Terms = new() { new TermDto() };

        await EnsureDefaultCurrencyAsync();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Price Quotations"], href: "/Quotations", icon: EntityIcons.QuotationsIcon),
            new((Id == 0 ? $"{languageContainer.Keys["Add"]} {languageContainer.Keys["Price Quotation"]}" : $"{languageContainer.Keys["Edit"]} {quotationForm.SerialNumber}"), href: null, disabled: true),
        });
    }

    private void TotalValueChanged()
    {
        quotationForm!.TotalPrice = quotationForm!.QuotationBodies.Select(q => q.TotalPrice).Sum();
    }

    private void AddService()
    {
        quotationForm!.QuotationBodies.Add(new QuotationServiceType());
    }

    private void DeleteService(QuotationServiceType service)
    {
        quotationForm!.QuotationBodies.Remove(service);
        quotationForm!.TotalPrice -= service.TotalPrice;
    }

    private void AddTerm() => quotationForm!.Terms?.Add(new());

    private void DeleteTerm(TermDto term) => quotationForm!.Terms?.Remove(term);

    private void OnTermSelected(TermDto term, string? value)
    {
        term.SelectedTerm = value ?? string.Empty;

        if (string.IsNullOrWhiteSpace(term.SelectedTerm))
            return;

        var selected = quotationForm!.UploadedTerms?
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

    private void CopyTermToTextArea(TermDto term)
    {
        if (string.IsNullOrWhiteSpace(term.SelectedTerm))
            return;

        // Find the full term record from the uploaded list
        var selected = quotationForm?.UploadedTerms?
            .FirstOrDefault(x => x.Term == term.SelectedTerm);

        if (selected is null)
        {
            // fallback: at least keep the selected string in Term
            term.Term = term.SelectedTerm;
            return;
        }

        term.Title = selected.Title ?? string.Empty;
        term.Term = selected.Term ?? string.Empty;
        term.TitleAr = selected.TitleAr ?? string.Empty;
        term.TermAr = selected.TermAr ?? string.Empty;
    }


    private async Task<IEnumerable<string>> GetTerms(string value)
    {
        quotationForm ??= new();

        quotationForm.UploadedTerms ??= await GetAllAsync<TermDto>("/Terms") ?? new List<TermDto>();

        var terms = quotationForm.UploadedTerms
            .Where(x => x is not null)
            .ToList();

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

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        quotationForm!.ClientId = quotationForm.Client?.Id;
        quotationForm!.PriceCurrencyId = quotationForm.PriceCurrency!.Id;

        foreach (var body in quotationForm.QuotationBodies)
        {
            body.ServiceId = body.Service!.Id;
            body.UnitId = body.Unit!.Id;
            body.TypeId = body.Type!.Id;
            body.ServiceRequestTypeId = body.ServiceRequestType!.Id;
            body.BranchId = body.Branch!.Id;
        }

        (bool result, QuotationDto? quotationDtoResult) = (Id == 0) ?
            await AddAsync("Quotations", quotationForm!) :
            await UpdateAsync($"Quotations/{Id}", quotationForm!);

        if (result)
        {
            if (Id == 0)
                quotationForm!.Id = quotationDtoResult!.Id;

            NavigationManager.NavigateTo("/Quotations");
        }

        StopProcessing();
    }


    private async Task EnsureDefaultCurrencyAsync()
    {
        quotationForm!.PriceCurrencies ??= await GetAllAsync<CurrencyDto>("Currencies");

        if (quotationForm.PriceCurrency == null && quotationForm.PriceCurrencies?.Any() == true)
        {
            quotationForm.PriceCurrency = quotationForm.PriceCurrencies.FirstOrDefault(c => (bool)c.IsDefault!)
                                       ?? quotationForm.PriceCurrencies.First();
        }
    }

    private async Task<IEnumerable<LookupDto>> GetClients(string value)
    {
        if (quotationForm!.Clients is null)
            quotationForm.Clients = await GetAllLookupsAsync("ContractClients");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return quotationForm.Clients;

        return quotationForm.Clients.Where(x => x.FirstName.Contains(value, StringComparison.InvariantCultureIgnoreCase) || (x.LastName.Contains(value, StringComparison.InvariantCultureIgnoreCase)));
    }

    private async Task<IEnumerable<CurrencyDto>> GetCurrencies(string value)
    {
        if (quotationForm!.PriceCurrencies is null)
            quotationForm.PriceCurrencies = await GetAllAsync<CurrencyDto>("Currencies");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return quotationForm.PriceCurrencies;

        return quotationForm.PriceCurrencies.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}
