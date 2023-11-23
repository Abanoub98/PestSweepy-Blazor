using Dashboard.Blazor.Models.Consts;

namespace Dashboard.Blazor.Pages.Quotations;

public partial class QuotationForm
{
    [Parameter][EditorRequired] public int Id { get; set; }
    private QuotationDto? quotationForm;
    private bool IsExistClient;
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
            //quotationForm = await GetByIdAsync<QuotationDto>($"Quotations/{Id}");

            if (quotationForm is null)
                return;
        }

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Quotations"], href: "/Quotations", icon: EntityIcons.QuotationsIcon),
            new(languageContainer.Keys[Id == 0 ? "Add Quotation" : $"Edit {quotationForm.SerialNumber}"], href: null, disabled: true),
        });
    }

    private void TotalValueChanged()
    {
        quotationForm!.TotalPrice = quotationForm!.QuotationServices.Select(q => q.TotalPrice).Sum();
    }

    private void AddService()
    {
        quotationForm!.QuotationServices.Add(new QuotationServiceType());
    }

    private void DeleteService(QuotationServiceType service)
    {
        quotationForm!.QuotationServices.Remove(service);
        quotationForm!.TotalPrice -= service.TotalPrice;
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        quotationForm!.ClientId = quotationForm.Client?.Id;

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

    private async Task<IEnumerable<LookupDto>> GetClients(string value)
    {
        if (quotationForm!.Clients is null)
            quotationForm.Clients = await GetAllLookupsAsync("Clients");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return quotationForm.Clients;

        return quotationForm.Clients.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}
