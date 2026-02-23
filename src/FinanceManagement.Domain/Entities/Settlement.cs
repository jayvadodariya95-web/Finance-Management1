using FinanceManagement.Domain.Common;
using FinanceManagement.Domain.Enums;

namespace FinanceManagement.Domain.Entities;

public class Settlement : BaseEntity
{
    public int PartnerId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public int ExpectedAmount { get; set; }
    public int ActualAmount { get; set; }
    public int SettlementAmount { get; set; }
    public SettlementStatus Status { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public string? Notes { get; set; }
    
    public Partner Partner { get; set; } = null!;
    
    public decimal CalculateSettlement()
    {
        return ActualAmount - ExpectedAmount;
    }
    public int TotalExpense { get; set; }
    public int GrossProfit { get; set; }
    public int NetProfit { get; set; }
    public int IsSetteled { get; set; }
    public DateTime? SettledDate { get; set; }
    public int CalculateNetProfit()
    {
        return GrossProfit - TotalExpense;
    }

}