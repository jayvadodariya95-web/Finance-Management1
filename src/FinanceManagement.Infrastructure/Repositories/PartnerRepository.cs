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
    public class PartnerRepository : IPartnerRepository
    {
        private readonly FinanceDbContext _context;

        public PartnerRepository(FinanceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Partner>> GetAllAsync()
        {
            return await _context.Partners
                .Include(p => p.User)
                .Include(p => p.Branch)
                .Where(p => p.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Partner?> GetByIdAsync(int id)
        {
            return await _context.Partners
                .Include(p => p.User)
                .Include(p => p.Branch)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
        }

        public async Task AddAsync(Partner partner)
        {
            await _context.Partners.AddAsync(partner);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Partner partner)
        {
            _context.Partners.Update(partner);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Partner partner)
        {
            _context.Partners.Remove(partner);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Partner>> GetMainPartnersAsync()
        {
            return await _context.Partners
                .Include(p => p.User)
                .Include(p => p.Branch)
                .Where(p => p.IsMainPartner)
                .ToListAsync();
        } 

    }
}
