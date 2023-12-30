namespace Dashboard.Blazor.Pages.Quotations;

public partial class QuotationDetails
{
    [Parameter][EditorRequired] public int Id { get; set; }

    private QuotationDto? quotation;
    private readonly string formUri = "Quotations/Form";
    private int ServiceNumber;

    protected override async Task OnParametersSetAsync()
    {
        //quotation = await GetByIdAsync<QuotationDto>($"Quotations/{Id}");

        quotation = new QuotationDto
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
