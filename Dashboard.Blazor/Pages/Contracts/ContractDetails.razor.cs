namespace Dashboard.Blazor.Pages.Contracts;

public partial class ContractDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ContractDto? contract;
    private readonly string formUri = "Contracts/Form";

    protected override async Task OnParametersSetAsync()
    {
        contract = await GetByIdAsync<ContractDto>($"Contracts/{Id}");

        if (contract is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Contracts"], href: "/Contracts", icon: EntityIcons.ContractsIcon),
            new(contract.Id.ToString(), href: null, disabled: true),
        });
    }
}
