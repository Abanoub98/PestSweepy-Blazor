namespace Dashboard.Blazor.Pages;

public class BasePage<T> : ComponentBase where T : class
{
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
    [Inject] protected IDialogService DialogService { get; set; } = default!;
    [Inject] private IApiService ApiService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    protected List<BreadcrumbItem> breadcrumbItems = new();
    protected bool isLoading;
    protected bool isDisable;

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

    protected async Task<bool> ChangeStatusAsync(string endPoint)
    {
        var response = await ApiService.ChangeStatusAsync<T>(endPoint);

        if (!response.IsSuccess)
        {
            ShowError(response.Error!);
            return false;
        }

        ShowSuccess("Status Changed Successfully");
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

    protected string HandelDuration(double durationInMin)
    {
        if (durationInMin < 60)
            return $"{durationInMin} Minutes";

        double durationInHours = durationInMin / 60;
        durationInHours = Math.Round(durationInHours, 1);

        return $"{durationInHours} Hours";
    }

    //TODO : Handel All Status Codes
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
