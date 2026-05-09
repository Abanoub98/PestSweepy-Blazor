namespace Dashboard.Blazor.Models.Dtos;

public class VisitSheetsDto
{
    public int Id { get; set; }

    // Scope
    public int ContractId { get; set; }   // FK -> Contract
    public ContractBaseDto Contract { get; set; } = null!;
    public IEnumerable<ContractBaseDto> Contracts { get; set; } = null!;
    public int BranchId { get; set; }     // FK -> Branch
    public BranchBaseDto Branch { get; set; } = null!;
    public IEnumerable<BranchBaseDto> Branches { get; set; } = null!;
    public int SupervisorId { get; set; } // FK -> Supervisor
    public SupervisorBaseDto Supervisor { get; set; } = null!;
    public IEnumerable<SupervisorBaseDto> Supervisors { get; set; } = null!;

    // Plan inputs (from UI)
    [Range(1, int.MaxValue)]
    public int TotalVisits { get; set; }  // e.g. 50

    public VisitDays Days { get; set; }   // e.g. Sunday | Monday
    public string? DaysText { get; set; }

    [Required]
    public DateTime StartingDate { get; set; } // date-only meaning

    [Required]
    public TimeSpan VisitTime { get; set; }    // time-only meaning

    // Optional extras
    public bool IsActive { get; set; } = true;
    [MaxLength(500)]
    public string? Notes { get; set; }
    public List<VisitBaseDto> Visits { get; set; } = new();

    public int? VisitTypeId { get; set; }
    public LookupDto? VisitType { get; set; } = null!;
    public IEnumerable<LookupDto>? VisitTypes { get; set; } = null!;

}

public class BranchBaseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Manager { get; set; } = null!;
    [StringLength(30)]
    public string PhoneNumber { get; set; } = null!;

    // Location Details
    public int CityId { get; set; }
    public string Location { get; set; } = null!;
    public string LocationURL { get; set; } = null!;
    // End Of Location Details
    public int ClientId { get; set; }
    public string Email { get; set; } = null!;
    public LookupDto City { get; set; } = null!;
}

public class ContractBaseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime EffectiveDate { get; set; }
    public ContractClientInitialDto ContractClient { get; set; } = null!;
}

public class ContractClientInitialDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Image { get; set; } = string.Empty;
}

public class SupervisorBaseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Image { get; set; } = string.Empty;

    public int ManagerId { get; set; }
}

public class VisitBaseDto
{
    public int Id { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; } = null!;
    public int VisitTypeId { get; set; }
    public LookupDto VisitType { get; set; } = null!;

    // Link to the sheet (sheet holds Contract/Branch)
    public int VisitSheetId { get; set; }

    // Schedule
    [Required]
    public DateTime ScheduledAt { get; set; } // exact datetime

    public string Status { get; set; } = null!;

    // Completion
    public DateTime? CompletedAt { get; set; }
    public DateTime? CanceledAt { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public VisitReportBaseDto? VisitReport { get; set; }
    public int? VisitReportId { get; set; }
}


public class VisitSummaryDto
{
    public int Id { get; set; }

    public int VisitTypeId { get; set; }
    public LookupDto VisitType { get; set; } = null!;

    // Link to the sheet (sheet holds Contract/Branch)
    public int VisitSheetId { get; set; }

    // Schedule
    [Required]
    public DateTime ScheduledAt { get; set; } // exact datetime

    public string Status { get; set; } = null!;

    // Completion
    public DateTime? CompletedAt { get; set; }
    public DateTime? CanceledAt { get; set; }

    public string? Notes { get; set; }
    public int? VisitReportId { get; set; }
}

public class VisitReportBaseDto
{
    public int Id { get; set; }

    // =========================
    // General
    // =========================
    public int VisitId { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CheckIn { get; set; }

    public DateTime? CheckOut { get; set; }
}