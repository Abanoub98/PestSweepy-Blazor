namespace Dashboard.Blazor.Pages.Complaints;

public partial class ComplaintDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private ComplaintDto? complaint;

    protected override async Task OnParametersSetAsync()
    {
        complaint = await GetByIdAsync<ComplaintDto>($"Complaints/{Id}");

        if (complaint is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Complaints"], href: "/Complaints", icon: EntityIcons.ComplaintIcon),
            new(complaint.Id.ToString(), href: null, disabled: true),
        });
    }
}
