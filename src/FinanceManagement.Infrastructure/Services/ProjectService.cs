using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepo;
        private readonly IProjectEmployeeRepository _projectEmployeeRepo;
        private readonly IEmployeeRepository _employeeRepo;

        public ProjectService(
            IProjectRepository projectRepo,
            IProjectEmployeeRepository projectEmployeeRepo,
            IEmployeeRepository employeeRepo)
        {
            _projectRepo = projectRepo;
            _projectEmployeeRepo = projectEmployeeRepo;
            _employeeRepo = employeeRepo;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            var projects = await _projectRepo.GetAllAsync();

            return projects.Select( p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                ClientName = p.ClientName,
                ProjectValue = p.ProjectValue,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Status = p.Status.ToString(),
                ClientManagerContact = p.ClientManagerContact,
                ClientManagerEmail = p.ClientManagerEmail,
                ClientManagerName   = p.ClientManagerName,
                Description = p.Description,
                InterviewingUserId = p.InterviewingUserId,
                IsSmooth = p.IsSmooth,
                IsToolUsed = (bool)p.IsToolUsed,
                LeaveApplyWay = p.LeaveApplyWay,
                ManagedByPartnerId = p.ManagedByPartnerId,
                ProfileId = p.ProfileId,
                ManagerContact = p.ManagerContact,
                ManagerEmail = p.ManagerEmail,
                ManagerName = p.ManagerName,
                MobileNumberUsed = p.MobileNumberUsed,
                TechnologyStack = p.TechnologyStack,

                ManagedByPartner = p.ManagedByPartner != null
                    ? p.ManagedByPartner.User.FirstName + " " + p.ManagedByPartner.User.LastName
                    : null,

                Employees = p.ProjectEmployees != null
                    ? p.ProjectEmployees.Select(pe => new ProjectEmployeeDto
                    {
                        Id = pe.Id,
                        EmployeeId = pe.Employee.Id,
                        ProjectId = pe.ProjectId,
                        EmployeeName = pe.Employee.User.FirstName + " " + pe.Employee.User.LastName,
                        AssignedDate = pe.AssignedDate,
                        UnassignedDate = pe.UnassignedDate,
                        HourlyRate = pe.HourlyRate,
                        Role = pe.Role,
                        IsBench = pe.IsBench,
                        IsActive = pe.IsActive
                       
                        
                        
                    }).ToList()
                    : new List<ProjectEmployeeDto>()

            });
        }


        public async Task<ProjectDto?> GetProjectByIdAsync(int id)
        {
            var p = await _projectRepo.GetByIdAsync(id);

            if (p == null) return null;

            return new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                ClientName = p.ClientName,
                ProjectValue = p.ProjectValue,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Status = p.Status.ToString(),
                ClientManagerContact = p.ClientManagerContact,
                ClientManagerEmail = p.ClientManagerEmail,
                ClientManagerName = p.ClientManagerName,
                Description = p.Description,
                InterviewingUserId = p.InterviewingUserId,
                IsSmooth = p.IsSmooth,
                IsToolUsed = (bool)p.IsToolUsed,
                LeaveApplyWay = p.LeaveApplyWay,
                ManagedByPartnerId = p.ManagedByPartnerId,
                ProfileId = p.ProfileId,
                ManagerContact = p.ManagerContact,
                ManagerEmail = p.ManagerEmail,
                ManagerName = p.ManagerName,
                MobileNumberUsed = p.MobileNumberUsed,
                TechnologyStack = p.TechnologyStack,

                ManagedByPartner = p.ManagedByPartner != null
                    ? p.ManagedByPartner.User.FirstName + " " + p.ManagedByPartner.User.LastName
                    : null,

                Employees = p.ProjectEmployees != null
                    ? p.ProjectEmployees.Select(pe => new ProjectEmployeeDto
                    {
                        Id = pe.Id,
                        EmployeeId = pe.Employee.Id,
                        ProjectId = pe.ProjectId,
                        EmployeeName = pe.Employee.User.FirstName + " " + pe.Employee.User.LastName,
                        AssignedDate = pe.AssignedDate,
                        UnassignedDate = pe.UnassignedDate,
                        HourlyRate = pe.HourlyRate,
                        Role = pe.Role,
                        IsBench = pe.IsBench,
                        IsActive = pe.IsActive



                    }).ToList()
                    : new List<ProjectEmployeeDto>()
            };
        }


        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto)
        {
            var entity = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                ClientName = dto.ClientName,
                ProjectValue = dto.ProjectValue,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = Enum.Parse<ProjectStatus>(dto.Status),
                ManagedByPartnerId = dto.ManagedByPartnerId,
                ProfileId = dto.ProfileId,
                TechnologyStack = dto.TechnologyStack,
                ManagerName = dto.ManagerName,
                ManagerEmail = dto.ManagerEmail,
                ManagerContact = dto.ManagerContact,
                LeaveApplyWay = dto.LeaveApplyWay,
                ClientManagerName = dto.ClientManagerName,
                ClientManagerEmail = dto.ClientManagerEmail,
                ClientManagerContact = dto.ClientManagerContact,
                IsSmooth = dto.IsSmooth,
                IsToolUsed = dto.IsToolUsed,
                MobileNumberUsed = dto.MobileNumberUsed,
                InterviewingUserId = dto.InterviewingUserId,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _projectRepo.CreateAsync(entity);

            return new ProjectDto
            {
                Id = result.Id,
                Name = result.Name,
                ClientName = result.ClientName,
                ProjectValue = result.ProjectValue,
                Status = result.Status.ToString()
            };
        }

        // ✅ 4. Update Project
        public async Task<ProjectDto> UpdateProjectAsync(int id, UpdateProjectDto dto)
        {
            var existing = await _projectRepo.GetByIdAsync(id);

            if (existing == null)
                throw new Exception("Project not found");

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.ClientName = dto.ClientName;
            existing.ProjectValue = dto.ProjectValue;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;
            existing.Status = Enum.Parse<ProjectStatus>(dto.Status);
            existing.ManagedByPartnerId = dto.ManagedByPartnerId;
            existing.ProfileId = dto.ProfileId;
            existing.TechnologyStack = dto.TechnologyStack;
            existing.ManagerName = dto.ManagerName;
            existing.ManagerEmail = dto.ManagerEmail;
            existing.ManagerContact = dto.ManagerContact;
            existing.LeaveApplyWay = dto.LeaveApplyWay;
            existing.ClientManagerName = dto.ClientManagerName;
            existing.ClientManagerEmail = dto.ClientManagerEmail;
            existing.ClientManagerContact = dto.ClientManagerContact;
            existing.IsSmooth = dto.IsSmooth;
            existing.IsToolUsed = dto.IsToolUsed;
            existing.MobileNumberUsed = dto.MobileNumberUsed;
            existing.InterviewingUserId = dto.InterviewingUserId;
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _projectRepo.UpdateAsync(existing);

            return new ProjectDto
            {
                Id = updated.Id,
                Name = updated.Name,
                ClientName = updated.ClientName,
                Status = updated.Status.ToString()
            };
        }


        public async Task<bool> DeleteProjectAsync(int id)
        {
            return await _projectRepo.DeleteAsync(id);
        }


        public async Task<ProjectEmployeeDto> AssignEmployeeToProjectAsync(AssignEmployeeDto dto)
        {
            // validation
            var project = await _projectRepo.GetByIdAsync(dto.ProjectId);
            var employee = await _employeeRepo.GetByIdAsync(dto.EmployeeId);

            if (project == null)
                throw new Exception("Project not found");

            if (employee == null)
                throw new Exception("Employee not found");

            var exists = await _projectEmployeeRepo
                .IsActiveAssignmentExists(dto.ProjectId, dto.EmployeeId);

            if (exists)
                throw new Exception("Employee already assigned");

            var entity = new ProjectEmployee
            {
                ProjectId = dto.ProjectId,
                EmployeeId = dto.EmployeeId,
                Role = dto.Role,
                HourlyRate = dto.HourlyRate,
                IsBench = dto.IsBench,
                AssignedDate = DateTime.UtcNow,
                IsActive = true
            };

            var result = await _projectEmployeeRepo.AddAsync(entity);

            return new ProjectEmployeeDto
            {
                Id = result.Id,
                ProjectId = result.ProjectId,
                EmployeeId = result.EmployeeId,
                Role = result.Role,
                HourlyRate = result.HourlyRate,
                AssignedDate = result.AssignedDate,
                IsActive = result.IsActive,
                IsBench = result.IsBench
            };
        }


        public async Task<bool> UnassignEmployeeFromProjectAsync(int projectId, int employeeId)
        {
            return await _projectEmployeeRepo.UnassignAsync(projectId, employeeId);
        }


        public async Task<IEnumerable<ProjectEmployeeDto>> GetProjectEmployeesAsync(int projectId)
        {
            var list = await _projectEmployeeRepo.GetByProjectIdAsync(projectId);

            return list.Select(x => new ProjectEmployeeDto
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                EmployeeId = x.EmployeeId,
                EmployeeName = x.Employee?.User != null
                    ? $"{x.Employee.User.FirstName} {x.Employee.User.LastName}"
                    : "N/A",
                Role = x.Role,
                HourlyRate = x.HourlyRate,
                AssignedDate = x.AssignedDate,
                UnassignedDate = x.UnassignedDate,
                IsActive = x.IsActive,
                IsBench = x.IsBench
            });
        }

        public async Task<IEnumerable<ProjectDto>> GetProjectsByPartnerAsync(int partnerId)
        {
            var projects = await _projectRepo.GetByPartnerAsync(partnerId);

            return projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                ClientName = p.ClientName,
                Status = p.Status.ToString()
            });
        }
    }
}
