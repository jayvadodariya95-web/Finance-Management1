using FinanceManagement.Application.DTOs;

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