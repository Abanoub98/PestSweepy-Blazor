using System.Security.Claims;

namespace Dashboard.Blazor.Pages.Authuntaction;

public partial class AccountSettings
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    private TwoFactorAuthDto? twoFactorAuthDto;
    private bool isGetInfoSuccess;

    protected override async Task OnInitializedAsync()
    {
        var userId = await GetClaimsPrincipalData(ClaimTypes.NameIdentifier);

        twoFactorAuthDto = await GetByIdAsync($"Account/IsTwoFactorEnabled?userId={userId}");

        if (twoFactorAuthDto.isTwoFactorEnabled)
        {
            (isGetInfoSuccess, TwoFactorAuthDto? obj) = await PostAsync<TwoFactorAuthDto>($"Account/EnableOrDisableTwoFactor?userId={userId}&enable=true", showSuccess: false);

            if (isGetInfoSuccess && obj is not null)
            {
                twoFactorAuthDto.QrCodeImage = obj.QrCodeImage;
                twoFactorAuthDto.SecretKey = obj.SecretKey;
            }
        }
    }

    private async Task ChangeStatusOfTwoFactorAuth()
    {
        StartProcessing();

        var userId = await GetClaimsPrincipalData(ClaimTypes.NameIdentifier);

        twoFactorAuthDto!.isTwoFactorEnabled = !twoFactorAuthDto.isTwoFactorEnabled;

        (isGetInfoSuccess, TwoFactorAuthDto? obj) = await PostAsync<TwoFactorAuthDto>
            ($"Account/EnableOrDisableTwoFactor?userId={userId}&enable={twoFactorAuthDto.isTwoFactorEnabled}", twoFactorAuthDto.isTwoFactorEnabled ? "Enabled Successfully" : "Disabled Successfully");

        if (isGetInfoSuccess && obj is not null && twoFactorAuthDto.isTwoFactorEnabled)
        {
            twoFactorAuthDto.QrCodeImage = obj.QrCodeImage;
            twoFactorAuthDto.SecretKey = obj.SecretKey;
        }

        StopProcessing();
    }
}
