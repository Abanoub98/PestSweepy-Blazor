using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Dashboard.Blazor;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZ";

        var identity = new ClaimsIdentity();

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }
}
