using FinanceManagement.Domain.Common;
using FinanceManagement.Domain.Enums;

namespace FinanceManagement.Domain.Entities;

public class MonthlyExpense : BaseEntity
{
    public int? AssetId { get; set; }
    public int PartnerId { get; set; }
    public int? EmployeeId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int CategoryId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public bool IsRecurring { get; set; } = false;
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public Employee? Employee { get; set; }
    public Partner? Partner { get; set; }
    public Asset? Asset { get; set; }
    public Category? Category { get; set; }
    public ICollection<EmployeeSalary> EmployeeSalaries { get; set; } = new List<EmployeeSalary>();

    // BUG: Amount can be negative
    // BUG: Month can be invalid (0, 13+)
    // BUG: Year can be unrealistic
    // BUG: No validation for required approvals above certain amount
}