using FinanceManagement.Domain.Common;
using FinanceManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManagement.Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int ProfileId { get; set; }
    public string? TechnologyStack { get; set; }
    public string ManagerName { get; set; } = string.Empty;
    public string ManagerEmail { get; set; } = string.Empty;
    public string ManagerContact { get; set; } = string.Empty;
    public string? LeaveApplyWay { get; set; }
    public bool IsSmooth { get; set; } = false;
    public string ClientManagerName { get; set; } = string.Empty;
    public string ClientManagerEmail { get; set; } = string.Empty;
    public string ClientManagerContact { get; set; } = string.Empty;
    public string? MobileNumberUsed { get; set; }
    public int? InterviewingUserId { get; set; }
    public bool? IsToolUsed { get; set; }
    public int ManagedByPartnerId { get; set; }

    // Navigation Property
    public Partner? ManagedByPartner { get; set; }
    public ICollection<BankTransaction> BankTransactions { get; set; } = new List<BankTransaction>();
    public ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();
    public Profile Profile { get; set; } = null!;
}