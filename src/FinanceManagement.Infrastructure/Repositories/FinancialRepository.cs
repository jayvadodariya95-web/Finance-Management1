using Microsoft.EntityFrameworkCore;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Domain.Enums;
using FinanceManagement.Infrastructure.Data;

namespace FinanceManagement.Infrastructure.Repositories;

public class FinancialRepository : IFinancialRepository
{
    private readonly FinanceDbContext _context;

    public FinancialRepository(FinanceDbContext context)
    {
        _context = context;
    }

    public async Task<decimal> GetTotalIncomeAsync(int month, int year)
    {
        // PERFORMANCE ISSUE: No index on TransactionDate
        var transactions = await _context.BankTransactions
            .Where(t => t.TransactionDate.Month == month && 
                       t.TransactionDate.Year == year &&
                       t.Type == TransactionType.Income)
            .ToListAsync();

        // BUG: Using LINQ Sum on client side instead of database
        return transactions.Sum(t => t.Amount);
    }

    public async Task<decimal> GetTotalExpensesAsync(int month, int year)
    {
        // BUG: Including both MonthlyExpenses and BankTransaction expenses - double counting
        var monthlyExpenses = await _context.MonthlyExpenses
            .Where(e => e.Month == month && e.Year == year)
            .SumAsync(e => e.Amount);

        var bankExpenses = await _context.BankTransactions
            .Where(t => t.TransactionDate.Month == month && 
                       t.TransactionDate.Year == year &&
                       t.Type == TransactionType.Expense)
            .SumAsync(t => t.Amount);

        return monthlyExpenses + bankExpenses; // BUG: Double counting
    }

    public async Task<decimal> GetTotalSalariesAsync(int month, int year)
    {
        // PERFORMANCE ISSUE: Loading all employees to calculate total salary
        var employees = await _context.Employees
            .Where(e => e.IsActive)
            .ToListAsync();

        // BUG: Calculating on client side, should be done in database
        return employees.Sum(e => e.MonthlySalary);
    }

    public async Task<IEnumerable<MonthlyExpense>> GetMonthlyExpensesAsync(int month, int year)
    {
        // PERFORMANCE ISSUE: Missing index on Month + Year
        return await _context.MonthlyExpenses
            .Where(e => e.Month == month && e.Year == year)
            .ToListAsync();
    }

    public async Task<decimal> GetPartnerIncomeAsync(int partnerId, int month, int year)
    {
        // PERFORMANCE ISSUE: Multiple database calls instead of single query
        var partnerProjects = await _context.Projects
            .Where(p => p.ManagedByPartnerId == partnerId)
            .Select(p => p.Id)
            .ToListAsync();

        decimal totalIncome = 0;
        
        // BUG: Calculating in a loop instead of single query
        foreach (var projectId in partnerProjects)
        {
            var projectIncome = await _context.BankTransactions
                .Where(t => t.ProjectId == projectId &&
                           t.TransactionDate.Month == month &&
                           t.TransactionDate.Year == year &&
                           t.Type == TransactionType.Income)
                .SumAsync(t => t.Amount);
            
            totalIncome += projectIncome;
        }

        return totalIncome;
    }
}

public class BankTransactionRepository : IBankTransactionRepository
{
    private readonly FinanceDbContext _context;

    public BankTransactionRepository(FinanceDbContext context)
    {
        _context = context;
    }

    public async Task<BankTransaction?> GetByIdAsync(int id)
    {
        return await _context.BankTransactions
            .Include(t => t.BankAccount)
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<BankTransaction>> GetAllAsync()
    {
        // PERFORMANCE ISSUE: Loading all transactions without pagination
        return await _context.BankTransactions
            .Include(t => t.BankAccount)
            .Include(t => t.Project)
            .ToListAsync();
    }

    public async Task<IEnumerable<BankTransaction>> GetByProjectAsync(int projectId)
    {
        return await _context.BankTransactions
            .Where(t => t.ProjectId == projectId)
            .Include(t => t.BankAccount)
            .ToListAsync();
    }

    public async Task<IEnumerable<BankTransaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        // PERFORMANCE ISSUE: Missing index on TransactionDate
        return await _context.BankTransactions
            .Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate)
            .Include(t => t.BankAccount)
            .Include(t => t.Project)
            .ToListAsync();
    }

    public async Task<BankTransaction> CreateAsync(BankTransaction transaction)
    {
        // BUG: No concurrency control for balance updates
        _context.BankTransactions.Add(transaction);
        
        // BUG: Updating bank account balance without proper locking
        var bankAccount = await _context.BankAccounts
            .FirstOrDefaultAsync(ba => ba.Id == transaction.BankAccountId);
        
        if (bankAccount != null)
        {
            if (transaction.Type == TransactionType.Income)
                bankAccount.Balance += transaction.Amount;
            else
                bankAccount.Balance -= transaction.Amount;
        }

        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task<BankTransaction> UpdateAsync(BankTransaction transaction)
    {
        _context.BankTransactions.Update(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }
}