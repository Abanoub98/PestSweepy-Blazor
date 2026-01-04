namespace Dashboard.Blazor.Pages.Certificates;

public partial class Certificates
{
    private List<CertificatesDto> certificates = new();

    private readonly string formUri = "Certificates/Form";
    private readonly string detailsUri = "Certificates/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Certificates"], href: null, disabled: true, icon: EntityIcons.CategoriesIcon),
        };

        certificates = await GetAllAsync<CertificatesDto>("Certificates?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<CertificatesDto>($"Certificates/{id}");

        if (isSuccess)
            certificates.Remove(certificates.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<CertificatesDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<CertificatesDto>($"Certificates/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            certificates.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }
    private bool FilterFunc(CertificatesDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.NameAr.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.NameEn.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.OrderIndex.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
