namespace Dashboard.Blazor.Pages.Contracts;

public partial class Contracts
{
    private List<ContractDto> contracts = new();

    private readonly string formUri = "Contracts/Form";
    private readonly string detailsUri = "Contracts/Details";

    private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();
    private string role = null!;

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        claims = await GetClaimsPrincipalData();
        role = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value ?? string.Empty;

        breadcrumbItems = new List<BreadcrumbItem>
    {
        new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
        new(languageContainer.Keys["Contracts"], href: null, disabled: true, icon: EntityIcons.ContractsIcon),
    };

        await LoadContracts();

        StopProcessing();
    }

    private async Task LoadContracts()
    {
        contracts = await GetAllAsync<ContractDto>("Contracts?OrderBy=id&Asc=false");
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<ContractDto>($"Contracts/{id}");

        if (isSuccess)
            contracts.Remove(contracts.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private async Task ShowContract(int id)
    {
        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            Position = DialogPosition.Center,
            CloseButton = true
        };

        DialogParameters<ContractReport> Parameters = new()
        {
            { x => x.Id, id }
        };

        await DialogService.ShowAsync<ContractReport>(languageContainer.Keys["Contract Report"], Parameters, dialogOptions);
    }

    private async Task ShowPdfUploadForm(int id)
    {
        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            Position = DialogPosition.TopCenter,
            CloseButton = true
        };

        DialogParameters<FileUpload> Parameters = new()
        {
            { x => x.Id, id }
        };

        await DialogService.ShowAsync<FileUpload>($"{languageContainer.Keys["Upload"]} {languageContainer.Keys["Contract Report"]}", Parameters, dialogOptions);
    }

    private async Task ShowPdfUploadForm(int id, string entityName)
    {
        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            Position = DialogPosition.TopCenter,
            CloseButton = true
        };

        DialogParameters<FileUpload> parameters = new()
    {
        { x => x.Id, id },
        { x => x.EntityName, entityName }
    };

        var dialog = await DialogService.ShowAsync<FileUpload>(
            $"{languageContainer.Keys["Upload"]} {languageContainer.Keys[entityName]}",
            parameters,
            dialogOptions
        );

        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            StartProcessing();
            await LoadContracts();
            StopProcessing();
            StateHasChanged();
        }
    }

    private bool FilterFunc(ContractDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.ContractClient.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.ContractClient.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
