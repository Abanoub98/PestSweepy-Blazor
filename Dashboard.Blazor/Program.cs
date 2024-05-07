using System.Reflection;
using Blazorise;
using Dashboard.Blazor;
using Dashboard.Blazor.Extensions;
using Majorsoft.Blazor.Components.Common.JsInterop;
using Majorsoft.Blazor.Components.Maps;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://api.pestsweepy.com/") });
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddMapExtensions();
builder.Services.AddJsInteropExtensions();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly());
builder.Services.AddAuthorizationCore();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddBlazorise(options =>
{
    options.Immediate = true;
}).AddEmptyProviders();

var host = builder.Build();
await host.SetDefaultCulture();
await host.RunAsync();