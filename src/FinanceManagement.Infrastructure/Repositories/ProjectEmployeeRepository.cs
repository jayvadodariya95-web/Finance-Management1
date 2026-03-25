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
    public class ProjectEmployeeRepository : IProjectEmployeeRepository
    {
        private readonly FinanceDbContext _context;

        public ProjectEmployeeRepository(FinanceDbContext context)
        {
            _context = context;
        }


        public async Task<ProjectEmployee> AddAsync(ProjectEmployee entity)
        {
            await _context.ProjectEmployees.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }


        public async Task<bool> IsActiveAssignmentExists(int projectId, int employeeId)
        {
            return await _context.ProjectEmployees
                .AnyAsync(x => x.ProjectId == projectId
                            && x.EmployeeId == employeeId
                            && x.IsActive);
        }


        public async Task<bool> UnassignAsync(int projectId, int employeeId)
        {
            var entity = await _context.ProjectEmployees
                .FirstOrDefaultAsync(x => x.ProjectId == projectId
                                      && x.EmployeeId == employeeId
                                      && x.IsActive);

            if (entity == null)
                return false;

            entity.IsActive = false;
            entity.UnassignedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<ProjectEmployee>> GetByProjectIdAsync(int projectId)
        {
            return await _context.ProjectEmployees
                .Where(x => x.ProjectId == projectId && x.IsActive)
                .Include(x => x.Employee)
                .AsNoTracking()
                .ToListAsync();
        }

      
        public async Task<IEnumerable<ProjectEmployee>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.ProjectEmployees
                .Where(x => x.EmployeeId == employeeId && x.IsActive)
                .Include(x => x.Project)
                .AsNoTracking()
                .ToListAsync();
        }

     
        public async Task<ProjectEmployee?> GetAsync(int projectId, int employeeId)
        {
            return await _context.ProjectEmployees
                .FirstOrDefaultAsync(x => x.ProjectId == projectId
                                      && x.EmployeeId == employeeId
                                      && x.IsActive);
        }
    }
}
