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
    public List<EmployeeDto> AssignedEmployees { get; set; } = new();
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