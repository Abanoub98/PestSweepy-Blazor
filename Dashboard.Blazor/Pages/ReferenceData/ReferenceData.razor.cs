namespace Dashboard.Blazor.Pages.ReferenceData;

public partial class ReferenceData
{
    //Page Variables 
    private List<string>? TablesNameList;
    private string? SelectedTable;
    private List<LookupDto>? LookupList = new();

    private string searchString = string.Empty;

    protected override void OnInitialized()
    {
        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["Reference Data"], href: null, disabled: true, icon: @Icons.Material.TwoTone.ManageSearch),
        });
    }

    private async Task TableChanged(string selectedTable)
    {
        StartProcessing();

        if (selectedTable == SelectedTable)
            return;

        LookupList = string.IsNullOrWhiteSpace(selectedTable) ?
            new() : await GetAllAsync("ReferenceData?tableName=" + selectedTable);

        SelectedTable = selectedTable;

        StopProcessing();
    }

    private async Task<IEnumerable<string>> GetTablesAsync(string value)
    {
        if (TablesNameList is not null)
            return TablesNameList;

        TablesNameList = await GetAllLookupsAsync<string>("ReferenceData/ShowTables");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return TablesNameList;

        return TablesNameList.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private bool FilterFunc(LookupDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
