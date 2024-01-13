namespace Dashboard.Blazor.Pages.LoginLogs;

public partial class LoginLogs
{
    private IEnumerable<LoginLogDto> pagedData;
    private MudGrid table;

    private int totalItems;

    //private async Task<GridData<LoginLogDto>> ServerReload(GridState<LoginLogDto> state)
    //{
    //    IEnumerable<LoginLogDto> data = await GetAllAsync<LoginLogDto>("SecuirtyLogs/LoginLogs?OrderBy=id&Asc=false");

    //    totalItems = data.Count();

    //    return new GridData<LoginLogDto>() { TotalItems = totalItems, Items = pagedData };
    //}

    //private void OnSearch(string text)
    //{
    //    searchString = text;
    //    table.ReloadServerData();
    //}

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Login Logs"], href: null, disabled: true, icon: Icons.Material.TwoTone.Security),
        });

        StopProcessing();
    }
}
