using FinanceManagement.Domain.Common;
using FinanceManagement.Domain.Entities;

public class Revenue : BaseEntity
{
    public int Partner_Id { get; set; }
    public int? Project_Id { get; set; }

    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public bool Revenue_From { get; set; } = true;
    public string? Notes { get; set; }

    // Navigation properties
    public Partner? Partner { get; set; }   // ✅ SINGLE object
    public Project? Project { get; set; }
}
