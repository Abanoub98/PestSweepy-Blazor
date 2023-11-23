using Dashboard.Blazor.Models.Consts;

namespace Dashboard.Blazor.Pages.Quotations;

public partial class QuotationForm
{
    [Parameter][EditorRequired] public int Id { get; set; }
    private QuotationDto? quotationForm;
    private bool IsExistClient;
    private int ServiceNumber;

    protected override async Task OnParametersSetAsync()
    {
        if (Id == 0)
        {
            quotationForm = new();
            AddService();
        }
        else
        {
            //quotationForm = await GetByIdAsync<QuotationDto>($"Quotations/{Id}");
            quotationForm = new QuotationDto
            {
                Id = 1,
                ClientName = "Youssef Ahmed",
                ReceiverEmail = "YoussefAhmed@gmail.com",
                Notes = " 2.\tيجب تسليم عرض سعركم حتى ___________________(التاريخ والوقت). سوف يتم فتح عروض الاسعار علنياً وبحضور ممثلي المقاولين الذين يختاروا الحضور، يوم ___________________________(نفس التاريخ كما لتسليم عرض السعر) في تمام الساعة_______  (فورا بعد الموعد النهائي لتقديم عروض الاسعار) على العنوان التالي: ____________________ (عنوان الشارع، رقم الغرفة، رقم الهاتف)لمساعدتكم في تحضير عرض سعركم نرفق لكم المخططات، المواصفات وجداول الكميات، وعينة نموذج لتقديم سعركم. يجب أن يكون عرض سعركم في النموذج المرفق محكم الإغلاق في مغلف ومعنون ليسلم الى العنوان التالي:",
                Date = DateTime.Now.AddDays(-10),
                SerialNumber = "059753654",
                TotalPrice = 50000,
                QuotationServices = new()
                    {
                        new QuotationServiceType
                        {
                            Category = new(){Id = 1,Name="الدهانات"},
                            Service = new(){Id=1,Name="دهانات الجدران"},
                            Unit="شقة صغيرة",
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
                            Unit="فيلا 3 ادوار",
                            Area = 100,
                            Price = 2000,
                            DiscountRate = 50,
                            DiscountPrice = 1000,
                            Count = 2,
                            TotalPrice = 2000
                        }
                    }
            };

            if (quotationForm is null)
                return;
        }

        breadcrumbItems.AddRange(new List<BreadcrumbItem>
        {
            new(languageContainer.Keys["Home"], href: "/", icon: Icons.Material.Filled.Home),
            new(languageContainer.Keys["Price Quotations"], href: "/Quotations", icon: EntityIcons.QuotationsIcon),
            new((Id == 0 ? $"{languageContainer.Keys["Add"]} {languageContainer.Keys["Price Quotation"]}" : $"{languageContainer.Keys["Edit"]} {quotationForm.SerialNumber}"), href: null, disabled: true),
        });
    }

    private void TotalValueChanged()
    {
        quotationForm!.TotalPrice = quotationForm!.QuotationServices.Select(q => q.TotalPrice).Sum();
    }

    private void AddService()
    {
        quotationForm!.QuotationServices.Add(new QuotationServiceType());
    }

    private void DeleteService(QuotationServiceType service)
    {
        quotationForm!.QuotationServices.Remove(service);
        quotationForm!.TotalPrice -= service.TotalPrice;
    }

    private async Task OnValidSubmit(EditContext context)
    {
        StartProcessing();

        quotationForm!.ClientId = quotationForm.Client?.Id;

        (bool result, QuotationDto? quotationDtoResult) = (Id == 0) ?
            await AddAsync("Quotations", quotationForm!) :
            await UpdateAsync($"Quotations/{Id}", quotationForm!);

        if (result)
        {
            if (Id == 0)
                quotationForm!.Id = quotationDtoResult!.Id;

            NavigationManager.NavigateTo("/Quotations");
        }

        StopProcessing();
    }

    private async Task<IEnumerable<LookupDto>> GetClients(string value)
    {
        if (quotationForm!.Clients is null)
            quotationForm.Clients = await GetAllLookupsAsync("Clients");

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return quotationForm.Clients;

        return quotationForm.Clients.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
}
