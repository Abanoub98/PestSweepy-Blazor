namespace Dashboard.Blazor.Pages.Services;

public partial class Services
{
    [Parameter] public int? CategoryId { get; set; } = null;

    private List<ServiceDto> services = new();
    private string searchString = string.Empty;
    private readonly string formUri = "Services/Form";
    private readonly string detailsUri = "Services/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["Services"], href: null, disabled: true, icon: Icons.Material.TwoTone.Handyman),
        });

        services = CategoryId is not null ?
            await GetAllAsync($"Services?OrderBy=id&Asc=false&FilterQuery={Uri.EscapeDataString($"CategoryId={CategoryId}")}") :
            await GetAllAsync("Services?OrderBy=id&Asc=false");


        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync($"Services/{id}");

        if (isSuccess)
        {
            services.Remove(services.FirstOrDefault(x => x.Id == id)!);
        }

        StopProcessing();
    }

    private bool FilterFunc(ServiceDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.DurationInMinutes.ToString()!.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.OrderIndex.ToString()!.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
