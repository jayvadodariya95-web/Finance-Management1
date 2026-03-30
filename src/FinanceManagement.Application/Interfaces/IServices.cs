using FinanceManagement.Application.Common;
using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Helpers;
using FinanceManagement.Domain.Entities;

namespace FinanceManagement.Application.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(string email, string password);
    Task<string> RefreshTokenAsync(string token);
    Task<bool> ValidateTokenAsync(string token);
}

public interface IFinancialService
{
    Task<MonthlyReportDto> GenerateMonthlyReportAsync(int month, int year);
    Task<decimal> CalculateNetIncomeAsync(int month, int year);
    Task<IEnumerable<PartnerIncomeDto>> CalculatePartnerIncomesAsync(int month, int year);
    Task ProcessSettlementsAsync(int month, int year);
    Task<decimal> CalculatePartnerSettlementAsync(int partnerId, int month, int year);
}

public interface IProjectService
{
    Task<ApiResponse<IEnumerable<ProjectDto>>> GetAllProjectsAsync();
    Task<ApiResponse<ProjectDto?>> GetProjectByIdAsync(int id);
    Task<ApiResponse<ProjectDto>> CreateProjectAsync(CreateProjectDto project);
    Task<ApiResponse<ProjectDto>> UpdateProjectAsync(int id, UpdateProjectDto project);
    Task<ApiResponse<bool>> DeleteProjectAsync(int id);
    Task<ProjectEmployeeDto> AssignEmployeeToProjectAsync(AssignEmployeeDto dto);
    Task<bool> UnassignEmployeeFromProjectAsync(int projectId, int employeeId);
    Task<IEnumerable<ProjectEmployeeDto>> GetProjectEmployeesAsync(int projectId);
    Task<ApiResponse<IEnumerable<ProjectDto>>> GetProjectsByPartnerAsync(int partnerId);
}

public interface INotificationService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task NotifySettlementCompletedAsync(int partnerId, decimal amount);
    Task NotifyPaymentReceivedAsync(int projectId, decimal amount);
}

// BUG: Missing proper error handling interfaces
// BUG: No logging interfaces defined
// BUG: Service methods lack proper validation
public interface IUserServices
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<PagedResult<User>> GetAllAsync(PaginationParams paginationParams);
    Task<User> CreateAsync(User user);
    Task<UpdateUserDTO> UpdateAsync(UpdateUserDTO updateUser, int id);
    Task<User> PatchAsync(PatchUserDTO patchUser, int id);
    Task DeleteAsync(int id);
    //Task<User> CreatePartnerAsync(Partner partner);
}


public interface IPartnerServices
{
    Task<Partner?> GetByIdAsync(int id);
    Task<IEnumerable<Partner>> GetAllAsync();
    Task<IEnumerable<Partner>> GetMainPartnersAsync();
    Task<Partner> CreateAsync(PartnerDto partner);
    Task<Partner> UpdateAsync(Partner partner,int id);
    Task<Partner> PatchAsync (int id, UpdatedPartnerDTO partner);
    Task<IEnumerable<Project>> GetPartnerProjectsAsync(int partnerId);
    Task<Partner?> GetByUserID(int userId);
}

public interface IEmployeeServices
{
    Task<Employee?> GetByIdAsync(int id);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee> CreateAsync(EmployeeDto employee);
    Task<Employee> UpdateAsync(int id,Employee employee);
    Task<IEnumerable<Project>> GetEmployeeProjectsAsync(int employeeId);
    Task<Employee?> GetEmployeeByUserIdAsync(int userId);

}

public interface IAssetService
{
    Task<IEnumerable<Asset>> GetAllAssetsAsync();
    Task<Asset?> GetAssetByIdAsync(int id);
    Task<Asset> CreateAssetAsync(CreateAssetDto dto);
    Task<Asset> UpdateAssetAsync(int id, Asset asset);
    Task<bool> DeleteAssetAsync(int id);
}
public interface IDocTypeService
{
    Task<IEnumerable<DocType>> GetAllAsync();
    Task<DocType?> GetByIdAsync(int id);
    Task<DocType> CreateAsync(CreateDocTypeDto docType);
    Task<DocType?> UpdateAsync(int id, DocType docType);
    Task<bool> DeleteAsync(int id);
    Task<DocType?> PatchAsync(int id, string? typeName);
}
public interface IExpenseService
{
    Task<IEnumerable<ExpenseDto>> GetAllAsync();
    Task<ExpenseDto?> GetByIdAsync(int id);
    Task<ExpenseDto> CreateAsync(CreateMonthlyExpenseDto dto);
    Task<ExpenseDto?> UpdateAsync(int id, UpdateMonthlyExpenseDto dto);
    Task<bool> DeleteAsync(int id);
    Task<ExpenseDto?> PatchAsync(int id, PatchMonthlyExpenseDto dto);
    Task<IEnumerable<ExpenseDto>> GetByMonthYearAsync(int month, int year);
    Task<ExpenseDto?> ApproveAsync(int id, string approvedBy);
}

public interface IRevenueService
{
    Task<Revenue> CreateAsync(RevenueDTO revenue);
    Task<Revenue?> UpdateAsync(int id, RevenueDTO revenue);
    Task<IEnumerable<Revenue>> GetAllAsync();
    Task<Revenue?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
    Task<Revenue?> PatchAsync(int id, PatchRevenueDTO dto);
}

public interface IEmployeeDocumentService
{
   Task UploadAsync(UploadEmployeeDocumentDto dto);
}
public interface IProfileService
{
    Task<ApiResponse<IEnumerable<Profile>>> GetAllAsync();
    Task<ApiResponse<ProfileDto>> GetByIdAsync(int id);
    Task<ApiResponse<ProfileDto>> CreateAsync(CreateProfileDto dto);
    Task<ApiResponse<ProfileDto>> UpdateAsync(int id, UpdateProfileDto dto);
    Task<ApiResponse<bool>> DeleteAsync(int id);
}
public interface ICategoryService
{
    public Task<ApiResponse<IEnumerable<Category>>> GetAllAsync();
    public Task<ApiResponse<Category?>> GetByIdAsync(int id);
    public Task<ApiResponse<CategoryDto>> CreateAsync(CategoryDto category);
    public Task<ApiResponse<CategoryDto>> UpdateAsync(int id, CategoryDto category);
    public Task<bool> DeleteAsync(int id);

}