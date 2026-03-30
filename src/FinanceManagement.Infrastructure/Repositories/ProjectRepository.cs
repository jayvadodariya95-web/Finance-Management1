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
                            .ThenInclude(mp => mp.User)
                        .Include(p => p.ProjectEmployees)
                            .ThenInclude(pe => pe.Employee)
                                .ThenInclude(e => e.User)
            .FirstOrDefaultAsync(p => p.Id == id &&  p.IsDeleted == false);

    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        var projects = await _context.Projects
                        .Include(p => p.ManagedByPartner)
                            .ThenInclude(mp => mp.User)
                        .Include(p => p.ProjectEmployees)
                            .ThenInclude(pe => pe.Employee)
                                .ThenInclude(e => e.User)
                        .Where(d => d.IsDeleted == false)
                        .ToListAsync();
        return projects;
    }

    public async Task<IEnumerable<Project>> GetByPartnerAsync(int partnerId)
    {
        return await _context.Projects
            .Where(p => p.ManagedByPartnerId == partnerId && p.IsDeleted == false)
            .ToListAsync();
    }

    public async Task<Project> CreateAsync(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<Project> UpdateAsync(Project project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<ProjectEmployee> AssignEmployeeAsync(ProjectEmployee assign)
    {
        await _context.ProjectEmployees.AddAsync(assign);
        await _context.SaveChangesAsync();
        return assign;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Projects.FindAsync(id);
        if (existing == null || existing.IsDeleted) return false;

        existing.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }
}

