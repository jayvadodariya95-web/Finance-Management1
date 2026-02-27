using FinanceManagement.Domain.Enums;

namespace FinanceManagement.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class CreateUserDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int Role { get; set; }
}

public class PartnerDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PartnershipType { get; set; } = string.Empty;
    public decimal SharePercentage { get; set; }
    public bool IsMainPartner { get; set; }
    public string? BranchName { get; set; }
}

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public decimal ProjectValue { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ManagedByPartner { get; set; } = string.Empty;
    public string? TechnologyStack { get; set; }
    public string? ManagerName { get; set; }
    public string? ManagerEmail { get; set; }
    public string? ManagerContact { get; set; }
    public string? ClientManagerName { get; set; }
    public string? LeaveApplyWay { get; set; }
    public string? ClientManagerEmail { get; set; }
    public string? ClientManagerContact { get; set; }
    public bool IsSmooth { get; set; }
    public string? MobileNumberUsed { get; set; }
    public bool? IsToolUsed { get; set; }
    public string? ProfileName { get; set; }
    public string? InterviewingUserName { get; set; }

}
public class CreateProjectDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public decimal ProjectValue { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int ManagedByPartnerId { get; set; }
    public int? ProfileId { get; set; }
    public string? TechnologyStack { get; set; }
    public string? ManagerName { get; set; }
    public string? ManagerEmail { get; set; }
    public string? ManagerContact { get; set; }
    public string? LeaveApplyWay { get; set; }
    public string? ClientManagerName { get; set; }
    public string? ClientManagerEmail { get; set; }
    public string? ClientManagerContact { get; set; }
    public bool IsSmooth { get; set; }
    public string? MobileNumberUsed { get; set; }
    public int? InterviewingUserId { get; set; }
    public bool? IsToolUsed { get; set; }
}
public class EmployeeDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmployeeCode { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public decimal MonthlySalary { get; set; }
    public DateTime JoinDate { get; set; }
    public bool IsActive { get; set; }
}

// BUG: Missing validation attributes
// BUG: Exposing sensitive data like salary in basic DTO
// BUG: No proper data annotations for required fields