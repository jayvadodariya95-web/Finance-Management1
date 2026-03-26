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
    public class AssetRepository : IAssetRepository
    {
        private readonly FinanceDbContext _context;

        public AssetRepository(FinanceDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Asset>> GetAllAsync()
        {
            return await _context.Asset
                .Include(a => a.MonthlyExpenses)
                .Where(a => a.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Asset?> GetByIdAsync(int id)
        {
            return await _context.Asset
                .Include(a => a.MonthlyExpenses)
                .Where(a => a.IsDeleted == false)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Asset> AddAsync(Asset asset)
        {
            await _context.Asset.AddAsync(asset);
            await _context.SaveChangesAsync();
            return asset;
        }

        public async Task<Asset> Update(Asset asset)
        {
             _context.Asset.Update(asset);
            await _context.SaveChangesAsync();
            return asset;
        }

        public void Delete(Asset asset)
        {
            _context.Asset.Remove(asset);
        }
    }
}
