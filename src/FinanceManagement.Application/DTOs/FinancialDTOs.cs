namespace FinanceManagement.Application.DTOs;

public class MonthlyReportDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal TotalSalaries { get; set; }
    public decimal NetIncome { get; set; }
    public List<PartnerIncomeDto> PartnerIncomes { get; set; } = new();
    public List<ExpenseDto> Expenses { get; set; } = new();
}

public class PartnerIncomeDto
{
    public int PartnerId { get; set; }
    public string PartnerName { get; set; } = string.Empty;
    public decimal ExpectedIncome { get; set; }
    public decimal ActualIncome { get; set; }
    public decimal SettlementAmount { get; set; }
    public int ProjectsManaged { get; set; }
}

public class ExpenseDto
{
    public int Id { get; set; }
    public int? AssetId { get; set; }
    public int PartnerId { get; set; }
    public int? EmployeeId { get; set; }

    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int CategoryId { get; set; } 

    public int Month { get; set; }
    public int Year { get; set; }

    public bool IsRecurring { get; set; }

    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }

    public string? EmployeeName { get; set; }
    public string? PartnerName { get; set; }
    public string? AssetName { get; set; }

    // Optional computed field
    public bool IsApproved => ApprovedDate != null;

    // Optional: Combined date for frontend
    public DateTime ExpenseDate => new DateTime(Year, Month, 1);
}

public class BankTransactionDto
{
    public int Id { get; set; }
    public string BankAccountName { get; set; } = string.Empty;
    public string? ProjectName { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public bool IsProcessed { get; set; }
}

public class SettlementDto
{
    public int Id { get; set; }
    public string PartnerName { get; set; } = string.Empty;
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal ExpectedAmount { get; set; }
    public decimal ActualAmount { get; set; }
    public decimal SettlementAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? ProcessedDate { get; set; }
}

// Sum of Bank Transaction + Monthly Expense 
public class TotalExpenseDTO
{
    public decimal TotalExpenseAmount { get; set; }
}
// BUG: No proper decimal precision handling for financial calculations
// BUG: Missing currency information
// BUG: Date handling without timezone consideration