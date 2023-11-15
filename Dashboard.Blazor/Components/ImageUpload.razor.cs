namespace Dashboard.Blazor.Components;

public partial class ImageUpload
{
    [Parameter][EditorRequired] public EventCallback UploadedImageCleared { get; set; }
    [Parameter][EditorRequired] public EventCallback<IBrowserFile> ImageSelected { get; set; }
    [Parameter][EditorRequired] public string? CurrentImage { get; set; }

    [Inject] protected IDialogService DialogService { get; set; } = default!;


    private string? uploadedImage;
    private IBrowserFile? uploadedImageFile;

    public async Task LoadImage(InputFileChangeEventArgs inputFileChangeEventArgs)
    {
        var image = await inputFileChangeEventArgs.File.RequestImageFileAsync("image/png", 600, 600);

        using Stream imageStream = image.OpenReadStream(1024 * 1024 * 10);
        using MemoryStream ms = new();

        //copy imageStream to Memory stream
        await imageStream.CopyToAsync(ms);

        //convert stream to base64
        uploadedImage = $"data:image/png;base64,{Convert.ToBase64String(ms.ToArray())}";
        await ImageSelected.InvokeAsync(image);

        StateHasChanged();
    }

    protected async Task ShowImagePreview(string? imageUrl)
    {
        if (imageUrl is null)
            return;

        Console.WriteLine(imageUrl);

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

    private async void Clear()
    {
        uploadedImage = null;
        uploadedImageFile = null;
        await UploadedImageCleared.InvokeAsync();
    }
}
