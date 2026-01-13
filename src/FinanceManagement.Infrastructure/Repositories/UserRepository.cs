using Microsoft.EntityFrameworkCore;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Infrastructure.Data;
using FinanceManagement.Application.DTOs;

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

        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> CreateAsync(User user)
    {
        
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

//public class EmployeeRepository : IEmployeeRepository
//{
//    private readonly FinanceDbContext _context;

//    public EmployeeRepository(FinanceDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<Employee?> GetByIdAsync(int id)
//    {
//        return await _context.Employees
//            .Include(e => e.User)
//            .Include(e => e.Branch)
//            .FirstOrDefaultAsync(e => e.Id == id);
//    }

//    public async Task<IEnumerable<Employee>> GetAllAsync()
//    {
//        return await _context.Employees
//            .Include(e => e.User)
//            .Include(e => e.Branch)
//            .ToListAsync();
//    }

//    public async Task<Employee> CreateAsync(Employee employee)
//    {
//        // BUG: No validation for duplicate employee codes
//        _context.Employees.Add(employee);
//        await _context.SaveChangesAsync();
//        return employee;
//    }

//    public async Task<Employee> UpdateAsync(Employee employee)
//    {
//        _context.Employees.Update(employee);
//        await _context.SaveChangesAsync();
//        return employee;
//    }

//    public async Task<IEnumerable<Project>> GetEmployeeProjectsAsync(int employeeId)
//    {
//        // PERFORMANCE ISSUE: N+1 query problem
//        var projectEmployees = await _context.ProjectEmployees
//            .Where(pe => pe.EmployeeId == employeeId && pe.IsActive)
//            .ToListAsync();

//        var projects = new List<Project>();

//        // BUG: Loading projects in a loop
//        foreach (var pe in projectEmployees)
//        {
//            var project = await _context.Projects
//                .FirstOrDefaultAsync(p => p.Id == pe.ProjectId);
//            if (project != null)
//            {
//                projects.Add(project);
//            }
//        }

//        return projects;
//    }
//}

public class EmployeeRepository : IEmployeeRepository
{
    private readonly FinanceDbContext _context;

    public EmployeeRepository(FinanceDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync(string? search)
    {
        var query = _context.Employees
            .Include(e => e.User)
            .Include(e => e.Branch)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e =>
                e.User.FirstName.Contains(search) ||
                e.User.LastName.Contains(search));
        }

        return await query
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                //FullName = e.User.FirstName + " " + e.User.LastName,
                Email = e.User.Email,
                BranchName = e.Branch != null ? e.Branch.Name : string.Empty,
                IsActive = e.IsActive
            })
            .ToListAsync();
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.User)
            .Include(e => e.Branch)
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                Email = e.User.Email,
                BranchName = e.Branch != null ? e.Branch.Name : string.Empty,
                IsActive = e.IsActive
            })
            .FirstOrDefaultAsync();
    }


    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
    {
        var employee = new Employee
        {
            UserId = dto.UserId,
            BranchId = dto.BranchId,
            IsActive = true
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(employee.Id)
            ?? throw new Exception("Employee creation failed");
    }

    public async Task<EmployeeDto> UpdateAsync(int id, UpdateEmployeeDto dto)
    {
        var employee = await _context.Employees.FindAsync(id)
            ?? throw new KeyNotFoundException("Employee not found");

        employee.BranchId = dto.BranchId;
        employee.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        return await GetByIdAsync(id)
            ?? throw new Exception("Employee update failed");
    }

    public async Task DeleteAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null)
            throw new KeyNotFoundException("Employee not found");

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }


    public async Task<IEnumerable<ProjectDto>> GetEmployeeProjectsAsync(int employeeId)
    {
        return await _context.ProjectEmployees
            .Where(pe => pe.EmployeeId == employeeId && pe.IsActive)
            .Include(pe => pe.Project)
            .AsNoTracking()
            .Select(pe => new ProjectDto
            {
                Id = pe.Project.Id,
                Name = pe.Project.Name,
                StartDate = pe.Project.StartDate,
                EndDate = pe.Project.EndDate
            })
            .ToListAsync();
    }
}
