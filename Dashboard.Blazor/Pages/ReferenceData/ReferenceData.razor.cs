using System.Text.RegularExpressions;

namespace Dashboard.Blazor.Pages.ReferenceData;

public partial class ReferenceData
{
    //Page Variables 
    private List<string>? TablesNameList;
    private string? SelectedTable;
    private List<LookupDto>? LookupList = new();

    private bool hideTableActions;

    protected override void OnInitialized()
    {
        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Reference Data"], href: null, disabled: true, icon: @Icons.Material.Outlined.ManageSearch),
        });
    }

    private async Task TableChanged(string selectedTable)
    {
        StartProcessing();

        if (selectedTable == SelectedTable)
            return;

        LookupList = string.IsNullOrWhiteSpace(selectedTable) ?
            new() : await GetAllLookupsAsync("ReferenceData?tableName=" + selectedTable);

        switch (selectedTable)
        {
            case "Countries":
                hideTableActions = true;
                break;
            case "OrderStates":
                hideTableActions = true;
                break;
            case "PaymentMethods":
                hideTableActions = true;
                break;
            default:
                hideTableActions = false;
                break;
        }

        SelectedTable = selectedTable;

        StopProcessing();
    }

    private string ConvertPascalCaseToNormalText(string input)
    {
        // Use regular expression to insert a space before each capital letter
        string result = Regex.Replace(input, "([a-z])([A-Z])", "$1 $2");

        // Capitalize the first letter
        result = char.ToUpper(result[0]) + result.Substring(1);

        return result;
    }

    private async Task ShowForm(int id = 0)
    {
        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            Position = DialogPosition.TopCenter,
            CloseButton = true
        };

        DialogParameters<ReferenceDataForm> Parameters = new()
        {
            { x => x.Id, id },
            {x => x.TableName, SelectedTable}
        };

        var dialog = await DialogService.ShowAsync<ReferenceDataForm>(id == 0 ? $"Add {SelectedTable}" : $"Edit {SelectedTable}", Parameters, dialogOptions);
        var result = await dialog.Result;

        if (!result.Canceled)
            LookupList = string.IsNullOrWhiteSpace(SelectedTable) ?
            new() : await GetAllLookupsAsync("ReferenceData?tableName=" + SelectedTable);
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<LookupDto>($"ReferenceData/{SelectedTable}/{id}");

        if (isSuccess)
            LookupList!.Remove(LookupList.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private async Task<IEnumerable<string>> GetTablesAsync(string value)
    {
        if (TablesNameList is not null)
            return TablesNameList;

        TablesNameList = await GetAllAsync<string>("ReferenceData/ShowTables");

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
