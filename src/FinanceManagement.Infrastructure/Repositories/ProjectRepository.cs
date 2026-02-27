using Microsoft.EntityFrameworkCore;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Infrastructure.Data;

namespace FinanceManagement.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly FinanceDbContext _context;

    public ProjectRepository(FinanceDbContext context)
    {
        _context = context;
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects
              .Include(p => p.ManagedByPartner)
                  .ThenInclude(part => part.User)
              .Include(p => p.ProjectEmployees)
                  .ThenInclude(pe => pe.Employee)
                      .ThenInclude(e => e.User)
              .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted==false);
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _context.Projects
            .Where(p=> !p.IsDeleted)
             .Include(p => p.ManagedByPartner)
                 .ThenInclude(part => part.User)
             .ToListAsync();
    }
    public async Task<Project> CreateAsync(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<Project> UpdateAsync(Project project, int id)
    {
        var data = await _context.Projects.Where(p => p.Id == id && !p.IsDeleted).FirstOrDefaultAsync();
        if (data == null) return null;
        _context.Projects.Update(data);
        await _context.SaveChangesAsync();
        return data;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var data = await _context.Projects.FindAsync(id);
        if(data == null || data.IsDeleted)
        {
            return false;
        }
        data.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public class PartnerRepository : IPartnerRepository
    {
        private readonly FinanceDbContext _context;

        public PartnerRepository(FinanceDbContext context)
        {
            _context = context;
        }

        public async Task<Partner?> GetByIdAsync(int id)
        {
            return await _context.Partners
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Partner>> GetAllAsync()
        {
            return await _context.Partners
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Partner>> GetMainPartnersAsync()
        {
            return await _context.Partners
                .Include(p => p.User)
                .Where(p => p.IsMainPartner)
                .ToListAsync();
        }

        public async Task<Partner> CreateAsync(Partner partner)
        {
            _context.Partners.Add(partner);
            await _context.SaveChangesAsync();
            return partner;
        }

        public async Task<Partner> UpdateAsync(Partner partner)
        {
            _context.Partners.Update(partner);
            await _context.SaveChangesAsync();
            return partner;
        }

        public async Task<IEnumerable<Project>> GetPartnerProjectsAsync(int partnerId)
        {
            var projects = await _context.Projects
                .Where(p => p.ManagedByPartnerId == partnerId)
                .ToListAsync();

            foreach (var project in projects)
            {
                var projectEmployees = await _context.ProjectEmployees
                    .Where(pe => pe.ProjectId == project.Id)
                    .Include(pe => pe.Employee)
                    .ThenInclude(e => e.User)
                    .ToListAsync();

                project.ProjectEmployees = projectEmployees;
            }

            return projects;
        }
    }
}