namespace Dashboard.Blazor.Pages.Services;

public partial class ServiceForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ServiceDto? serviceForm;

    protected override async Task OnParametersSetAsync()
    {
        if (Id == 0)
            serviceForm = new();
        else
            serviceForm = await GetByIdAsync($"Services/{Id}");


        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["Services"], href: "/Services", icon: Icons.Material.TwoTone.Handyman),
            new BreadcrumbItem(languageContainer.Keys[Id == 0 ? "Add Service" : $"Edit {serviceForm.Name}"], href: null, disabled: true),
        });
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        serviceForm!.CategoryId = serviceForm.Category!.Id;

        bool result;
        ServiceDto? serviceDtoResult;

        if (Id == 0)
            (result, serviceDtoResult) = await AddAsync("Services", serviceForm!);
        else
            (result, serviceDtoResult) = await UpdateAsync($"Services/{Id}", serviceForm!);

        if (result)
        {
            if (Id == 0)
                serviceForm!.Id = serviceDtoResult!.Id;

            if (serviceForm.UploadedImage is not null)
                await UploadImage("Services", serviceForm.Id, serviceForm.UploadedImage);

            NavigationManager.NavigateTo("/Services");
        }

        StopProcessing();
    }

    private void CaptureUploadedImage(IBrowserFile image) => serviceForm!.UploadedImage = image;

    private void ClearUploadedImage() => serviceForm!.UploadedImage = null;

    private async Task<IEnumerable<LookupDto>> GetCategories(string value)
    {
        if (serviceForm!.Categories is null)
            serviceForm.Categories = await GetAllLookupsAsync<LookupDto>("Categories");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return serviceForm.Categories;

        return serviceForm.Categories.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}
