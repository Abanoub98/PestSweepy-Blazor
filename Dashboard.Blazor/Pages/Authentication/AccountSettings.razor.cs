using System.Security.Claims;

namespace Dashboard.Blazor.Pages.Authentication;

public partial class AccountSettings
{
    [Parameter][EditorRequired] public IEnumerable<Claim> Claims { get; set; } = null!;
    [Parameter][EditorRequired] public bool IsTwoFactorEnabled { get; set; }
    [Parameter][EditorRequired] public EventCallback<bool> TwoFactorAuthChanged { get; set; }

    private TwoFactorAuthDto? twoFactorAuthDto;
    private string? userId;

    protected override void OnParametersSet()
    {
        userId = Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (IsTwoFactorEnabled)
            twoFactorAuthDto = new() { isTwoFactorEnabled = true };
        else
            twoFactorAuthDto = new();
    }

    private async Task ChangeStatusOfTwoFactorAuth()
    {
        StartProcessing();
        StateHasChanged();

        twoFactorAuthDto = await GetTwoFactorAuthInfo(userId, !twoFactorAuthDto!.isTwoFactorEnabled);

        if (twoFactorAuthDto is not null)
            await TwoFactorAuthChanged.InvokeAsync(twoFactorAuthDto.isTwoFactorEnabled);

        StopProcessing();
    }

    private async Task ShowTwoFactorAuthMoreInfo()
    {
        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            Position = DialogPosition.Center,
            CloseButton = true
        };

        DialogParameters<TwoFactorAuthInfoDialog> Parameters = new()
        {
            { x => x.userId, userId }
        };

        var dialog = await DialogService.ShowAsync<TwoFactorAuthInfoDialog>("Two-factor authentication Info", Parameters, dialogOptions);
        var result = await dialog.Result;

        if (!result.Canceled)
            await ChangeStatusOfTwoFactorAuth();
    }

    private async Task<TwoFactorAuthDto?> GetTwoFactorAuthInfo(string? userId, bool isTwoFactorEnabled)
    {
        var result = await PostAsync<TwoFactorAuthDto>
            ($"Account/EnableOrDisableTwoFactor?userId={userId}&enable={isTwoFactorEnabled}", showSuccess: false);

        var twoFactorAuthInfo = result.obj;

        if (twoFactorAuthInfo is null)
            return null;

        twoFactorAuthInfo.isTwoFactorEnabled = isTwoFactorEnabled;
        return twoFactorAuthInfo;
    }
}
