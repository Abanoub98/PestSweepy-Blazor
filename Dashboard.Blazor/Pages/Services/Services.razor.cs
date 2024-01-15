namespace Dashboard.Blazor.Pages.Services;

public partial class Services
{
    [Parameter] public int? CategoryId { get; set; } = null;

    private List<ServiceDto> services = new();

    private readonly string formUri = "Services/Form";
    private readonly string detailsUri = "Services/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Services"], href: null, disabled: true, icon: Icons.Material.Outlined.Handyman),
        });

        services = CategoryId is not null ?
            await GetAllAsync<ServiceDto>($"Services?OrderBy=id&Asc=false&FilterQuery={Uri.EscapeDataString($"CategoryId={CategoryId}")}") :
            await GetAllAsync<ServiceDto>("Services?OrderBy=id&Asc=false");


        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<ServiceDto>($"Services/{id}");

        if (isSuccess)
        {
            services.Remove(services.FirstOrDefault(x => x.Id == id)!);

            if (selectedIds.Contains(id))
                selectedIds.Remove(id);
        }

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<ServiceDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<ServiceDto>($"Services/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            services.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }

    private bool FilterFunc(ServiceDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.DurationInMinutes.ToString()!.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.NameEn.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.NameAr.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.OrderIndex.ToString()!.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
