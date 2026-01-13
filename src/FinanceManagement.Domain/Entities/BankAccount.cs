using FinanceManagement.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManagement.Domain.Entities;

public class BankAccount : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string AccountNumber { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string BankName { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    public string AccountHolderName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal CurrentBalance { get; set; }
    [Required]
    [MaxLength(10)]
    public string Currency { get; set; } = "USD"; // Default INR

    [MaxLength(20)]
    public string IfscCode { get; set; } = string.Empty; // For India payments

    [MaxLength(20)]
    public string SwiftCode { get; set; } = string.Empty; // For International

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