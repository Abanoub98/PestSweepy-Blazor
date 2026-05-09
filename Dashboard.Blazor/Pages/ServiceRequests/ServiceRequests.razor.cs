namespace Dashboard.Blazor.Pages.ServiceRequests;

public partial class ServiceRequests
{
    private List<ServiceRequestsDto> serviceRequests = new();

    private readonly string formUri = "ServiceRequests/Form";
    private readonly string detailsUri = "ServiceRequests/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["ServiceRequests"], href: null, disabled: true, icon: EntityIcons.CategoriesIcon),
        };

        serviceRequests = await GetAllAsync<ServiceRequestsDto>("ServiceRequests?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<ServiceRequestsDto>($"ServiceRequests/{id}");

        if (isSuccess)
            serviceRequests.Remove(serviceRequests.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<ServiceRequestsDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<ServiceRequestsDto>($"ServiceRequests/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            serviceRequests.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }
    private bool FilterFunc(ServiceRequestsDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.EntityName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.EntityPhone.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.EntityEmail.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
