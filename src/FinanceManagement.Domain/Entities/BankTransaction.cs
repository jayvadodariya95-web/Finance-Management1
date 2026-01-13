using FinanceManagement.Domain.Common;
using FinanceManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinanceManagement.Domain.Entities;

public class BankTransaction : BaseEntity
{
    public int BankAccountId { get; set; }
    public int? ProjectId { get; set; }
    // ✅ FIX BUG-016: Transactions cannot be zero or negative
    [Range(0.01, double.MaxValue, ErrorMessage = "Transaction Amount must be greater than zero")]
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public DateTime TransactionDate { get; set; }
    public bool IsProcessed { get; set; } = false;

    // Navigation properties
    public BankAccount BankAccount { get; set; } = null!;
    public Project? Project { get; set; }
    
    // BUG: No concurrency control - can process same transaction multiple times
    // BUG: Amount can be zero or negative for income
    // PERFORMANCE ISSUE: Missing index on TransactionDate
    // BUG: No validation for required fields
}