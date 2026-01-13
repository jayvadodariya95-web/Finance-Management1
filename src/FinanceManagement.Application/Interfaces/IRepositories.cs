using FinanceManagement.Application.DTOs;
using FinanceManagement.Domain.Entities;

namespace FinanceManagement.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task DeleteAsync(int id);
}

public interface IPartnerRepository
{
    Task<Partner?> GetByIdAsync(int id);
    Task<IEnumerable<PartnerListDto>> GetAllAsync();
    Task<IEnumerable<PartnerListDto>> GetMainPartnersAsync();
    Task<Partner> CreateAsync(Partner partner);
    Task<Partner> UpdateAsync(Partner partner);
    Task<IEnumerable<Project>> GetPartnerProjectsAsync(int partnerId);
}

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(int id);
    Task<IEnumerable<Project>> GetAllAsync();
    Task<IEnumerable<Project>> GetByPartnerAsync(int partnerId);
    Task<Project> CreateAsync(Project project);
    Task<Project> UpdateAsync(Project project);
    Task AssignEmployeeAsync(int projectId, int employeeId, string? role = null);
}

public interface IBankTransactionRepository
{
    Task<BankTransaction?> GetByIdAsync(int id);
    Task<IEnumerable<BankTransaction>> GetAllAsync();
    Task<IEnumerable<BankTransaction>> GetByProjectAsync(int projectId);
    Task<IEnumerable<BankTransaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<BankTransaction> CreateAsync(BankTransaction transaction);
    Task<BankTransaction> UpdateAsync(BankTransaction transaction);
    Task<PagedResultDto<BankTransaction>> GetPagedAsync(int pageNumber,int pageSize, DateTime? startDate = null,
    DateTime? endDate = null);
}

public interface IFinancialRepository
{
    Task<decimal> GetTotalIncomeAsync(int month, int year);
    Task<decimal> GetTotalExpensesAsync(int month, int year);
    Task<decimal> GetTotalSalariesAsync(int month, int year);
    Task<IEnumerable<MonthlyExpense>> GetMonthlyExpensesAsync(int month, int year);
    Task<decimal> GetPartnerIncomeAsync(int partnerId, int month, int year);
}

//public interface IEmployeeRepository
//{
//    Task<Employee?> GetByIdAsync(int id);
//    Task<IEnumerable<Employee>> GetAllAsync();
//    Task<Employee> CreateAsync(Employee employee);
//    Task<Employee> UpdateAsync(Employee employee);
//    Task<IEnumerable<Project>> GetEmployeeProjectsAsync(int employeeId);
//}

public interface IEmployeeRepository
{

    Task<IEnumerable<EmployeeDto>> GetAllAsync(string? search = null);
    Task<EmployeeDto?> GetByIdAsync(int id);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto employee);
    Task<EmployeeDto> UpdateAsync(int id, UpdateEmployeeDto employee);
    Task DeleteAsync(int id);
    Task<IEnumerable<ProjectDto>> GetEmployeeProjectsAsync(int employeeId);
}


public interface IBankAccountRepository
{
    Task<IEnumerable<BankAccount>> GetAllAsync();
    Task<BankAccount?> GetByIdAsync(int id);
    Task<BankAccount> CreateAsync(BankAccount account);
    Task<BankAccount> UpdateAsync(BankAccount account);
    Task<bool> DeleteAsync(int id);
}

// BUG: Missing proper async patterns in some methods
// BUG: No cancellation token support
// PERFORMANCE ISSUE: No pagination support for large datasets