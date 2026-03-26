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
    public class DocTypeRepository : IDocTypeRepository
    {
        private readonly FinanceDbContext _context;

        public DocTypeRepository(FinanceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DocType>> GetAllAsync()
        {
            return await _context.DocTypes
                .Where(d => d.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<DocType?> GetByIdAsync(int id)
        {
            return await _context.DocTypes.FindAsync(id);
        }

        public async Task<DocType> AddAsync(DocType docType)
        {
            await _context.DocTypes.AddAsync(docType);
            await _context.SaveChangesAsync();
            return docType;
        }

        public async Task<DocType?> UpdateAsync(DocType docType)
        {
            var existing = await _context.DocTypes.FindAsync(docType.Id);
            if (existing == null) return null;

            existing.TypeName = docType.TypeName;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.DocTypes.FindAsync(id);
            if (existing == null) return false;

            _context.DocTypes.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DocType?> PatchAsync(int id, string? typeName)
        {
            var existing = await _context.DocTypes.FindAsync(id);
            if (existing == null) return null;

            if (typeName != null)
                existing.TypeName = typeName;

            await _context.SaveChangesAsync();
            return existing;
        }
    }
}
