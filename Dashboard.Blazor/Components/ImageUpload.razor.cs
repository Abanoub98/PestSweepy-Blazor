namespace Dashboard.Blazor.Components;

public partial class ImageUpload
{
    [Parameter][EditorRequired] public EventCallback UploadedImageCleared { get; set; }
    [Parameter][EditorRequired] public EventCallback<IBrowserFile> ImageSelected { get; set; }
    [Parameter][EditorRequired] public string? CurrentImage { get; set; }

    protected string? uploadedImage;

    public async Task LoadImage(InputFileChangeEventArgs inputFileChangeEventArgs)
    {
        var image = await inputFileChangeEventArgs.File.RequestImageFileAsync("image/png", 600, 600);

        using Stream imageStream = image.OpenReadStream(1024 * 1024 * 10);

        using MemoryStream ms = new();
        //copy imageStream to Memory stream
        await imageStream.CopyToAsync(ms);

        //convert stream to base64
        uploadedImage = $"data:image/png;base64,{Convert.ToBase64String(ms.ToArray())}";

        StateHasChanged();
    }

    private async void Clear()
    {
        uploadedImage = null;
        await UploadedImageCleared.InvokeAsync();
    }

    private async Task ImageChanged(IBrowserFile image) =>
        await ImageSelected.InvokeAsync(image);

}
