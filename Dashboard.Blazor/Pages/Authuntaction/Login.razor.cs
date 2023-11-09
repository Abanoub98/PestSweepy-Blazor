using System.Net;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Dashboard.Blazor.Pages.Authuntaction;

public partial class Login
{
    [Parameter] public string? ReturnUrl { get; set; }

    [Inject] IDialogService DialogService { get; set; } = default!;
    [Inject] ILocalStorageService LocalStorage { get; set; } = default!;
    [Inject] AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
    [Inject] NavigationManager NavigationManager { get; set; } = default!;
    [Inject] HttpClient HttpClient { get; set; } = default!;


    private LoginDto loginDto = new();
    private string? errorMessage;
    private bool isLoading;

    private async Task HandleLogin()
    {
        isLoading = true;
        errorMessage = null;

        var responseMessage = await CallLoginApiAsync(loginDto, "Account/Login");

        if (responseMessage is not null)
        {
            var tokenDto = await responseMessage.Content.ReadFromJsonAsync<TokenDto>();
            await SuccessfulLogin(tokenDto!);
        }

        isLoading = false;
    }

    private async Task<HttpResponseMessage?> CallLoginApiAsync(LoginDto loginDto, string endPoint)
    {
        var responseMessage = await HttpClient.PostAsJsonAsync(endPoint, loginDto);

        if (responseMessage.IsSuccessStatusCode)
            return responseMessage;

        if (responseMessage.StatusCode == HttpStatusCode.Forbidden)
            return await HandelForbiddenResponseAsync();

        await ShowErrorAsync(responseMessage);
        return null;
    }

    private async Task<HttpResponseMessage?> HandelForbiddenResponseAsync()
    {
        var twoFactorAuthCode = await ShowTwoFactorAuthAsync();

        if (twoFactorAuthCode is null)
            return null;

        loginDto.code = twoFactorAuthCode;
        var responseMessage = await HttpClient.PostAsJsonAsync("Account/TwoFactorLogin", loginDto);

        if (responseMessage.IsSuccessStatusCode)
            return responseMessage;

        await ShowErrorAsync(responseMessage);
        return null;
    }

    private async Task<string?> ShowTwoFactorAuthAsync()
    {
        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            Position = DialogPosition.TopCenter,
            CloseButton = true,
        };

        var dialog = await DialogService.ShowAsync<TwoFactorAuthentication>("", dialogOptions);
        var result = await dialog.Result;

        if (result.Canceled)
            return null;

        return result.Data.ToString();
    }

    private async Task SuccessfulLogin(TokenDto tokenDto)
    {
        var token = tokenDto.Token;

        await LocalStorage.SetItemAsync("token", token);
        await AuthStateProvider.GetAuthenticationStateAsync();

        NavigationManager.NavigateTo(ReturnUrl is null ? "/" : ReturnUrl);
    }

    private async Task ShowErrorAsync(HttpResponseMessage responseMessage)
    {
        var response = await responseMessage.Content.ReadFromJsonAsync<ApiResponseDto>();

        if (response is not null)
            errorMessage = response.Message;
    }
}
