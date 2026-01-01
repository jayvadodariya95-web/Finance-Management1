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
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public bool IsApproved { get; set; }
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

// BUG: No proper decimal precision handling for financial calculations
// BUG: Missing currency information
// BUG: Date handling without timezone consideration