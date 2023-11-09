using System.Reflection;
using AKSoftware.Localization.MultiLanguages;
using Blazored.LocalStorage;
using Dashboard.Blazor;
using Dashboard.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://api.pestsweepy.com/") });
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IApiService, ApiService>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly());
builder.Services.AddAuthorizationCore();
builder.Services.AddMudServices();

var host = builder.Build();
await host.SetDefaultCulture();
await host.RunAsync();