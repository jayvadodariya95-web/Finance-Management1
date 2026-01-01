using FinanceManagement.Domain.Common;

namespace FinanceManagement.Domain.Entities;

public class Branch : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<Partner> Partners { get; set; } = new List<Partner>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}