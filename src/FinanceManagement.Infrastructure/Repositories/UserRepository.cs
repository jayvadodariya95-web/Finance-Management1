using FinanceManagement.Application.Helpers;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FinanceManagement.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FinanceDbContext _context;
    private readonly IConfiguration _configuration;

    public UserRepository(FinanceDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        // PERFORMANCE ISSUE: Missing index on Email field
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    //public async Task<PagedResult<User>> GetUsersAsync(PaginationParams paginationParams)
    //{
    //    var query = _context.Users
    //        .AsNoTracking()
    //        .OrderBy(x => x.Id)
    //        .AsQueryable();

    //    return await PaginationHelpers.CreateAsync(
    //        query,
    //        paginationParams.PageNumber,
    //        paginationParams.PageSize
    //    );
    //}

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
        // Soft Deleted implemented 
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            user.IsDeleted = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }

    // Load user with pagination
    public async Task<PagedResult<User>> GetAllAsync(PaginationParams paginationParams)
    {
        // Read connection string into local variable and validate to avoid passing null.
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        }

        var users = await PaginationHelper.CreateAsync<User>(
            "GetUsersPaged",
            connectionString,
            paginationParams.PageNumber,
            paginationParams.PageSize,
            paginationParams.SearchName,
            reader => new User
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                FirstName = reader["FirstName"].ToString(),
                LastName = reader["LastName"].ToString(),
                Email = reader["Email"].ToString(),
                MobileNumber = reader["MobileNumber"].ToString(),
                Role = (Domain.Enums.UserRole)reader.GetInt32(reader.GetOrdinal("Role"))
            });

        return users;
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
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .Include(e => e.User)
            .Include(e => e.Branch)
            .Where(e => !e.IsDeleted)
            .ToListAsync();
    }

    public async Task<Employee> CreateAsync(Employee employee)
    {
        // BUG: No validation for duplicate employee codes
        await _context.Employees.AddAsync(employee);
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

    public async Task<Employee?> GetEmployeeByUserIdAsync(int userId)
    {
        return await _context.Employees
           .Include(e => e.User)
           .FirstOrDefaultAsync(e => e.UserId == userId);
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
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
    }

    public async Task<IEnumerable<Partner>> GetAllAsync()
    {
        return await _context.Partners
            .Include(p => p.User)
            .Where(e => !e.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<Partner>> GetMainPartnersAsync()
    {
        return await _context.Partners
            .Include(p => p.User)
            .Where(p => (bool)p.IsMainPartner)
            .ToListAsync();
    }

    public async Task<Partner> CreateAsync(Partner partner)
    {
        await _context.Partners.AddAsync(partner);
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

    public async Task<Partner?> GetByUserID(int userId)
    {
        return await _context.Partners
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }
}