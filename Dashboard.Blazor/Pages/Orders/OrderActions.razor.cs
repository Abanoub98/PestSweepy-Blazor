namespace Dashboard.Blazor.Pages.Orders;

public partial class OrderActions
{
    [Parameter][EditorRequired] public OrderDto? Order { get; set; }
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;
}
