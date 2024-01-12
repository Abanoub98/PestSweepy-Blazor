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
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Categories"], href: "/Categories", icon: EntityIcons.CategoriesIcon),
            new(Id == 0 ? $"{languageContainer.Keys[("Add")]} {languageContainer.Keys[("Category")]}" : $"{languageContainer.Keys[("Edit")]} {categoryForm.Name}", href: null, disabled: true),
        });
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        var result = (Id == 0) ?
            await AddAsync("Categories", categoryForm!) :
            await UpdateAsync($"Categories/{Id}", categoryForm!);

        if (result.isSuccess)
        {
            if (Id == 0)
                categoryForm!.Id = result.obj!.Id;

            if (categoryForm!.UploadedImage is not null)
                await UploadImage("Categories", categoryForm.Id, categoryForm.UploadedImage);

            NavigationManager.NavigateTo("/Categories");
        }

        StopProcessing();
    }

    private void CaptureUploadedImage(IBrowserFile image) => categoryForm!.UploadedImage = image;

    private void ClearUploadedImage() => categoryForm!.UploadedImage = null;
}
