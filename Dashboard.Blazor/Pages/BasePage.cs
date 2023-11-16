using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;

namespace Dashboard.Blazor.Pages;

public class BasePage<T> : ComponentBase where T : class
{
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
    [Inject] protected IDialogService DialogService { get; set; } = default!;
    [Inject] private IApiService ApiService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    protected List<BreadcrumbItem> breadcrumbItems = new();
    protected bool isLoading;
    protected bool isDisable;

    private bool isShowPassword;
    protected InputType PasswordInput = InputType.Password;
    protected string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    protected int tableHight;

    protected readonly string imageUrl = "https://api.pestsweepy.com/Upload/";
    protected readonly long maxFileSize = 1024 * 1024; //represents 1MB 

    protected async Task<List<T>> GetAllAsync(string endPoint)
    {
        var response = await ApiService.GetAllAsync<T>(endPoint);

        if (!response.IsSuccess)
            HandelNavigation(response.StatusCode!);

        return response.ObjectsList!;
    }

    protected async Task<List<TItem>> GetAllLookupsAsync<TItem>(string endPoint) where TItem : class
    {
        var response = await ApiService.GetAllAsync<TItem>(endPoint);

        if (!response.IsSuccess)
        {
            ShowError(response.Error!);
            return new();
        }

        return response.ObjectsList!;
    }

    protected async Task<T> GetByIdAsync(string endPoint)
    {
        var response = await ApiService.GetByIdAsync<T>(endPoint);

        if (!response.IsSuccess)
            HandelNavigation(response.StatusCode!);

        return response.Object!;
    }

    protected async Task<(bool, T?)> AddAsync(string endPoint, T model)
    {
        var response = await ApiService.AddAsync<T>(endPoint, model);

        if (!response.IsSuccess)
        {
            ShowError(response.Error!);
            return (false, response.Object);
        }

        ShowSuccess("Added Successfully");
        return (true, response.Object!);
    }

    protected async Task<(bool, ResponseDto?)> UploadImage(string entityName, int id, IBrowserFile image)
    {
        if (image.Size > maxFileSize)
        {
            ShowError("File size must be less than 1Mb");
            return (false, new());
        }

        using MultipartFormDataContent content = HandelImage(image);

        var response = await ApiService.PostAsync<ResponseDto>($"Files/UploadFile?id={id}&entityName={entityName}&isPdf=false", content);

        if (!response.IsSuccess)
        {
            ShowError(response.Error ?? response.Message ?? string.Empty);
            return (false, response.Object);
        }

        return (true, response.Object!);
    }

    protected void ChangePasswordStatus()
    {
        if (isShowPassword)
        {
            isShowPassword = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else
        {
            isShowPassword = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }

    protected async Task<(bool, TItem?)> PostAsync<TItem>(string endPoint, string successMessage = "Added Successfully", bool showSuccess = true) where TItem : class
    {
        var response = await ApiService.PostAsync<TItem>(endPoint);

        if (!response.IsSuccess)
        {
            ShowError(response.Error!);
            return (false, response.Object);
        }

        if (showSuccess)
            ShowSuccess(successMessage);

        return (true, response.Object!);
    }

    protected async Task<(bool, T?)> UpdateAsync(string endPoint, T model)
    {
        var response = await ApiService.UpdateAsync<T>(endPoint, model);

        if (!response.IsSuccess)
        {
            ShowError(response.Error!);
            return (false, response.Object);
        }

        ShowSuccess("Updated Successfully");
        return (true, response.Object);
    }

    protected async Task<bool> DeleteAsync(string endPoint)
    {
        var isConfirmed = await ShowConfirmation();

        if (!isConfirmed)
            return false;

        var response = await ApiService.DeleteAsync<T>(endPoint);

        if (!response.IsSuccess)
        {
            ShowError(response.Error!);
            return false;
        }

        ShowSuccess("Deleted Successfully");
        return true;
    }

    protected void StartProcessing()
    {
        isLoading = true;
        isDisable = true;
    }

    protected void StopProcessing()
    {
        isLoading = false;
        isDisable = false;
    }

    protected void OpenForm(string uri, int id = 0)
    {
        NavigationManager.NavigateTo(id == 0 ? uri : $"{uri}/{id}");
    }

    protected void OpenDetails(string uri, int id)
    {
        NavigationManager.NavigateTo($"{uri}/{id}");
    }

    protected async Task<bool> ShowConfirmation(string? confirmationMessage = null)
    {
        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            Position = DialogPosition.TopCenter,
            CloseButton = true
        };

        DialogParameters<ConfirmationDialog> formParameters = new();

        if (confirmationMessage is not null)
            formParameters.Add(x => x.ConfirmationMessage, confirmationMessage);

        var dialog = await DialogService.ShowAsync<ConfirmationDialog>("Are you sure?", formParameters, dialogOptions);
        var result = await dialog.Result;

        if (result.Canceled)
            return false;

        return true;
    }

    protected async Task ShowImagePreview(string imageUrl)
    {
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

    protected string HandelDuration(int durationInMin)
    {
        if (durationInMin <= 60)
            return $"{durationInMin} Minutes";

        int hours = durationInMin / 60;
        int remainingMinutes = durationInMin % 60;
        return $"{hours} hours and {remainingMinutes} minutes";
    }

    protected async Task<string?> GetClaimsPrincipalData(string claim)
    {
        var authState = await AuthenticationStateProvider
            .GetAuthenticationStateAsync();

        var user = authState.User;

        if (user.Identity is not null && user.Identity.IsAuthenticated)
            return user.FindFirst(claim)?.Value;

        return null;
    }

    protected void TableHeightChanged(int newTableHight) => tableHight = newTableHight;
    //TODO : Handel All Status Codes
    private MultipartFormDataContent HandelImage(IBrowserFile image)
    {
        var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(image.OpenReadStream(maxFileSize));
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);

        content.Add
        (
        content: fileContent,
        name: "\"files\"",
        fileName: image.Name
        );
        return content;
    }


    private void HandelNavigation(string StatusCode)
    {
        NavigationManager.NavigateTo("/ServerError");
    }

    private void ShowError(string Error)
    {
        Snackbar.Add("SomeThing Went Wrong!", Severity.Error, config =>
        {
            config.Action = "More info";
            config.ActionColor = Color.Surface;
            config.ActionVariant = Variant.Filled;
            config.Onclick = snackbar =>
            {
                ShowErrorDetailsDialog(Error);
                return Task.CompletedTask;
            };
        });
    }

    private void ShowErrorDetailsDialog(string Error)
    {
        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            Position = DialogPosition.Center,
            CloseButton = true
        };

        DialogParameters<ErrorDetailsDialog> formParameters = new()
        {
            { x => x.Error, Error }
        };

        DialogService.Show<ErrorDetailsDialog>("Error Details", formParameters, dialogOptions);
    }

    private void ShowSuccess(string message)
    {
        Snackbar.Add(message, Severity.Success);
    }
}
