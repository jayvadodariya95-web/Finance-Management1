using FinanceManagement.Domain.Common;
using FinanceManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinanceManagement.Domain.Entities;

public class Project : BaseEntity, IValidatableObject
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ClientName { get; set; } = string.Empty;
    [Range(0, double.MaxValue, ErrorMessage = "Project Value must be positive")]
    public decimal ProjectValue { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ProjectStatus Status { get; set; }
    public int? ManagedByPartnerId { get; set; }

    // Navigation properties
    public Partner? ManagedByPartner { get; set; } = null!;
    public ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();
    public ICollection<BankTransaction> BankTransactions { get; set; } = new List<BankTransaction>();

    // ✅ FIX BUG-017: Implement the Validate Method
    // This runs automatically when you try to Save or Model Bind
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Rule: End Date cannot be before Start Date
        if (EndDate < StartDate)
        {
            yield return new ValidationResult(
                "Project End Date cannot be earlier than Start Date.",
                new[] { nameof(EndDate) } // Highlights the "EndDate" field in the error
            );
        }
    }

        // BUG: ProjectValue can be negative
        // BUG: EndDate can be before StartDate
        // PERFORMANCE ISSUE: No index on ManagedByPartnerId
        // BUG: No validation for required fields
    }