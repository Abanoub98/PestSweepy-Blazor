
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
            new BreadcrumbItem(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new BreadcrumbItem(languageContainer.Keys["Audits"], href: null, disabled: true, icon: @Icons.Material.TwoTone.Archive),
        });

        audits = await GetAllAsync("SecuirtyLogs/Audits");

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
        if (element.DateTime.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
