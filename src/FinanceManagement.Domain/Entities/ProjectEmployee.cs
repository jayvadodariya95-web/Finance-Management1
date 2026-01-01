using FinanceManagement.Domain.Common;

namespace FinanceManagement.Domain.Entities;

public class ProjectEmployee : BaseEntity
{
    public int ProjectId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime AssignedDate { get; set; }
    public DateTime? UnassignedDate { get; set; }
    public string? Role { get; set; }
    public decimal? HourlyRate { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public Project Project { get; set; } = null!;
    public Employee Employee { get; set; } = null!;
    
    // BUG: Can assign same employee to same project multiple times
    // BUG: HourlyRate can be negative
    // BUG: UnassignedDate can be before AssignedDate
}