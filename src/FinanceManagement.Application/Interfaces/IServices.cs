using FinanceManagement.Application.DTOs;

namespace FinanceManagement.Application.Interfaces;

public interface IAuthService
{
    // ✅ Matches AuthService.LoginAsync
    Task<LoginResponseDto?> LoginAsync(string email, string password);

    // ✅ FIX: Now takes 2 parameters (Access Token + Refresh Token)
    // This matches the AuthService.RefreshTokenAsync implementation.
    Task<LoginResponseDto?> RefreshTokenAsync(string accessToken, string refreshToken);

    // ✅ Keeps the validation method
    Task<bool> ValidateTokenAsync(string token);
}

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto> CreateAsync(CreateUserDto dto);
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
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
    Task<ProjectDto?> GetProjectByIdAsync(int id);
    Task<ProjectDto> CreateProjectAsync(ProjectDto project);
    Task AssignEmployeeToProjectAsync(int projectId, int employeeId);
    Task<IEnumerable<ProjectDto>> GetProjectsByPartnerAsync(int partnerId);
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