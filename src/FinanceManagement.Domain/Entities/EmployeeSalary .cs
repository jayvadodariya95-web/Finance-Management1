using FinanceManagement.Domain.Common;

namespace FinanceManagement.Domain.Entities;

public class EmployeeSalary : BaseEntity
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public int PartnerId { get; set; }
    public Partner Partner { get; set; } = null!;

    public int ExpenseId { get; set; }
    public MonthlyExpense MonthlyExpense { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime SalaryDate { get; set; }
}