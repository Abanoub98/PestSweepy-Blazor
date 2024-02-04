namespace Dashboard.Blazor.Pages.Complaints;

public partial class Complaints
{
    private List<ComplaintDto> complaints = new();

    private readonly string detailsUri = "Complaints/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems = new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Complaints"], href: null, disabled: true, icon: EntityIcons.ComplaintIcon),
        };

        complaints = await GetAllAsync<ComplaintDto>("Complaints?OrderBy=id&Asc=false");

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<ComplaintDto>($"Complaints/{id}");

        if (isSuccess)
        {
            complaints.Remove(complaints.FirstOrDefault(x => x.Id == id)!);

            if (selectedIds.Contains(id))
                selectedIds.Remove(id);
        }

        StopProcessing();
    }

    private void SelectedItemsChanged(HashSet<ComplaintDto> items) => selectedIds = items.Select(i => i.Id).ToList();

    private async Task DeleteAll()
    {
        StartProcessing();

        var isSuccess = await DeleteAllAsync<ComplaintDto>($"Complaints/DeleteMultiple", selectedIds);

        if (isSuccess)
        {
            complaints.RemoveAll(x => selectedIds.Contains(x.Id));
            selectedIds = new();
        }

        StopProcessing();
    }

    private async Task ToggleStatus(ComplaintDto complaint)
    {
        StartProcessing();

        var isSuccess = await ShowConfirmation($"Are you sure that you will {(complaint.IsResolved ? "decline" : "resolve")} this complaint", true);

        if (isSuccess)
        {
            var result = await UpdateAsync($"Complaints/UpdateComplaintState/{complaint.Id}", complaint);

            if (result.isSuccess)
                complaint.IsResolved = !complaint.IsResolved;
        }

        StopProcessing();
    }

    private bool FilterFunc(ComplaintDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;

        return false;
    }
}
