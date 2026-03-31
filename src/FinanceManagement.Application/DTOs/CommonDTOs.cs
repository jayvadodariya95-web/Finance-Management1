using FinanceManagement.Domain.Enums;
using Microsoft.AspNetCore.Http;

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
    //public int Id { get; set; }
    //public string FirstName { get; set; } = string.Empty;
    //public string LastName { get; set; } = string.Empty;
    //public string Email { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string PartnershipType { get; set; } = string.Empty;
    public decimal SharePercentage { get; set; }
    public bool IsMainPartner { get; set; }
    //public int  BranchId { get; set; }
}


    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public decimal ProjectValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int? ManagedByPartnerId { get; set; }
        public int? ProfileId { get; set; }
        public string? TechnologyStack { get; set; }
        public string? ManagerName { get; set; }
        public string? ManagerEmail { get; set; }
        public string? ManagerContact { get; set; }
        public string? ClientManagerName { get; set; }
        public string? ClientManagerEmail { get; set; }
        public string? ClientManagerContact { get; set; }
        public string? LeaveApplyWay { get; set; }
        public bool IsSmooth { get; set; }
        public bool IsToolUsed { get; set; }
        public string? MobileNumberUsed { get; set; }
        public int? InterviewingUserId { get; set; }
        public List<ProjectEmployeeDto>? Employees { get; set; }
        public List<UserDto>? Manager { get; set; }
        public string ManagedByPartner { get; set; }
}


public class EmployeeDto
{
    public int UserId { get; set; }
    public int? BranchId { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    
    public string Position { get; set; } = string.Empty;
    public int MonthlySalary { get; set; }
    public int? PreviousCTC { get; set; }
    public int CurrentCTC { get; set; }
    public DateTime JoinDate { get; set; }
    public DateTime? RelievingDate { get; set; }
    public int? TakenLeave { get; set; }
}

public class UpdateUserDTO
{
    public string? Username { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? MobileNumber { get; set; }
    public string? EmergencyMobileNumber { get; set; }
    public UserGender Gender { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;
}
public class PatchUserDTO
{
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int? Role { get; set; }
    public int? Gender { get; set; }
    public string? MobileNumber { get; set; }
    public string? EmergencyMobileNumber { get; set; }
}

public class DocTypeDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class CreateAssetDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime PurchaseDate { get; set; }
}

public class UpdateAssetDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Purchase_Date { get; set; }
}

public class PatchAssetDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? Purchase_Date { get; set; }
}
public class CreateDocTypeDto
{
    public string? TypeName { get; set; }
}
public class UpdateDocTypeDto
{
    public string? TypeName { get; set; }
}


public class CreateMonthlyExpenseDto
{
    public int? AssetId { get; set; }
    public int PartnerId { get; set; }
    public int? EmployeeId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int CategoryId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public bool IsRecurring { get; set; }
}

public class UpdateMonthlyExpenseDto
{
    public int? AssetId { get; set; }
    public int PartnerId { get; set; }
    public int? EmployeeId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int CategoryId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public bool IsRecurring { get; set; }
}
public class PatchMonthlyExpenseDto
{
    public int? AssetId { get; set; }
    public int? PartnerId { get; set; }
    public int? EmployeeId { get; set; }
    public string? Description { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public int CategoryId { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
    public bool? IsRecurring { get; set; }
}
// BUG: Missing validation attributes
// BUG: Exposing sensitive data like salary in basic DTO
// BUG: No proper data annotations for required fields

public class CreateProjectDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public decimal ProjectValue { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
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
    public bool IsToolUsed { get; set; }
    public string? MobileNumberUsed { get; set; }
    public int? InterviewingUserId { get; set; }
    //Assign employees during creation
    public List<int>? ProjectEmployeeIds { get; set; }
}

public class ProjectEmployeeDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string? Role { get; set; }
    public decimal? HourlyRate { get; set; }
    public DateTime AssignedDate { get; set; }
    public DateTime? UnassignedDate { get; set; }
    public bool IsActive { get; set; }
    public bool? IsBench { get; set; }
}
public class UpdateProjectDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string ClientName { get; set; } = string.Empty;
    public decimal ProjectValue { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string Status { get; set; } = string.Empty;

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
    public bool IsToolUsed { get; set; }

    public string? MobileNumberUsed { get; set; }

    public int? InterviewingUserId { get; set; }
}

public class AssignEmployeeDto
{
    public int ProjectId { get; set; }
    public int EmployeeId { get; set; }

    public string? Role { get; set; }
    public decimal? HourlyRate { get; set; }

    public bool IsBench { get; set; } = false;

}

public class RevenueDTO
{
    public int PartnerId { get; set; }
    public int? ProjectId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public bool? Revenue_From { get; set; } = true;
    public string? Notes { get; set; }
}
public class PatchRevenueDTO
{
    public int? PartnerId { get; set; }
    public int? ProjectId { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? Date { get; set; }
    public bool? Revenue_From { get; set; }
    public string? Notes { get; set; }
}
public class UploadEmployeeDocumentDto
{
    public int EmployeeId { get; set; }
    public int DocType_Id { get; set; }
    public IFormFile File { get; set; }
}



public class ProfileDto
{
    public int UserId { get; set; }
    public bool IsPaid { get; set; }
    public int? Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateProfileDto
{
    public int UserId { get; set; }
    public bool IsPaid { get; set; }
    public int? Amount { get; set; }
}

public class UpdateProfileDto
{
    public int UserId { get; set; }
    public bool IsPaid { get; set; }
    public int? Amount { get; set; }
}
public class CategoryDto
{
    public string? CategoryName { get; set; }
    public bool IsRecurring { get; set; }

}
