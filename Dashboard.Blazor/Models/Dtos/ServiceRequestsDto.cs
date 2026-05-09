namespace Dashboard.Blazor.Models.Dtos;

public class ServiceRequestsDto
{
    public int Id { get; set; }

    //Entity Details
    [StringLength(255)]
    public string EntityName { get; set; } = null!;

    [StringLength(30)]
    public string? EntityPhone { get; set; }

    [StringLength(255)]
    public string? EntityEmail { get; set; }

    [StringLength(255)]
    public string City { get; set; } = null!;
    public string? Branch { get; set; }
    public string Location { get; set; } = null!;
    public string? LocationURL { get; set; }
    // End Of Entity Details

    public int ServiceRequestTypeId { get; set; }
    public LookupDto ServiceRequestType { get; set; } = null!;
    public IEnumerable<LookupDto> ServiceRequestTypes { get; set; } = null!;

    // =========================
    // Pest Control SR
    // =========================
    public int? BuildingTypeId { get; set; }
    public LookupDto? BuildingType { get; set; }
    public IEnumerable<LookupDto>? BuildingTypes { get; set; }
    public int? PestsTypeId { get; set; }
    public LookupDto? PestsType { get; set; }
    public IEnumerable<LookupDto>? PestsTypes { get; set; }
    public int? PestsTreatmentTypeId { get; set; }
    public LookupDto? PestsTreatmentType { get; set; }
    public IEnumerable<LookupDto>? PestsTreatmentTypes { get; set; }
    public int? InfectionPercent { get; set; }
    public int? RoomsCount { get; set; }
    public int? SuitesCount { get; set; }
    public int? KitchensCount { get; set; }
    public int? RestuarntsCount { get; set; }
    //End Of Pest Control SR

    // =========================
    // Deep Cleaning SR
    // =========================
    public int? CleaningTypeId { get; set; }
    public LookupDto? CleaningType { get; set; }
    public IEnumerable<LookupDto>? CleaningTypes { get; set; }
    public int? EntityCount { get; set; }
    //End Of Deep Cleaning SR


    // =========================
    // Building Surface SR
    // =========================
    public int? SurfaceTypeId { get; set; }
    public LookupDto? SurfaceType { get; set; }
    public IEnumerable<LookupDto>? SurfaceTypes { get; set; }
    public int? WorkWayId { get; set; }
    public LookupDto? WorkWay { get; set; }
    public IEnumerable<LookupDto>? WorkWays { get; set; }
    //End Of Building Surface SR

    // =========================
    // General Properties
    // =========================
    public int? FloorsCount { get; set; }
    public int? UnitId { get; set; }
    public LookupDto? Unit { get; set; }
    public IEnumerable<LookupDto>? Units { get; set; }
    public float? MeasureArea { get; set; }
    public string? CheckupSummary { get; set; }
    public string? Notes { get; set; }
    public List<string>? ServiceRequestAttachments { get; set; }
    public List<IBrowserFile>? UploadedImages { get; set; } = new();
    //End Of General Properties
}

//public class ServiceBaseDto
//{
//    public int Id { get; set; }
//    public string NameAr { get; set; } = null!;
//    public string NameEn { get; set; } = null!;
//    public int? OrderIndex { get; set; }
//    public string? Image { get; set; }
//    public int CategoryId { get; set; }
//    public string? Description { get; set; }
//    public int? DurationInMinutes { get; set; }
//}