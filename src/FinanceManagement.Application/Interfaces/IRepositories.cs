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
    Task<IEnumerable<Partner>> GetAllAsync();
    Task<IEnumerable<Partner>> GetMainPartnersAsync();
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

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(int id);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee> CreateAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task<IEnumerable<Project>> GetEmployeeProjectsAsync(int employeeId);
}

public interface IBankTransactionRepository
{
    Task<BankTransaction?> GetByIdAsync(int id);
    Task<IEnumerable<BankTransaction>> GetAllAsync();
    Task<IEnumerable<BankTransaction>> GetByProjectAsync(int projectId);
    Task<IEnumerable<BankTransaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<BankTransaction> CreateAsync(BankTransaction transaction);
    Task<BankTransaction> UpdateAsync(BankTransaction transaction);
}

public interface IFinancialRepository
{
    Task<decimal> GetTotalIncomeAsync(int month, int year);
    Task<decimal> GetTotalExpensesAsync(int month, int year);
    Task<decimal> GetTotalSalariesAsync(int month, int year);
    Task<IEnumerable<MonthlyExpense>> GetMonthlyExpensesAsync(int month, int year);
    Task<decimal> GetPartnerIncomeAsync(int partnerId, int month, int year);
}

// BUG: Missing proper async patterns in some methods
// BUG: No cancellation token support
// PERFORMANCE ISSUE: No pagination support for large datasets