using static System.Net.Mime.MediaTypeNames;

namespace FinanceManagement.Application.DTOs;

public class ProjectListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PartnerName { get; set; } = string.Empty;
    public int EmployeeCount { get; set; }
    public bool IsActive { get; set; }
}