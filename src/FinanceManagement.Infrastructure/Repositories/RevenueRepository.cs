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
    public class RevenueRepository : IRevenueRepository
    {
        public readonly FinanceDbContext _context;

        public RevenueRepository(FinanceDbContext context)
        {
            this._context = context; 
        }

        public async Task<Revenue> CreateAsync(Revenue revenue)
        {
            await _context.Revenue.AddAsync(revenue);
            await _context.SaveChangesAsync();
            return revenue;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existRevenue = await _context.Revenue.FindAsync(id);
            if (existRevenue == null || existRevenue.IsDeleted == true)
            {
                return false;
            }

            existRevenue.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Revenue>> GetAllAsync()
        {
            return await _context.Revenue
                        .Where(e => !e.IsDeleted)
                        .ToListAsync();
        }

        public async Task<Revenue?> GetByIdAsync(int id)
        {
            return await _context.Revenue
           .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<Revenue?> UpdateAsync(Revenue revenue)
        {
            _context.Revenue.Update(revenue);
            await _context.SaveChangesAsync();
            return revenue;
        }
    }
}
