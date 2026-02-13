using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using FinanceManagement.Domain.Common;
using FinanceManagement.Domain.Enums;

namespace FinanceManagement.Domain.Entities;

public class User : BaseEntity
{
    public string? UserName { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public string? EmergencyMobileNumber { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;

    //Navigation Properties
    public Partner? Partner { get; set; }
    public Employee? Employee { get; set; }
    public Profile? Profile {get; set; }
}