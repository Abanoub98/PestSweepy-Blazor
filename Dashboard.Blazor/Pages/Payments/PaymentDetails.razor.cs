namespace Dashboard.Blazor.Pages.Payments;

public partial class PaymentDetails
{
    [Parameter][EditorRequired] public string Id { get; set; } = null!;

    private PaymentDto? payment;

    protected override async Task OnParametersSetAsync()
    {
        payment = await GetByIdAsync<PaymentDto>($"Payments/{Id}");

        if (payment is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Payments"], href: "/Payments", icon: Icons.Material.Outlined.Payment),
            new(payment.Id, href: null, disabled: true),
        });
    }
}
