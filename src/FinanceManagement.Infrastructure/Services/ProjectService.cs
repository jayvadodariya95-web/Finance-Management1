using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Domain.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FinanceManagement.Infrastructure.Services
{
   public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IPartnerRepository _partnerRepository;
    
        public ProjectService(IProjectRepository projectRepository, IPartnerRepository partnerRepository ) 
        { _projectRepository = projectRepository;
          _partnerRepository = partnerRepository;
        }
        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto)
        {
            var project = new Domain.Entities.Project
            {
                Name = dto.Name,
                Description = dto.Description,
                ClientName = dto.ClientName,
                ProjectValue = dto.ProjectValue,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                ManagedByPartnerId = dto.ManagedByPartnerId,
                ProfileId = dto.ProfileId,
                TechnologyStack = dto.TechnologyStack,
                ManagerName = dto.ManagerName,
                ManagerEmail = dto.ManagerEmail,
                ManagerContact = dto.ManagerContact,
                LeaveApplyWay = dto.LeaveApplyWay,
                ClientManagerName = dto.ClientManagerName,
                ClientManagerContact = dto.ClientManagerContact,
                ClientManagerEmail = dto.ClientManagerEmail,
                MobileNumberUsed = dto.MobileNumberUsed,
                InterviewingUserId = dto.InterviewingUserId,
                IsSmooth = dto.IsSmooth,
                IsToolUsed = dto.IsToolUsed,
                Status = ProjectStatus.Active
            };

            var createdProject = await _projectRepository.CreateAsync(project);
            var partner = await _partnerRepository.GetByIdAsync(createdProject.ManagedByPartnerId);

            return new ProjectDto
            {
                Id = createdProject.Id,
                Name = createdProject.Name,
                ClientName = createdProject.ClientName,
                ProjectValue = createdProject.ProjectValue,
                StartDate = createdProject.StartDate,
                EndDate = createdProject.EndDate,
                Status = createdProject.Status.ToString(),
                ManagedByPartner = createdProject.ManagedByPartner?.User?.FirstName ?? "N/A",
                TechnologyStack = createdProject.TechnologyStack,
                ManagerName = createdProject.ManagerName,
                ManagerContact = createdProject.ManagerContact,
                ManagerEmail = createdProject.ManagerEmail,
                LeaveApplyWay = createdProject.LeaveApplyWay,
                ClientManagerName = createdProject.ClientManagerEmail,
                ClientManagerContact = createdProject.ManagerContact,
                ClientManagerEmail = createdProject.ClientManagerEmail,
                IsSmooth = createdProject.IsSmooth,
                MobileNumberUsed = createdProject.MobileNumberUsed,
                InterviewingUserName = createdProject.InterviewingUser?.FirstName,
                IsToolUsed = createdProject.IsToolUsed
            };
        }
        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            var projects = await _projectRepository.GetAllAsync();
            return projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                ClientName = p.ClientName,
                ProjectValue = p.ProjectValue,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Status = p.Status.ToString(),
                ManagedByPartner = p.ManagedByPartner?.User?.FullName ?? "N/A",
                TechnologyStack = p.TechnologyStack,
                ManagerName = p.ManagerName,
                ManagerEmail = p.ManagerEmail,
                ManagerContact = p.ManagerContact,
                ClientManagerName = p.ClientManagerName,
                LeaveApplyWay = p.LeaveApplyWay,
                ClientManagerEmail = p.ClientManagerEmail,
                ClientManagerContact = p.ClientManagerContact,
                IsSmooth = p.IsSmooth,
                MobileNumberUsed = p.MobileNumberUsed,
                InterviewingUserName = p.InterviewingUser?.FirstName
            });
        }

        public async Task<ProjectDto?> GetProjectByIdAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null) return null;

            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                ClientName = project.ClientName,
                ProjectValue = project.ProjectValue,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = project.Status.ToString(),
                TechnologyStack = project.TechnologyStack,
                ManagerName = project.ManagerName,
                ManagerEmail = project.ManagerEmail,
                ManagerContact = project.ManagerContact,
                ClientManagerName = project.ClientManagerName,
                ClientManagerEmail =project.ClientManagerEmail,
                ClientManagerContact = project.ClientManagerContact,
                LeaveApplyWay = project.LeaveApplyWay,
                IsSmooth = project.IsSmooth,
                ManagedByPartner = project.ManagedByPartner?.User?.FullName ?? "N/A"
            };
        }
        public async Task<ProjectDto?> UpdateProjectAsync(int id, CreateProjectDto request)
        {
            var projects = await _projectRepository.GetByIdAsync(id);
            if (projects == null) return null;

            projects.Name = request.Name;
            projects.Description = request.Description;
            projects.ClientName = request.ClientName;
            projects.ProjectValue = request.ProjectValue;
            projects.StartDate = request.StartDate;
            projects.EndDate = request.EndDate;
            projects.TechnologyStack = request.TechnologyStack;
            projects.ManagerName = request.ManagerName;
            projects.ManagerEmail = request.ManagerEmail;
            projects.ManagerContact = request.ManagerContact;
            projects.ClientManagerName = request.ClientManagerName;
            projects.ClientManagerEmail = request.ClientManagerEmail;
            projects.ClientManagerContact = request.ClientManagerContact;
            projects.MobileNumberUsed = request.MobileNumberUsed;
            projects.IsToolUsed = request.IsToolUsed;
            projects.IsSmooth = request.IsSmooth;
            projects.ManagedByPartnerId = request.ManagedByPartnerId;

            await _projectRepository.UpdateAsync(projects,id);
            return await GetProjectByIdAsync(id);

        }
 
      public  async Task<bool> DeleteProjectAsync(int id)
        {
           var data = await _projectRepository.GetByIdAsync(id);
           if (data == null) return false;
           await _projectRepository.DeleteAsync(id);
           return true;  
        }
    }
}
