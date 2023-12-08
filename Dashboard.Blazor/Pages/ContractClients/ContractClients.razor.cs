namespace Dashboard.Blazor.Pages.ContractClients;

public partial class ContractClients
{
    private List<ContractClientDto> contractClients = new();
    private string searchString = string.Empty;

    private readonly string formUri = "ContractClients/Form";
    private readonly string detailsUri = "ContractClients/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Price ContractClients"], href: null, disabled: true, icon: EntityIcons.ContractsIcon),
        });

        //contractClients = await GetAllAsync<ContractClientDto>("ContractClients?OrderBy=id&Asc=false");

        contractClients = new()
        {
            new ContractClientDto
            {
                Id = 1,
                Date = DateTime.Now,
                Number = 56447,
                FirstParty = "John Sam",
                SecondParty = "Doe Corporation",
                SecondPartyExecutiveOfficer = "Jane Doe",
                FirstPartyExecutiveOfficer = "Alice Johnson",
                Terms = new() { new Term() { TermContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit." } },
                Quotation = new LookupDto { Id = 101, Name = "Standard Quotation" }
            }
        };

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<ContractClientDto>($"ContractClients/{id}");

        if (isSuccess)
            contractClients.Remove(contractClients.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private bool FilterFunc(ContractClientDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Date.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Number.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.FirstParty.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.SecondParty.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
