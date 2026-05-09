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
    public DateTime CheckInLocal { get => CheckIn.ToLocalTime(); }

    public DateTime? CheckOut { get; set; }

    public string? AlternateContactPerson { get; set; }
    public string? Notes { get; set; }

    public List<string>? AdditionalImages { get; set; }
    public string Image { get; set; } = null!;
    public IBrowserFile? UploadedImage { get; set; }

    //optional images
    public List<IBrowserFile>? UploadedImages { get; set; } = new();

    public string? ClientSignatureImage { get; set; }

    public string? SupervisorSignatureImage { get; set; }
    public string? ClientSignatureBase64 { get; set; }
    public string? SupervisorSignatureBase64 { get; set; }

    public int? CustomerReview { get; set; }  // 1..5

    public string? ReviewNote { get; set; }

    // =========================
    // M:M Providers (Bridge DTOs)
    // =========================
    //[MinCountIf("Visit.VisitTypeId", 1, (int)ServiceRequestTypeEnum.PestControl)]
    public ICollection<VisitReportsProviderDto> Providers { get; set; } = new List<VisitReportsProviderDto>();
    public IEnumerable<LookupDto>? ProvidersLookup { get; set; } // for autocomplete options

    public int? FloorsCount { get; set; }

    public string? Floors { get; set; }

    public int? RoomsCount { get; set; }
    [Required]
    public string? Rooms { get; set; } // e.g. "room1,room2,room4"
    [Required]
    public int? SuitesCount { get; set; }
    [Required]
    public int? KitchensCount { get; set; }
    [Required]
    public int? RestuarntsCount { get; set; }

    // =========================
    // Pest Control
    // =========================

    // =========================
    // M:M lookups (Bridge DTOs)
    // =========================

    public ICollection<VisitReportsPestsTreatmentTypeDto> PestsTreatmentTypes { get; set; } = new List<VisitReportsPestsTreatmentTypeDto>();
    public IEnumerable<LookupDto>? PestsTreatmentTypesLookup { get; set; } // for autocomplete options

    public ICollection<VisitReportsPestsTypeDto> PestsTypes { get; set; } = new List<VisitReportsPestsTypeDto>();
    public IEnumerable<LookupDto>? PestsTypesLookup { get; set; } // for autocomplete options

    public ICollection<VisitReportsChemicalDto> Chemicals { get; set; } = new List<VisitReportsChemicalDto>();
    public IEnumerable<LookupDto>? ChemicalsLookup { get; set; } // for autocomplete options

    public ICollection<VisitReportsInfectionDto> Infections { get; set; } = new List<VisitReportsInfectionDto>();
    public IEnumerable<LookupDto>? InfectionsLookup { get; set; } // for autocomplete options

    // =========================
    // 1:M lookup (single target pest type)
    // =========================
    public int? PestsActivityId { get; set; }

    [RequiredIf("Visit.VisitTypeId", (int)ServiceRequestTypeEnum.PestControl)]
    public LookupDto? PestsActivity { get; set; }
    public IEnumerable<LookupDto>? PestsActivities { get; set; }

    public int? TargetPestTypeId { get; set; }

    [RequiredIf("Visit.VisitTypeId", (int)ServiceRequestTypeEnum.PestControl)]
    public LookupDto? TargetPestType { get; set; }
    public IEnumerable<LookupDto>? TargetPestTypes { get; set; }

    public string? TreatmentMethodSummary { get; set; }

    [RequiredIf("Visit.VisitTypeId", (int)ServiceRequestTypeEnum.PestControl)]
    public string? AreasApplied { get; set; }

    [RequiredIf("Visit.VisitTypeId", (int)ServiceRequestTypeEnum.PestControl)]
    public string? AreasTreated { get; set; }

    [RequiredIf("Visit.VisitTypeId", (int)ServiceRequestTypeEnum.PestControl)]
    public float? DilutionPercent { get; set; }

    // =========================
    // Deep Cleaning
    // =========================
    public int? CleaningTypeId { get; set; }

    [RequiredIf("Visit.VisitTypeId", (int)ServiceRequestTypeEnum.DeepCleaning)]
    public LookupDto? CleaningType { get; set; }
    public IEnumerable<LookupDto>? CleaningTypes { get; set; }

    // keep as int to match your current dto style (0 => not selected)
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
    // M:M lookups (Bridge DTOs)
    //[MinCountIf("Visit.VisitTypeId", 1, (int)ServiceRequestTypeEnum.Maintenance)]
    public ICollection<VisitReportsMaintenanceTypeDto> MaintenanceTypes { get; set; } = new List<VisitReportsMaintenanceTypeDto>();
    public IEnumerable<LookupDto>? MaintenanceTypesLookup { get; set; } // for autocomplete options
}


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


#region BRIDGE GET DTOS - VisitReport <-> PestsTreatmentType (Lookup)
public class VisitReportsPestsTreatmentTypeDto
{
    public int VisitReportId { get; set; }
    public int PestsTreatmentTypeId { get; set; }
    public LookupDto PestsTreatmentType { get; set; } = null!;
}

public class VisitReportsPestsTreatmentTypeCreateDto
{
    public int PestsTreatmentTypeId { get; set; }
}
#endregion


#region BRIDGE GET DTOS - VisitReport <-> PestsType (Lookup)
public class VisitReportsPestsTypeDto
{
    public int VisitReportId { get; set; }
    public int PestsTypeId { get; set; }
    public LookupDto PestsType { get; set; } = null!;
}

public class VisitReportsPestsTypeCreateDto
{
    public int PestsTypeId { get; set; }
}
#endregion


#region BRIDGE GET DTOS - VisitReport <-> Chemicals (Lookup)
public class VisitReportsChemicalDto
{
    public int VisitReportId { get; set; }
    public int ChemicalId { get; set; }
    public LookupDto Chemical { get; set; }
}

public class VisitReportsChemicalCreateDto
{
    public int ChemicalId { get; set; }
}


#endregion


#region BRIDGE GET DTOS - VisitReport <-> MaintenanceType (Lookup)
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

#region BRIDGE GET DTOS - VisitReport <-> Infection (Lookup)
public class VisitReportsInfectionDto
{
    public int VisitReportId { get; set; }
    public int InfectionId { get; set; }
    public LookupDto Infection { get; set; } = null!;
}

public class VisitReportsInfectionCreateDto
{
    public int InfectionId { get; set; }
}
#endregion


public class VisitReportInitDto
{
    public int VisitId { get; set; }
}
