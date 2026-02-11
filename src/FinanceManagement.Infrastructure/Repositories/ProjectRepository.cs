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
        return await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        var projects = await _context.Projects.ToListAsync();
        
        foreach (var project in projects)
        {
            project.ManagedByPartner = await _context.Partners
                .FirstOrDefaultAsync(p => p.Id == project.ManagedByPartnerId);
        }
        
        return projects;
    }

    public async Task<IEnumerable<Project>> GetByPartnerAsync(int partnerId)
    {
        return await _context.Projects
            .Where(p => p.ManagedByPartnerId == partnerId)
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

    public async Task AssignEmployeeAsync(int projectId, int employeeId, string? role = null)
    {
        var assignment = new ProjectEmployee
        {
            ProjectId = projectId,
            EmployeeId = employeeId,
            Role = role,
            AssignedDate = DateTime.UtcNow,
            IsActive = true
        };

        _context.ProjectEmployees.Add(assignment);
        await _context.SaveChangesAsync();
    }
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