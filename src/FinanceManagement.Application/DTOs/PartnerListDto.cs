namespace FinanceManagement.Application.DTOs;

public class PartnerListDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsMainPartner { get; set; }
    public int ProjectCount { get; set; }
    public decimal SharePercentage { get; set; }
    public UserDto? User { get; set; }
}