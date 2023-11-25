using System.Globalization;
using System.Security.Claims;
using AKSoftware.Localization.MultiLanguages;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Dashboard.Blazor.Components;

public partial class UserMenu
{
    [Inject] ILanguageContainerService LangContainer { get; set; } = default!;
    [Inject] ILocalStorageService LocalStorage { get; set; } = default!;
    [Inject] NavigationManager NavManager { get; set; } = default!;

    [Parameter][EditorRequired] public string CurrentCulture { get; set; } = string.Empty;
    [Parameter][EditorRequired] public EventCallback ThemeChanged { get; set; }

    private IEnumerable<Claim> claims = Enumerable.Empty<Claim>();

    protected override async Task OnInitializedAsync()
    {
        claims = await GetClaimsPrincipalData();
    }

    private async Task ChangeThemeAsync() => await ThemeChanged.InvokeAsync();

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
        await AuthenticationStateProvider.GetAuthenticationStateAsync();
    }
}
