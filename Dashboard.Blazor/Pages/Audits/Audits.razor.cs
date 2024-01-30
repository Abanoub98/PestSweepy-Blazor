namespace Dashboard.Blazor.Pages.Audits;

public partial class Audits
{
    private List<AuditDto> audits = new();
    private string searchString = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Audits"], href: null, disabled: true, icon: @Icons.Material.Outlined.History),
        });

        audits = await GetAllAsync<AuditDto>("SecuirtyLogs/Audits?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private void ShowAuditDetailsDialog(int id)
    {
        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            Position = DialogPosition.Center,
            CloseButton = true
        };

        DialogParameters<AuditDetails> formParameters = new()
        {
            { x => x.Id, id }
        };

        DialogService.Show<AuditDetails>(languageContainer.Keys["Audit Details"], formParameters, dialogOptions);
    }

    private bool FilterFunc(AuditDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.TableName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Type.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.UserId.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.DateTimeLocal.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
