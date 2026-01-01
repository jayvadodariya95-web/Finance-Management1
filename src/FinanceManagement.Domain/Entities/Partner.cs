using FinanceManagement.Domain.Common;

namespace FinanceManagement.Domain.Entities;

public class Partner : BaseEntity
{
    public int UserId { get; set; }
    public string PartnershipType { get; set; } = string.Empty;
    public decimal SharePercentage { get; set; }
    public int? BranchId { get; set; }
    public bool IsMainPartner { get; set; }
    
    public User User { get; set; } = null!;
    public Branch? Branch { get; set; }
    public ICollection<Project> ManagedProjects { get; set; } = new List<Project>();
    public ICollection<Settlement> Settlements { get; set; } = new List<Settlement>();
}