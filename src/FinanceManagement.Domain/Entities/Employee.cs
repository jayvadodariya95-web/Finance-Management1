using FinanceManagement.Domain.Common;

namespace FinanceManagement.Domain.Entities;

public class Employee : BaseEntity
{
    public int UserId { get; set; }
    public int BranchId { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public int MonthlySalary { get; set; }
    public int? PreviousCTC { get; set; }
    public int CurrentCTC { get; set; }
    public DateTime JoinDate { get; set; }
    public DateTime? RelievingDate { get; set; }
    public int TakenLeave { get; set; }
    public bool IsActive { get; set; } = true;
    public User User { get; set; } = null!;
    public Branch? Branch { get; set; }
    public ICollection<ProjectEmployee> ProjectEmployee { get; set; } = new List<ProjectEmployee>();
    public ICollection<EmployeeDocument> EmployeeDocuments { get; set; } = new List<EmployeeDocument>();
    public ICollection<MonthlyExpense> MonthlyExpenses { get; set; } = new List<MonthlyExpense>();
    public ICollection<EmployeeSalary> EmployeeSalaries { get; set; } = new List<EmployeeSalary>();
}