using FinanceManagement.Domain.Common;

namespace FinanceManagement.Domain.Entities;

public class Employee : BaseEntity
{
    public int UserId { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public decimal MonthlySalary { get; set; }
    public DateTime JoinDate { get; set; }
    public int? BranchId { get; set; }
    public bool IsActive { get; set; } = true;
    
    public User User { get; set; } = null!;
    public Branch? Branch { get; set; }
    public ICollection<ProjectEmployee> ProjectAssignments { get; set; } = new List<ProjectEmployee>();
}