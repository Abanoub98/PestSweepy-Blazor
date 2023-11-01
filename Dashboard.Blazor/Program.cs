using System.Reflection;
using AKSoftware.Localization.MultiLanguages;
using Blazored.LocalStorage;
using Dashboard.Blazor;
using Dashboard.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly());
builder.Services.AddAuthorizationCore();

var host = builder.Build();
await host.SetDefaultCulture();
await host.RunAsync();