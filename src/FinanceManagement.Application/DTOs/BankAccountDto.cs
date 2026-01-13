using System.ComponentModel.DataAnnotations;

namespace FinanceManagement.Application.DTOs;

public class BankAccountDto
{
    public int Id { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty; // Masked in UI usually
    public string AccountHolderName { get; set; } = string.Empty;
    public string IfscCode { get; set; } = string.Empty;
    public decimal CurrentBalance { get; set; }
    public string Currency { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class CreateBankAccountDto
{
    [Required]
    public string BankName { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{9,18}$", ErrorMessage = "Invalid Account Number")]
    public string AccountNumber { get; set; } = string.Empty;

    [Required]
    public string AccountHolderName { get; set; } = string.Empty;

    [Required]
    public string IfscCode { get; set; } = string.Empty;

    public decimal InitialBalance { get; set; } = 0;
    public string Currency { get; set; } = "INR";
}