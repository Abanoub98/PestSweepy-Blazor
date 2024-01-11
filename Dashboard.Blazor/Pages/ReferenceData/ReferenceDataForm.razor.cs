namespace Dashboard.Blazor.Pages.ReferenceData;

public partial class ReferenceDataForm
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public int Id { get; set; }
    [Parameter] public string? TableName { get; set; }

    private LookupDto? lookupDto { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        lookupDto = (Id == 0) ? new() : await GetByIdAsync<LookupDto>($"ReferenceData/{TableName}/{Id}");

        if (lookupDto is null)
            return;
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        (bool isSuccess, LookupDto? obj) result;

        if (Id == 0)
            result = await AddAsync($"ReferenceData?tableName={TableName}", lookupDto!);
        else
            result = await UpdateAsync($"ReferenceData?tableName={TableName}&id={Id}", lookupDto!);

        if (result.isSuccess) Ok();

        StopProcessing();
    }

    private void Ok() => MudDialog.Close(DialogResult.Ok(true));
}
