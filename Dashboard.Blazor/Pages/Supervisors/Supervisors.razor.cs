namespace Dashboard.Blazor.Pages.Supervisors;

public partial class Supervisors
{
    [Parameter] public int? ManagerId { get; set; }

    private List<SupervisorDto> supervisors = new();
    private string searchString = string.Empty;
    private readonly string formUri = "Supervisors/Form";
    private readonly string detailsUri = "Supervisors/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["Supervisors"], href: null, disabled: true, icon: Icons.Material.TwoTone.SupervisorAccount),
        });

        supervisors = ManagerId is not null ?
            await GetAllAsync($"Supervisors?OrderBy=id&Asc=false&FilterQuery={Uri.EscapeDataString($"ManagerId={ManagerId}")}") :
            await GetAllAsync("Supervisors?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync($"Supervisors/{id}");
        if (isSuccess)
        {
            supervisors.Remove(supervisors.FirstOrDefault(x => x.Id == id)!);
        }

        StopProcessing();
    }

    private bool FilterFunc(SupervisorDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
