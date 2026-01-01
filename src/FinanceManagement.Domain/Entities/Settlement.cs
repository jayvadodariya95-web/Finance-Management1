using FinanceManagement.Domain.Common;
using FinanceManagement.Domain.Enums;

namespace FinanceManagement.Domain.Entities;

public class Settlement : BaseEntity
{
    public int PartnerId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal ExpectedAmount { get; set; }
    public decimal ActualAmount { get; set; }
    public decimal SettlementAmount { get; set; }
    public SettlementStatus Status { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public string? Notes { get; set; }
    
    public Partner Partner { get; set; } = null!;
    
    public decimal CalculateSettlement()
    {
        return ActualAmount - ExpectedAmount;
    }
}