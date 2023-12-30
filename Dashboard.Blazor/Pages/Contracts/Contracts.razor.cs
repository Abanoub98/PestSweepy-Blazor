namespace Dashboard.Blazor.Pages.Contracts;

public partial class Contracts
{
    private List<ContractDto> contracts = new();

    private readonly string formUri = "Contracts/Form";
    private readonly string detailsUri = "Contracts/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Contracts"], href: null, disabled: true, icon: EntityIcons.ContractsIcon),
        });

        contracts = await GetAllAsync<ContractDto>("Contracts?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<ContractDto>($"Contracts/{id}");

        if (isSuccess)
            contracts.Remove(contracts.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private bool FilterFunc(ContractDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.CreatedAt.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.FirstParty.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.SecondParty.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
