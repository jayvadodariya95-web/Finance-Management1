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
    
    // Navigation properties
    public Partner ManagedByPartner { get; set; } = null!;
    public ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();
    public ICollection<BankTransaction> BankTransactions { get; set; } = new List<BankTransaction>();
    
    // BUG: ProjectValue can be negative
    // BUG: EndDate can be before StartDate
    // PERFORMANCE ISSUE: No index on ManagedByPartnerId
    // BUG: No validation for required fields
}