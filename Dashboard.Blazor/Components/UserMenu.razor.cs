using System.Globalization;
using System.Security.Claims;
using AKSoftware.Localization.MultiLanguages;
using Blazored.LocalStorage;
using Dashboard.Blazor.Pages.Authuntaction;
using Microsoft.AspNetCore.Components.Authorization;

namespace Dashboard.Blazor.Components;

public partial class UserMenu
{
    [Inject] ILanguageContainerService LangContainer { get; set; } = default!;
    [Inject] ILocalStorageService LocalStorage { get; set; } = default!;
    [Inject] AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
    [Inject] NavigationManager NavManager { get; set; } = default!;
    [Inject] IDialogService DialogService { get; set; } = default!;

    [Parameter][EditorRequired] public string CurrentCulture { get; set; } = string.Empty;
    [Parameter][EditorRequired] public EventCallback ThemeChnaged { get; set; }

    private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();

    protected override async Task OnInitializedAsync()
    {
        claims = await GetClaimsPrincipalData();
    }

    private async Task ChangeThemeAsync()
    {
        await ThemeChnaged.InvokeAsync();
    }

    private void SetLanguage(string cultureCode)
    {
        LangContainer.SetLanguage(CultureInfo.GetCultureInfo(cultureCode));
        if (CultureInfo.CurrentCulture != null)
        {
            // Set the culture in LocalStorage
            LocalStorage.SetItemAsync("CurrentCulture", cultureCode);
            // Load the Current URL
            NavManager.NavigateTo(NavManager.Uri, forceLoad: true);
        }
    }

    private async Task SignOut()
    {
        await LocalStorage.RemoveItemAsync("token");
        await AuthStateProvider.GetAuthenticationStateAsync();
    }

    private async Task ShowAccountSettingsAsync()
    {
        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            Position = DialogPosition.TopCenter,
            CloseButton = true,
        };

        await DialogService.ShowAsync<AccountSettings>(languageContainer.Keys["Account Settings"], dialogOptions);
    }

    private async Task<IEnumerable<Claim>> GetClaimsPrincipalData()
    {
        var authState = await AuthStateProvider
            .GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            foreach (var item in user.Claims)
            {
                Console.WriteLine(item);
            }
            return user.Claims;
        }

        return Enumerable.Empty<Claim>();
    }


}
