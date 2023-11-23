namespace Dashboard.Blazor.Pages.Category;

public partial class CategoryForm
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private CategoryDto? categoryForm;

    protected override async Task OnParametersSetAsync()
    {
        categoryForm = (Id == 0) ? new() : await GetByIdAsync<CategoryDto>($"Categories/{Id}");

        if (categoryForm is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["Categories"], href: "/Categories", icon: Icons.Material.TwoTone.Person3),
            new BreadcrumbItem(languageContainer.Keys[Id == 0 ? "Add Category" : $"Edit {categoryForm.Name}"], href: null, disabled: true),
        });
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        bool result;
        CategoryDto? categoryDtoResult;

        if (Id == 0)
            (result, categoryDtoResult) = await AddAsync("Categories", categoryForm!);
        else
            (result, categoryDtoResult) = await UpdateAsync($"Categories/{Id}", categoryForm!);

        if (result)
        {
            if (Id == 0)
                categoryForm!.Id = categoryDtoResult!.Id;

            if (categoryForm!.UploadedImage is not null)
                await UploadImage("Categories", categoryForm.Id, categoryForm.UploadedImage);

            NavigationManager.NavigateTo("/Categories");
        }

        StopProcessing();
    }

    private void CaptureUploadedImage(IBrowserFile image) => categoryForm!.UploadedImage = image;

    private void ClearUploadedImage() => categoryForm!.UploadedImage = null;
}
