namespace Dashboard.Blazor.Models.Dtos;

public class VisitSheetReportBaseDto
{
    public int Id { get; set; }
    public int VisitSheetId { get; set; }
    public int SequenceNumber { get; set; }
    public string ReportCode { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime PeriodFrom { get; set; }
    public DateTime PeriodTo { get; set; }
    public DateTime GeneratedAt { get; set; }
    public int TotalVisits { get; set; }
    public int ContractId { get; set; }
    public string ContractTitle { get; set; } = null!;
    public int ClientId { get; set; }
    public string ClientName { get; set; } = null!;
    public int BranchId { get; set; }
    public string BranchName { get; set; } = null!;
    public int VisitTypeId { get; set; }
    public string VisitTypeName { get; set; } = null!;
}

public class VisitSheetReportDto : VisitSheetReportBaseDto
{
    public string? ReportSummary { get; set; }
    public string? Recommendations { get; set; }
    public List<VisitSheetReportPestDto> Pests { get; set; } = new();
    public List<string>? Images { get; set; } = new();
    public List<IBrowserFile> UploadedImages { get; set; } = new();
}

public class VisitSheetReportGenerateDto
{
    [Required]
    public int VisitSheetId { get; set; }

    [Required]
    public DateTime PeriodFrom { get; set; }

    [Required]
    public DateTime PeriodTo { get; set; }
}

public class VisitSheetReportUpdateDto
{
    public string? ReportSummary { get; set; }

    public string? Recommendations { get; set; }
}

public class VisitSheetReportPestDto
{
    public int Id { get; set; }
    public int VisitSheetReportId { get; set; }
    public int PestTypeId { get; set; }
    public LookupDto PestType { get; set; } = null!;
    public int? HighestActivityId { get; set; }
    public LookupDto? HighestActivity { get; set; }
    public int? LowestActivityId { get; set; }
    public LookupDto? LowestActivity { get; set; }
    public string? ChemicalsUsed { get; set; }
    public string? InfectionCauses { get; set; }
    public string? AreasTreated { get; set; }
    public string? TreatmentTypesUsed { get; set; }
}
