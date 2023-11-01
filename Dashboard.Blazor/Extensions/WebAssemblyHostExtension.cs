using System.Globalization;

using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Dashboard.Blazor.Extensions;

public static class WebAssemblyHostExtension
{
    public async static Task SetDefaultCulture(this WebAssemblyHost host)
    {
        var localStorage = host.Services.GetRequiredService<ILocalStorageService>();

        var result = await localStorage.GetItemAsync<string>("CurrentCulture");

        CultureInfo culture;
        if (result != null)
            culture = new CultureInfo(result);
        else
            culture = new CultureInfo("en-US");

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }

}
