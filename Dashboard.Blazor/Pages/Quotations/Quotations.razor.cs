using Dashboard.Blazor.Models.Consts;

namespace Dashboard.Blazor.Pages.Quotations;

public partial class Quotations
{
    private List<QuotationDto> quotations = new();
    private string searchString = string.Empty;

    private readonly string formUri = "Quotations/Form";
    private readonly string detailsUri = "Quotations/Details";

    protected override async Task OnInitializedAsync()
    {
        StartProcessing();

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Price Quotations"], href: null, disabled: true, icon: EntityIcons.QuotationsIcon),
        });

        //quotations = await GetAllAsync<QuotationDto>("Quotations?OrderBy=id&Asc=false");

        quotations = new()
        {
            new QuotationDto
            {
                Id = 1,
                ClientName ="Youssef Ahmed",
                Date = DateTime.Now.AddDays(-10),
                SerialNumber = "059753654",
                TotalPrice = 50000,
                QuotationServices = new()
                {
                    new QuotationServiceType
                    {
                        Category = new(){Id = 1,Name="الدهانات"},
                        Service = new(){Id=1,Name="دهانات الجدران"},
                        Area = 200,
                        Price = 4000,
                        DiscountRate = 25,
                        DiscountPrice = 1000,
                        Count = 2,
                        TotalPrice = 6000
                    },
                    new QuotationServiceType
                    {
                        Category = new(){Id = 1,Name="صيانة الكهرباء"},
                        Service = new(){Id=1,Name="تصليح إضاءه"},
                        Area = 100,
                        Price = 2000,
                        DiscountRate = 50,
                        DiscountPrice = 1000,
                        Count = 2,
                        TotalPrice = 2000
                    }
                }
            }
        };

        StopProcessing();
    }

    private async Task Delete(int id)
    {
        StartProcessing();

        var isSuccess = await DeleteAsync<QuotationDto>($"Quotations/{id}");

        if (isSuccess)
            quotations.Remove(quotations.FirstOrDefault(x => x.Id == id)!);

        StopProcessing();
    }

    private bool FilterFunc(QuotationDto element)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Date.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.SerialNumber.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.ClientName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }
}
