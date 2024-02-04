namespace Dashboard.Blazor.Pages.LoginLogs;

public partial class LoginLogs
{
    private List<LoginLogDto> loginLogs = new();

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Login Logs"], href: null, disabled: true, icon: Icons.Material.Outlined.Security),
        };

        loginLogs = await GetAllAsync<LoginLogDto>("SecuirtyLogs/LoginLogs?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private bool FilterFunc(LoginLogDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.UserName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Role.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.UserId.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.LoginDateLocal.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Ip.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}
