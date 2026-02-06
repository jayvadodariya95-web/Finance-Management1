using FinanceManagement.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace FinanceManagement.Domain.Entities;

public class BankAccount : BaseEntity
{
    public string AccountNumber { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public string AccountHolderName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "USD";
    public bool IsActive { get; set; } = true;

    // ✅ FIX BUG-013: Optimistic Concurrency Control
    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;

    // Navigation properties
    public ICollection<BankTransaction> Transactions { get; set; } = new List<BankTransaction>();
    
    // BUG: AccountNumber can be duplicate
    // BUG: Balance calculation not thread-safe
    // BUG: No validation for account number format
}