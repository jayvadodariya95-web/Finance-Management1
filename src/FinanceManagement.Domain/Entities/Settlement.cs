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

    //public decimal CalculateSettlement()
    //{
    //    return ActualAmount - ExpectedAmount;
    //}

    // ✅ FIX BUG-003: Gracefully handle Zero Percentage
    // This method ensures we never divide by zero or process invalid shares
    public void CalculateSettlement(decimal netIncome, decimal partnerSharePercentage, decimal       actualDrawnAmount)
    {
        //if (partnerSharePercentage <= 0)
        //{
        //    ExpectedAmount = 0;
        //}
        //else
        //{
        //    // Standard Calculation: (Total Profit * Share%) / 100
        //    ExpectedAmount = Math.Round(netIncome * (partnerSharePercentage / 100m), 2);
        //}
        ExpectedAmount = partnerSharePercentage > 0
        ? Math.Round(netIncome * (partnerSharePercentage / 100m), 2)
        : 0;

        ActualAmount = actualDrawnAmount;
        SettlementAmount = ActualAmount - ExpectedAmount;
    }
}