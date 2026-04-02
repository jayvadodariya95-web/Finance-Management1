using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Repositories
{
    public class ExpenseRepository : IMonthlyExpenseRepository
    {
        private readonly FinanceDbContext _context;

        public ExpenseRepository(FinanceDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<ExpenseDto>> GetAllAsync()
        {
            var users = _context.Users;

                return await _context.MonthlyExpenses
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted)
                    .Select(x => new ExpenseDto
                    {
                        Id = x.Id,
                        AssetId = x.AssetId,
                        PartnerId = x.PartnerId,
                        EmployeeId = x.EmployeeId,

                        Description = x.Description,
                        Amount = x.Amount,
                        CategoryId = x.CategoryId,

                        Month = x.Month,
                        Year = x.Year,
                        IsRecurring = x.IsRecurring,

                        ApprovedBy = x.ApprovedBy,
                        ApprovedDate = x.ApprovedDate,

                        EmployeeName = x.Employee != null && x.Employee.User != null
                            ? ((x.Employee.User.FirstName ?? "") + " " + (x.Employee.User.LastName ?? "")).Trim()
                            : null,

                        PartnerName = x.Partner != null && x.Partner.User != null
                            ? ((x.Partner.User.FirstName ?? "") + " " + (x.Partner.User.LastName ?? "")).Trim()
                            : null,
                        AssetName = x.Asset != null ? x.Asset.Name : null,
                        CategoryName = x.Category != null ? x.Category.CategoryName : null
                            
                    })
                    .ToListAsync();
        }

        public async Task<MonthlyExpense?> GetByIdAsync(int id)
        {
            return await _context.MonthlyExpenses
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<MonthlyExpense> AddAsync(MonthlyExpense entity)
        {
            await _context.MonthlyExpenses.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<MonthlyExpense?> UpdateAsync(MonthlyExpense entity)
        {
            var existing = await _context.MonthlyExpenses.FindAsync(entity.Id);
            if (existing == null || existing.IsDeleted) return null;

            existing.AssetId = entity.AssetId;
            existing.PartnerId = entity.PartnerId;
            existing.EmployeeId = entity.EmployeeId == 0 ? null : entity.EmployeeId;
            existing.Description = entity.Description;
            existing.Amount = entity.Amount;
            existing.Category = entity.Category;
            existing.Month = entity.Month;
            existing.Year = entity.Year;
            existing.IsRecurring = entity.IsRecurring;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.MonthlyExpenses.FindAsync(id);
            if (existing == null || existing.IsDeleted) return false;

            existing.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<MonthlyExpense>> GetByMonthYearAsync(int month, int year)
        {
            return await _context.MonthlyExpenses
                .Where(x => x.Month == month && x.Year == year && !x.IsDeleted)
                .ToListAsync();
        }
    }
}
