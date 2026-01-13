using Microsoft.EntityFrameworkCore;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Infrastructure.Data;
using FinanceManagement.Application.DTOs;

namespace FinanceManagement.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly FinanceDbContext _context;

    public ProjectRepository(FinanceDbContext context)
    {
        _context = context;
    }

    // ✅ FIXED: Eager loading (No N+1)
    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects
            .Include(p => p.ManagedByPartner)
                .ThenInclude(p => p.User)
            .Include(p => p.ProjectEmployees)
                .ThenInclude(pe => pe.Employee)
                    .ThenInclude(e => e.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    //public async Task<Project?> GetByIdAsync(int id)
    //{
    //    return await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
    //}

    //public async Task<IEnumerable<Project>> GetAllAsync()
    //{
    //    var projects = await _context.Projects.ToListAsync();

    //    foreach (var project in projects)
    //    {
    //        project.ManagedByPartner = await _context.Partners
    //            .FirstOrDefaultAsync(p => p.Id == project.ManagedByPartnerId);
    //    }

    //    return projects;
    //}

    //Bug 005 - N+1 Query solved using Single sql Query
    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _context.Projects
            .Include(p => p.ManagedByPartner)
                .ThenInclude(p => p.User)
            .AsNoTracking()
            .ToListAsync();
    }


    //public async Task<IEnumerable<Project>> GetByPartnerAsync(int partnerId)
    //{
    //    return await _context.Projects
    //        .Where(p => p.ManagedByPartnerId == partnerId)
    //        .ToListAsync();
    //}

    public async Task<IEnumerable<Project>> GetByPartnerAsync(int partnerId)
    {
        return await _context.Projects
            .AsNoTracking()
            .Where(p => p.ManagedByPartnerId == partnerId)
            .Include(p => p.ProjectEmployees)
            .ToListAsync();
    }


    //public async Task<Project> CreateAsync(Project project)
    //{
    //    _context.Projects.Add(project);
    //    await _context.SaveChangesAsync();
    //    return project;
    //}

    // ✅ Existing validation preserved
    public async Task<Project> CreateAsync(Project project)
    {
        var partnerExists = await _context.Partners
            .AnyAsync(p => p.Id == project.ManagedByPartnerId);

        if (!partnerExists)
            throw new ArgumentException($"Invalid Partner ID: {project.ManagedByPartnerId}");

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

    // ✅ N+1 safe assignment check
    public async Task AssignEmployeeAsync(int projectId, int employeeId, string? role = null)
    {
        var exists = await _context.ProjectEmployees
            .AnyAsync(pe => pe.ProjectId == projectId &&
                            pe.EmployeeId == employeeId &&
                            pe.IsActive);

        if (exists)
            throw new InvalidOperationException("Employee already assigned.");

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

    // ✅ Optimized with AsNoTracking
    public async Task<Partner?> GetByIdAsync(int id)
    {
        return await _context.Partners
            .Include(p => p.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    // ✅ TASK-006 CORE FIX: DTO Projection
    public async Task<IEnumerable<PartnerListDto>> GetAllAsync()
    {
        return await _context.Partners
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.Projects)
            .Select(p => new PartnerListDto
            {
                Id = p.Id,
                FullName = p.User.FirstName + " " + p.User.LastName,
                Email = p.User.Email,
                IsMainPartner = p.IsMainPartner,
                ProjectCount = p.Projects.Count,
                SharePercentage = p.SharePercentage 
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<PartnerListDto>> GetMainPartnersAsync()
    {
        return await _context.Partners
            .AsNoTracking()
            .Where(p => p.IsMainPartner)
            .Include(p => p.User)
            .Include(p => p.Projects)
            .Select(p => new PartnerListDto
            {
                Id = p.Id,
                FullName = p.User.FirstName + " " + p.User.LastName,
                Email = p.User.Email,
                IsMainPartner = true,
                ProjectCount = p.Projects.Count,
                SharePercentage = p.SharePercentage
            })
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

    //public async Task<IEnumerable<Project>> GetPartnerProjectsAsync(int partnerId)
    //{
    //    var projects = await _context.Projects
    //        .Where(p => p.ManagedByPartnerId == partnerId)
    //        .ToListAsync();

    //    foreach (var project in projects)
    //    {
    //        var projectEmployees = await _context.ProjectEmployees
    //            .Where(pe => pe.ProjectId == project.Id)
    //            .Include(pe => pe.Employee)
    //            .ThenInclude(e => e.User)
    //            .ToListAsync();

    //        project.ProjectEmployees = projectEmployees;
    //    }

    //    return projects;
    //}

    // ✅ N+1 FIXED: Single SQL query
    public async Task<IEnumerable<Project>> GetPartnerProjectsAsync(int partnerId)
    {
        return await _context.Projects
            .AsNoTracking()
            .Where(p => p.ManagedByPartnerId == partnerId)
            .Include(p => p.ProjectEmployees)
                .ThenInclude(pe => pe.Employee)
                    .ThenInclude(e => e.User)
            .ToListAsync();
    }

}