using System.Security.Claims;

namespace Dashboard.Blazor.Pages.Authuntaction;

public partial class AccountSettings
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    private TwoFactorAuthDto? twoFactorAuthDto;
    private bool isGetInfoSuccess;
    private string? userId;

    protected override async Task OnInitializedAsync()
    {
        userId = await GetClaimsPrincipalData(ClaimTypes.NameIdentifier);

        twoFactorAuthDto = await GetByIdAsync($"Account/IsTwoFactorEnabled?userId={userId}");

        if (twoFactorAuthDto.isTwoFactorEnabled)
            await GetTwoFactorAuthInfo(userId);
    }

    private async Task ChangeStatusOfTwoFactorAuth()
    {
        StartProcessing();

        twoFactorAuthDto!.isTwoFactorEnabled = !twoFactorAuthDto.isTwoFactorEnabled;

        await GetTwoFactorAuthInfo(userId);

        StopProcessing();
    }

    private async Task GetTwoFactorAuthInfo(string? userId)
    {
        (isGetInfoSuccess, TwoFactorAuthDto? obj) = await PostAsync<TwoFactorAuthDto>
            ($"Account/EnableOrDisableTwoFactor?userId={userId}&enable={twoFactorAuthDto.isTwoFactorEnabled}", showSuccess: false);

        if (isGetInfoSuccess && obj is not null && twoFactorAuthDto.isTwoFactorEnabled)
        {
            twoFactorAuthDto.QrCodeImage = obj.QrCodeImage;
            twoFactorAuthDto.SecretKey = obj.SecretKey;
        }
    }
}
