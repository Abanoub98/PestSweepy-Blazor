namespace Dashboard.Blazor.Components;

public partial class ImagesUpload
{
    [Parameter][EditorRequired] public EventCallback UploadedImagesCleared { get; set; }
    [Parameter][EditorRequired] public EventCallback<List<IBrowserFile>> ImagesSelected { get; set; }
    [Parameter] public List<string>? CurrentImages { get; set; }

    [Inject] protected IDialogService DialogService { get; set; } = default!;

    private readonly List<UploadedImageItem> uploadedImages = new();

    public async Task LoadImages(IReadOnlyList<IBrowserFile>? files)
    {
        if (files is null || !files.Any())
            return;

        uploadedImages.Clear();

        foreach (var file in files)
        {
            var image = await file.RequestImageFileAsync("image/png", 600, 600);

            using Stream imageStream = image.OpenReadStream(1024 * 1024 * 10);
            using MemoryStream ms = new();

            await imageStream.CopyToAsync(ms);

            var previewUrl = $"data:image/png;base64,{Convert.ToBase64String(ms.ToArray())}";

            uploadedImages.Add(new UploadedImageItem
            {
                File = image,
                PreviewUrl = previewUrl
            });
        }

        await ImagesSelected.InvokeAsync(uploadedImages.Select(x => x.File).ToList());

        StateHasChanged();
    }

    protected async Task ShowImagePreview(string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            return;

        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            Position = DialogPosition.Center,
            CloseButton = true
        };

        DialogParameters<ImagePreview> formParameters = new()
        {
            { x => x.ImageUrl, imageUrl }
        };

        await DialogService.ShowAsync<ImagePreview>("Image Preview", formParameters, dialogOptions);
    }

    private async Task RemoveImage(IBrowserFile file)
    {
        var item = uploadedImages.FirstOrDefault(x => x.File == file);

        if (item is null)
            return;

        uploadedImages.Remove(item);

        await ImagesSelected.InvokeAsync(uploadedImages.Select(x => x.File).ToList());

        StateHasChanged();
    }

    private async Task ClearAll()
    {
        uploadedImages.Clear();
        await UploadedImagesCleared.InvokeAsync();
        await ImagesSelected.InvokeAsync(new List<IBrowserFile>());

        StateHasChanged();
    }

    private class UploadedImageItem
    {
        public IBrowserFile File { get; set; } = default!;
        public string PreviewUrl { get; set; } = string.Empty;
    }
}