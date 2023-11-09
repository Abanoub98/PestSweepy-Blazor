namespace Dashboard.Blazor.Pages.LoginLogs;

public partial class LoginLogs
{
    private List<LoginLogDto> loginLogs = new();
    private string searchString = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["LoginLogs"], href: null, disabled: true),
        });

        loginLogs = await GetAllAsync("SecuirtyLogs/LoginLogs");

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
        if (element.LoginDate.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Ip.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}
