using FinanceManagement.Domain.Entities;
using FinanceManagement.Application.Helpers;
using FinanceManagement.Application.DTOs;

namespace FinanceManagement.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<PagedResult<User>> GetAllAsync(PaginationParams paginationParams);
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
    //Task<Partner> PatchAsync (UpdatedPartnerDTO partner);
    Task<IEnumerable<Project>> GetPartnerProjectsAsync(int partnerId);
    Task<Partner?> GetByUserID(int userId);
}

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(int id);
    Task<IEnumerable<Project>> GetAllAsync();
    Task<IEnumerable<Project>> GetByPartnerAsync(int partnerId);
    Task<Project> CreateAsync(Project project);
    Task<Project> UpdateAsync(Project project);
    Task<ProjectEmployee> AssignEmployeeAsync(ProjectEmployee assign);
    Task<bool> DeleteAsync(int id);
    //Task<IEnumerable<Project>> GetByPartnerIdAsync(int partnerId);
    //Task<IEnumerable<Project>> GetByStatusAsync(string status);
}
public interface IProjectEmployeeRepository
{
    Task<ProjectEmployee> AddAsync(ProjectEmployee entity);
    Task<bool> IsActiveAssignmentExists(int projectId, int employeeId);
    Task<bool> UnassignAsync(int projectId, int employeeId);
    Task<IEnumerable<ProjectEmployee>> GetByProjectIdAsync(int projectId);
    Task<IEnumerable<ProjectEmployee>> GetByEmployeeIdAsync(int employeeId);
    Task<ProjectEmployee?> GetAsync(int projectId, int employeeId);
}
public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(int id);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee> CreateAsync(Employee employee);
    Task<Employee> UpdateAsync( Employee employee);
    Task<IEnumerable<Project>> GetEmployeeProjectsAsync(int employeeId);
   // Task<Employee?> GetEmployeeByUserID(int userid);
    Task<Employee?> GetEmployeeByUserIdAsync(int userId);
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
public interface IAssetRepository
{
    Task<IEnumerable<Asset>> GetAllAsync();
    Task<Asset?> GetByIdAsync(int id);
    Task<Asset> AddAsync(Asset asset);
    Task<Asset> Update(Asset asset);
    void Delete(Asset asset);
}
public interface IDocTypeRepository
{
    Task<IEnumerable<DocType>> GetAllAsync();
    Task<DocType?> GetByIdAsync(int id);
    Task<DocType> AddAsync(DocType docType);
    Task<DocType?> UpdateAsync(DocType docType);
    Task<bool> DeleteAsync(int id);
    Task<DocType?> PatchAsync(int id, string? typeName);
}
public interface IMonthlyExpenseRepository
{
    Task<IEnumerable<ExpenseDto>> GetAllAsync();
    Task<MonthlyExpense?> GetByIdAsync(int id);
    Task<MonthlyExpense> AddAsync(MonthlyExpense entity);
    Task<MonthlyExpense?> UpdateAsync(MonthlyExpense entity);
    Task<bool> DeleteAsync(int id);

    Task<IEnumerable<MonthlyExpense>> GetByMonthYearAsync(int month, int year);
}
public interface IRevenueRepository
{
    Task<Revenue> CreateAsync(Revenue revenue);
    Task<Revenue?> UpdateAsync(Revenue revenue);
    Task<IEnumerable<RevenueDTO>> GetAllAsync();
    Task<Revenue?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
}
public interface IEmployeeDocumentRepository
{
    Task <Documents>AddDocumentAsync(Documents document);
    Task <EmployeeDocument>AddEmployeeDocumentAsync(EmployeeDocument employeeDocument);
}
public interface IProfileRepository
{
    Task<IEnumerable<Profile>> GetAllAsync();
    Task<Profile?> GetByIdAsync(int id);
    Task<Profile> CreateAsync(Profile profile);
    Task<Profile> UpdateAsync(Profile profile);
    Task<bool> DeleteAsync(int id);
}
public interface ICategoryRepository
{
    public Task<IEnumerable<Category>> GetAllAsync();
    public Task<Category?> GetByIdAsync(int id);
    public Task<Category> CreateAsync(Category category);
    public Task<Category> UpdateAsync(int id, Category category);
    public Task<bool> DeleteAsync(int id);

}