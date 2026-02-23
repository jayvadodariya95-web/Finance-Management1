using FinanceManagement.Domain.Common;
using FinanceManagement.Domain.Enums;

namespace FinanceManagement.Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public decimal ProjectValue { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ProjectStatus Status { get; set; }
    public int ManagedByPartnerId { get; set; }
    public int? ProfileId { get; set; }
    public string? TechnologyStack { get; set; }
    public string? ManagerName { get; set; }
    public string? ManagerEmail { get; set; }
    public string? ManagerContact { get; set; }
    public string? LeaveApplyWay { get; set; }
    public string? ClientManagerName { get; set; }
    public string? ClientManagerEmail { get; set; }
    public string? ClientManagerContact { get; set; }
    public bool IsSmooth { get; set; } = false;
    public string? MobileNumberUsed { get; set; }
    public int? InterviewingUserId { get; set; }
    public bool? IsToolUsed { get; set; }
    // Navigation properties
    public Profile? Profile { get; set; }
    public Partner ManagedByPartner { get; set; } = null!;
    public ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();
    public ICollection<BankTransaction> BankTransactions { get; set; } = new List<BankTransaction>();
    public Revenue? Revenue { get; set; }
    public User? InterviewingUser { get; set; }

    // BUG: ProjectValue can be negative
    // BUG: EndDate can be before StartDate
    // PERFORMANCE ISSUE: No index on ManagedByPartnerId
    // BUG: No validation for required fields
}