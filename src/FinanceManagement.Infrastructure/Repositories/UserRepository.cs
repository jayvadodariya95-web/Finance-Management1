using Microsoft.EntityFrameworkCore;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Infrastructure.Data;

namespace FinanceManagement.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FinanceDbContext _context;

    public UserRepository(FinanceDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        // PERFORMANCE ISSUE: Missing index on Email field
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> CreateAsync(User user)
    {
        // BUG: No validation for duplicate emails
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(int id)
    {
        // BUG: Hard delete instead of soft delete
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}

public class EmployeeRepository : IEmployeeRepository
{
    private readonly FinanceDbContext _context;

    public EmployeeRepository(FinanceDbContext context)
    {
        _context = context;
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.User)
            .Include(e => e.Branch)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .Include(e => e.User)
            .Include(e => e.Branch)
            .ToListAsync();
    }

    public async Task<Employee> CreateAsync(Employee employee)
    {
        // BUG: No validation for duplicate employee codes
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<Employee> UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<IEnumerable<Project>> GetEmployeeProjectsAsync(int employeeId)
    {
        // PERFORMANCE ISSUE: N+1 query problem
        var projectEmployees = await _context.ProjectEmployees
            .Where(pe => pe.EmployeeId == employeeId && pe.IsActive)
            .ToListAsync();

        var projects = new List<Project>();
        
        // BUG: Loading projects in a loop
        foreach (var pe in projectEmployees)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == pe.ProjectId);
            if (project != null)
            {
                projects.Add(project);
            }
        }

        return projects;
    }
}