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
    public class ProfileRepository : IProfileRepository
    {
        private readonly FinanceDbContext _context;

        public ProfileRepository(FinanceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Profile>> GetAllAsync()
        {
            return await _context.Profiles
                .Include(p => p.User)
                .Where(p => p.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Profile?> GetByIdAsync(int id)
        {
            return await _context.Profiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
        }

        public async Task<Profile> CreateAsync(Profile profile)
        {
            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();
            return profile;
        }

        public async Task<Profile> UpdateAsync(Profile profile)
        {
            _context.Profiles.Update(profile);
            await _context.SaveChangesAsync();
            return profile;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Profiles.FindAsync(id);
            if (existing == null || existing.IsDeleted) return false;
            existing.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
