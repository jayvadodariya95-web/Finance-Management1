using FinanceManagement.Domain.Common;

namespace FinanceManagement.Domain.Entities;

public class Employee : BaseEntity
{
    public string EmployeeCode { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public int MonthlySalary { get; set; }
    public int? Previous_CTC { get; set; }
    public int Current_CTC { get; set; }
    public DateTime JoinDate { get; set; }
    public int? Taken_Leave { get; set; }
    public DateTime? RelievingDate { get; set; }
    public int? BranchId { get; set; }
    public bool IsActive { get; set; } = true;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public Branch? Branch { get; set; }
    public ICollection<ProjectEmployee> ProjectAssignments { get; set; } = new List<ProjectEmployee>();
}
