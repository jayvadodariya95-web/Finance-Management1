using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagement.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FinanceDbContext financeDb;

        public CategoryRepository(FinanceDbContext financeDb)
        {
            this.financeDb = financeDb;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await financeDb.Categories.AddAsync(category);
            await financeDb.SaveChangesAsync();
            return category;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var category = await financeDb.Categories.FindAsync(id);

            if (category == null || category.IsDeleted == true)
                return false;

            category.IsDeleted = true;
            category.UpdatedAt = DateTime.Now;

            await financeDb.SaveChangesAsync();

            return true;
        }


        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await financeDb.Categories
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await financeDb.Categories
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task<Category> UpdateAsync(int id, Category category)
        {
            var existing = await financeDb.Categories.FindAsync(id);

            if (existing == null || existing.IsDeleted == true)
                throw new Exception("Category not found");

            existing.CategoryName = category.CategoryName;
            existing.IsRecurring = category.IsRecurring;
            existing.UpdatedAt = DateTime.Now;

            await financeDb.SaveChangesAsync();

            return existing;
        }
    }
}
