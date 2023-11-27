namespace Dashboard.Blazor.Pages.Authentication;

public partial class Profile
{
    [Inject] ILocalStorageService LocalStorage { get; set; } = default!;

    private IEnumerable<Claim>? claims;
    private AccountProfile? accountProfile;
    private int profileCompletion = 0;

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        claims = await GetClaimsPrincipalData();
        var userId = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        accountProfile = await GetByIdAsync<AccountProfile>($"Account/Profile/{userId}");

        profileCompletion = CalculateProfileCompletion(accountProfile);

        StopProcessing();
    }

    private int CalculateProfileCompletion(AccountProfile? profile)
    {
        if (profile is null)
            return 0;

        int result = 25;

        if (profile.TwoFactorEnabled)
            result += 25;

        if (profile.EmailConfirmed)
            result += 25;

        if (profile.PhoneNumberConfirmed)
            result += 25;

        return result;
    }

    private async Task SignOut()
    {
        await LocalStorage.RemoveItemAsync("token");
        await AuthenticationStateProvider.GetAuthenticationStateAsync();
        StateHasChanged();
    }
    private void ChangeTwoFactorEnabledValue(bool value)
    {
        accountProfile!.TwoFactorEnabled = value;
        profileCompletion = CalculateProfileCompletion(accountProfile);
    }
}
