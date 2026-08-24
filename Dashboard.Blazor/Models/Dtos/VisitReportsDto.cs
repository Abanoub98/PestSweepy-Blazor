namespace Dashboard.Blazor.Models.Dtos;

public class VisitReportsDto
{
    public int Id { get; set; }

    // =========================
    // General
    // =========================
    public int BranchId { get; set; }
    public string BranchName { get; set; } = null!;
    public int VisitId { get; set; }
    public VisitSummaryDto Visit { get; set; } = null!;
    public bool IsCheckOut { get; set; }
    public string? Status { get; set; }

    [Required]
    public DateTime CheckIn { get; set; }

    public DateTime CheckInLocal => CheckIn.ToLocalTime();

    public DateTime? CheckOut { get; set; }

    public string? AlternateContactPerson { get; set; }
    public string? Notes { get; set; }

    public List<string>? AdditionalImages { get; set; }
    public string Image { get; set; } = null!;
    public IBrowserFile? UploadedImage { get; set; }

    // Optional images
    public List<IBrowserFile>? UploadedImages { get; set; } = new();

    public string? ClientSignatureImage { get; set; }
    public string? SupervisorSignatureImage { get; set; }
    public string? ClientSignatureBase64 { get; set; }
    public string? SupervisorSignatureBase64 { get; set; }

    public int? CustomerReview { get; set; }
    public string? ReviewNote { get; set; }

    // =========================
    // M:M Providers
    // =========================
    public ICollection<VisitReportsProviderDto> Providers { get; set; }
        = new List<VisitReportsProviderDto>();

    public IEnumerable<LookupDto>? ProvidersLookup { get; set; }

    public int? FloorsCount { get; set; }
    public string? Floors { get; set; }

    public int? RoomsCount { get; set; }

    [Required]
    public string? Rooms { get; set; }

    [Required]
    public int? SuitesCount { get; set; }

    [Required]
    public int? KitchensCount { get; set; }

    [Required]
    public int? RestuarntsCount { get; set; }

    // =========================
    // Pest Control
    // =========================
    public ICollection<VisitReportPestDto> Pests { get; set; }
        = new List<VisitReportPestDto>();

    // Shared lookup options used by every pest card
    public IEnumerable<LookupDto>? PestsTypesLookup { get; set; }
    public IEnumerable<LookupDto>? PestsActivities { get; set; }
    public IEnumerable<LookupDto>? InfectionsLookup { get; set; }
    public IEnumerable<LookupDto>? PestsTreatmentTypesLookup { get; set; }
    public IEnumerable<LookupDto>? ChemicalsLookup { get; set; }

    // =========================
    // Deep Cleaning
    // =========================
    public int? CleaningTypeId { get; set; }

    [RequiredIf("Visit.VisitTypeId", (int)ServiceRequestTypeEnum.DeepCleaning)]
    public LookupDto? CleaningType { get; set; }

    public IEnumerable<LookupDto>? CleaningTypes { get; set; }

    public int? UnitId { get; set; }

    [RequiredIf("Visit.VisitTypeId", (int)ServiceRequestTypeEnum.DeepCleaning)]
    public LookupDto? Unit { get; set; }

    public IEnumerable<LookupDto>? Units { get; set; }

    [RequiredIf("Visit.VisitTypeId", (int)ServiceRequestTypeEnum.DeepCleaning)]
    public float? MeasureArea { get; set; }

    // =========================
    // Building Surface
    // =========================
    public int? SurfaceTypeId { get; set; }

    [RequiredIf("Visit.VisitTypeId", (int)ServiceRequestTypeEnum.BuildingSurface)]
    public LookupDto? SurfaceType { get; set; }

    public IEnumerable<LookupDto>? SurfaceTypes { get; set; }

    public int? WorkWayId { get; set; }

    [RequiredIf("Visit.VisitTypeId", (int)ServiceRequestTypeEnum.BuildingSurface)]
    public LookupDto? WorkWay { get; set; }

    public IEnumerable<LookupDto>? WorkWays { get; set; }

    // =========================
    // Maintenance
    // =========================
    public ICollection<VisitReportsMaintenanceTypeDto> MaintenanceTypes { get; set; }
        = new List<VisitReportsMaintenanceTypeDto>();

    public IEnumerable<LookupDto>? MaintenanceTypesLookup { get; set; }
}

#region CHILD DTO - VisitReport -> Pests

public class VisitReportPestDto
{
    public int Id { get; set; }
    public int VisitReportId { get; set; }

    public int PestTypeId { get; set; }

    [Required]
    public LookupDto? PestType { get; set; }

    public int? PestsActivityId { get; set; }

    [Required]
    public LookupDto? PestsActivity { get; set; }

    public int? InfectionId { get; set; }

    [Required]
    public LookupDto? Infection { get; set; }

    public int? PestsTreatmentTypeId { get; set; }

    [Required]
    public LookupDto? PestsTreatmentType { get; set; }

    public int? ChemicalId { get; set; }

    [Required]
    public LookupDto? Chemical { get; set; }

    [Required]
    [Range(0, 100)]
    public float? DilutionPercent { get; set; }

    [MaxLength(1000)]
    public string? TreatmentMethodSummary { get; set; }

    [Required]
    [MaxLength(1000)]
    public string? AreasApplied { get; set; }

    [Required]
    [MaxLength(1000)]
    public string? AreasTreated { get; set; }
}

#endregion

#region BRIDGE GET DTOS - VisitReport <-> Providers

public class VisitReportsProviderDto
{
    public int VisitReportId { get; set; }
    public int ProviderId { get; set; }
    public LookupDto Provider { get; set; } = null!;
}

public class VisitReportsProviderCreateDto
{
    public int ProviderId { get; set; }
}

#endregion

#region BRIDGE GET DTOS - VisitReport <-> MaintenanceType

public class VisitReportsMaintenanceTypeDto
{
    public int VisitReportId { get; set; }
    public int MaintenanceTypeId { get; set; }
    public LookupDto MaintenanceType { get; set; } = null!;
}

public class VisitReportsMaintenanceTypeCreateDto
{
    public int MaintenanceTypeId { get; set; }
}

#endregion

public class VisitReportInitDto
{
    public int VisitId { get; set; }
}

public class VisitReportLegacyDto
{
    public int Id { get; set; }
    public int OriginalVisitReportId { get; set; }

    public string? PestsTypes { get; set; }
    public string? PestsTreatmentTypes { get; set; }
    public string? Chemicals { get; set; }
    public string? Infections { get; set; }

    public int? PestsActivityId { get; set; }
    public string? PestsActivityName { get; set; }

    public int? TargetPestTypeId { get; set; }
    public string? TargetPestTypeName { get; set; }

    public string? TreatmentMethodSummary { get; set; }
    public string? AreasApplied { get; set; }
    public string? AreasTreated { get; set; }
    public float? DilutionPercent { get; set; }
}