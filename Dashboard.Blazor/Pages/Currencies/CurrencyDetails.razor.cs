namespace Dashboard.Blazor.Pages.Currencies;

public partial class CurrencyDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    //private CurrencyDto? currency;

    //protected override async Task OnParametersSetAsync()
    //{
    //    currency = await GetByIdAsync<CurrencyDto>($"Currencies/{Id}");

    //    if (currency is null)
    //        return;

    //    breadcrumbItems.AddRange(new List<BreadcrumbItem>
    //    {
    //        new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
    //        new(languageContainer.Keys["Currencies"], href: "/Currencies", icon: EntityIcons.CurrencyIcon),
    //        new(currency.Name, href: null, disabled: true),
    //    });
    //}
}
