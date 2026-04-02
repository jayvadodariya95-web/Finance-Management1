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

        public async Task<bool> DeleteAsync()
        {
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RevenueDTO>> GetAllAsync()
        {
            return await _context.Revenue
                .AsNoTracking()
                .Where(e => !e.IsDeleted)
                .Select(r => new RevenueDTO
                {
                    Id = r.Id,
                    Amount = r.Amount,
                    Date = r.Date,
                    Revenue_From = r.Revenue_From,
                    Notes = r.Notes,

                    PartnerId = r.PartnerId,

                    PartnerName = r.Partner.User != null
                        ? r.Partner.User.FirstName + " " + r.Partner.User.LastName
                        : null,

                    ProjectId = r.ProjectId,
                    ProjectName = r.Project != null ? r.Project.Name : null
                })
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
