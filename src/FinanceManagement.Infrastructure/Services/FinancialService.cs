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
        var totalIncome = _financialRepository.GetTotalIncomeAsync(month, year).Result;
        var totalExpenses = _financialRepository.GetTotalExpensesAsync(month, year).Result;
        var totalSalaries = _financialRepository.GetTotalSalariesAsync(month, year).Result;

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
                Date = new DateTime(e.Year, e.Month, 1),
                IsApproved = !string.IsNullOrEmpty(e.ApprovedBy)
            }).ToList()
        };
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

        foreach (var partner in partners)
        {
            var actualIncome = await _financialRepository.GetPartnerIncomeAsync(partner.Id, month, year);
            var expectedIncome = 200000m;
            var settlementAmount = actualIncome - expectedIncome;

            var projectsCount = await _context.Projects
                .CountAsync(p => p.ManagedByPartnerId == partner.Id);

            var partnerName = partner.User?.FirstName + " " + partner.User?.LastName;

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

        foreach (var partner in partners)
        {
            var settlementAmount = await CalculatePartnerSettlementAsync(partner.Id, month, year);
            
            var settlement = new Settlement
            {
                PartnerId = partner.Id,
                Month = month,
                Year = year,
                ExpectedAmount = 200000m,
                ActualAmount = await _financialRepository.GetPartnerIncomeAsync(partner.Id, month, year),
                SettlementAmount = settlementAmount,
                Status = SettlementStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Settlements.Add(settlement);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<decimal> CalculatePartnerSettlementAsync(int partnerId, int month, int year)
    {
        var partner = await _partnerRepository.GetByIdAsync(partnerId);
        if (partner == null) return 0;

        var actualIncome = await _financialRepository.GetPartnerIncomeAsync(partnerId, month, year);
        var expectedIncome = 200000m;
        var settlement = actualIncome - expectedIncome;
        
        return Math.Round(settlement, 2);
    }
}