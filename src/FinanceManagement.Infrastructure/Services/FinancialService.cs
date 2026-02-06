using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Domain.Enums;
using FinanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagement.Infrastructure.Services;

public class FinancialService : IFinancialService
{
    private readonly IFinancialRepository _financialRepository;
    private readonly IPartnerRepository _partnerRepository;
    private readonly FinanceDbContext _context;

    public FinancialService(
        IFinancialRepository financialRepository,
        IPartnerRepository partnerRepository,
        FinanceDbContext context)
    {
        _financialRepository = financialRepository;
        _partnerRepository = partnerRepository;
        _context = context;
    }

    public async Task<MonthlyReportDto> GenerateMonthlyReportAsync(int month, int year)
    {
        // FIX: BUG-012 - Removed .Result blocking calls and used await
        var totalIncome = await _financialRepository.GetTotalIncomeAsync(month, year);
        var totalExpenses = await _financialRepository.GetTotalExpensesAsync(month, year);
        var totalSalaries = await _financialRepository.GetTotalSalariesAsync(month, year);

        var netIncome = totalIncome - totalExpenses - totalSalaries;

        var partnerIncomes = await CalculatePartnerIncomesAsync(month, year);
        var expenses = await _financialRepository.GetMonthlyExpensesAsync(month, year);

        return new MonthlyReportDto
        {
            Month = month,
            Year = year,
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            TotalSalaries = totalSalaries,
            NetIncome = netIncome,
            PartnerIncomes = partnerIncomes.ToList(),
            Expenses = expenses.Select(e => new ExpenseDto
            {
                Id = e.Id,
                Description = e.Description,
                Amount = e.Amount,
                Category = e.Category.ToString(),
                // FIX: BUG-004 - Added check for valid date creation
                Date = IsValidDate(e.Year, e.Month, 1) ? new DateTime(e.Year, e.Month, 1) : DateTime.MinValue,
                IsApproved = !string.IsNullOrEmpty(e.ApprovedBy)
            }).ToList()
        };
    }

    // Helper to prevent BUG-004 (Index out of range date)
    private bool IsValidDate(int year, int month, int day)
    {
        if (year < 1 || year > 9999 || month < 1 || month > 12 || day < 1) return false;
        return day <= DateTime.DaysInMonth(year, month);
    }

    public async Task<decimal> CalculateNetIncomeAsync(int month, int year)
    {
        var totalIncome = await _financialRepository.GetTotalIncomeAsync(month, year);
        var totalExpenses = await _financialRepository.GetTotalExpensesAsync(month, year);
        var totalSalaries = await _financialRepository.GetTotalSalariesAsync(month, year);

        return totalIncome - totalExpenses - totalSalaries;
    }

    public async Task<IEnumerable<PartnerIncomeDto>> CalculatePartnerIncomesAsync(int month, int year)
    {
        var partners = await _partnerRepository.GetMainPartnersAsync();
        var partnerIncomes = new List<PartnerIncomeDto>();

        //new logic added
        //var totalIncome = await _financialRepository.GetTotalIncomeAsync(month, year);
        var netIncome = await CalculateNetIncomeAsync(month, year);

        foreach (var partner in partners)
        {
            var actualIncome = await _financialRepository.GetPartnerIncomeAsync(partner.Id, month, year);
            //var expectedIncome = 200000m;
            // FIX: BUG-009 - Calculate share based on Profit
            var expectedIncome = partner.SharePercentage > 0
                ? Math.Round(netIncome * partner.SharePercentage / 100, 2)
                : 0;
            var settlementAmount = actualIncome - expectedIncome;

            var projectsCount = await _context.Projects
                .CountAsync(p => p.ManagedByPartnerId == partner.Id);

            //var partnerName = partner.User?.FirstName + " " + partner.User?.LastName;
            // FIX: BUG-002 - Null check
            var partnerName = partner.User == null
                ? "Unknown Partner"
                : $"{partner.User.FirstName} {partner.User.LastName}";


            partnerIncomes.Add(new PartnerIncomeDto
            {
                PartnerId = partner.Id,
                PartnerName = partnerName,
                ExpectedIncome = expectedIncome,
                ActualIncome = actualIncome,
                SettlementAmount = settlementAmount,
                ProjectsManaged = projectsCount
            });
        }

        return partnerIncomes;
    }

    public async Task ProcessSettlementsAsync(int month, int year)
    {
        var partners = await _partnerRepository.GetMainPartnersAsync();
        //var totalIncome = await _financialRepository.GetTotalIncomeAsync(month, year);
        // CHANGED: Use NetIncome for settlements
        var netIncome = await CalculateNetIncomeAsync(month, year);

        foreach (var partner in partners)
        {
            var alreadyExists = await _context.Settlements.AnyAsync(s =>
                s.PartnerId == partner.Id &&
                s.Month == month &&
                s.Year == year);

            if (alreadyExists)
                continue;

            var actualIncome = await _financialRepository.GetPartnerIncomeAsync(partner.Id, month, year);

            var expectedIncome = partner.SharePercentage > 0
                ? Math.Round(netIncome * partner.SharePercentage / 100, 2)
                : 0;

            var settlementAmount = actualIncome - expectedIncome;

            var settlement = new Settlement
            {
                PartnerId = partner.Id,
                Month = month,
                Year = year,
                ExpectedAmount = expectedIncome,
                ActualAmount = actualIncome,
                SettlementAmount = settlementAmount,
                Status = SettlementStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Settlements.Add(settlement);
        }

        await _context.SaveChangesAsync();
    }

    //public async Task ProcessSettlementsAsync(int month, int year)
    //{
    //    var partners = await _partnerRepository.GetMainPartnersAsync();

    //    foreach (var partner in partners)
    //    {
    //        var settlementAmount = await CalculatePartnerSettlementAsync(partner.Id, month, year);

    //        var settlement = new Settlement
    //        {
    //            PartnerId = partner.Id,
    //            Month = month,
    //            Year = year,
    //            ExpectedAmount = 200000m,
    //            ActualAmount = await _financialRepository.GetPartnerIncomeAsync(partner.Id, month, year),
    //            SettlementAmount = settlementAmount,
    //            Status = SettlementStatus.Pending,
    //            CreatedAt = DateTime.UtcNow
    //        };

    //        _context.Settlements.Add(settlement);
    //    }

    //    await _context.SaveChangesAsync();
    //}

    //public async Task<decimal> CalculatePartnerSettlementAsync(int partnerId, int month, int year)
    //{
    //    var partner = await _partnerRepository.GetByIdAsync(partnerId);
    //    if (partner == null) return 0;

    //    var actualIncome = await _financialRepository.GetPartnerIncomeAsync(partnerId, month, year);
    //    var expectedIncome = 200000m;
    //    var settlement = actualIncome - expectedIncome;

    //    return Math.Round(settlement, 2);
    //}
    public async Task<decimal> CalculatePartnerSettlementAsync(int partnerId, int month, int year)
    {
        var partner = await _partnerRepository.GetByIdAsync(partnerId);
        if (partner == null)
            return 0;

        //var totalIncome = await _financialRepository.GetTotalIncomeAsync(month, year);
        // CHANGED: Use NetIncome
        var netIncome = await CalculateNetIncomeAsync(month, year);
        var actualIncome = await _financialRepository.GetPartnerIncomeAsync(partnerId, month, year);

        var expectedIncome = partner.SharePercentage > 0
            ? Math.Round(netIncome * partner.SharePercentage / 100, 2)
            : 0;

        return Math.Round(actualIncome - expectedIncome, 2);
    }
}