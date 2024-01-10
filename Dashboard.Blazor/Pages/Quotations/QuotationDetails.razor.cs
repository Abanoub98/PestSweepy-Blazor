namespace Dashboard.Blazor.Pages.Quotations;

public partial class QuotationDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private QuotationDto? quotation;
    private readonly string formUri = "Quotations/Form";
    private int ServiceNumber;

    protected override async Task OnParametersSetAsync()
    {
        quotation = await GetByIdAsync<QuotationDto>($"Quotations/{Id}");

        if (quotation is null)
            return;

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Quotations"], href: "/Quotations", icon: Icons.Material.TwoTone.Diversity3),
            new(quotation.SerialNumber, href: null, disabled: true),
        });
    }

    private async Task Print()
    {


        DialogOptions dialogOptions = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            Position = DialogPosition.Center,
            CloseButton = true
        };

        //DialogParameters<PricingReport> formParameters = new()
        //{
        //    { x => x.ImageUrl, imageUrl }
        //};

        await DialogService.ShowAsync<PricingReport>("Image Preview", dialogOptions);
    }
}
