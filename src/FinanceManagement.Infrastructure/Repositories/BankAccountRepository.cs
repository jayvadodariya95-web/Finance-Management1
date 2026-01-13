using Microsoft.EntityFrameworkCore;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Infrastructure.Data;

namespace FinanceManagement.Infrastructure.Repositories;

public class BankAccountRepository : IBankAccountRepository
{
    private readonly FinanceDbContext _context;

    public BankAccountRepository(FinanceDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BankAccount>> GetAllAsync()
    {
        return await _context.BankAccounts
            .AsNoTracking()
            .Where(b => b.IsActive)
            .ToListAsync();
    }

    public async Task<BankAccount?> GetByIdAsync(int id)
    {
        return await _context.BankAccounts
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<BankAccount> CreateAsync(BankAccount account)
    {
        _context.BankAccounts.Add(account);
        await _context.SaveChangesAsync();
        return account;
    }

    public async Task<BankAccount> UpdateAsync(BankAccount account)
    {
        _context.BankAccounts.Update(account);
        await _context.SaveChangesAsync();
        return account;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var account = await _context.BankAccounts.FindAsync(id);
        if (account == null) return false;

        // Soft Delete (Better for Finance History)
        account.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }
}