using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinanceManagement.Application.Common;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Application.DTOs;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Domain.Enums;

namespace FinanceManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;
    private readonly IPartnerRepository _partnerRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(
        IProjectRepository projectRepository,
        IPartnerRepository partnerRepository,
        IEmployeeRepository employeeRepository,
        ILogger<ProjectsController> logger)
    {
        _projectRepository = projectRepository;
        _partnerRepository = partnerRepository;
        _employeeRepository = employeeRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProjectDto>>>> GetAllProjects()
    {
        try
        {
            var projects = await _projectRepository.GetAllAsync();

            var projectDtos = projects.Select(project =>
            {
                var partner = project.ManagedByPartner;

                return new ProjectDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    ClientName = project.ClientName,
                    ProjectValue = project.ProjectValue,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    Status = project.Status.ToString(),
                    ManagedByPartner = partner?.User != null
                        ? $"{partner.User.FirstName} {partner.User.LastName}"
                        : "Unassigned Partner"
                };
            });

            return Ok(ApiResponse<IEnumerable<ProjectDto>>.SuccessResult(projectDtos));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving projects");
            return StatusCode(500,
                ApiResponse<IEnumerable<ProjectDto>>.ErrorResult("Failed to retrieve projects"));
        }
    }

    //[Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> GetProject(int id)
    {
        try
        {
            var project = await _projectRepository.GetByIdAsync(id);
            
            if (project == null)
            {
                return NotFound(ApiResponse<ProjectDto>.ErrorResult("Project not found"));
            }

            //var partner = await _partnerRepository.GetByIdAsync(project.ManagedByPartnerId);
            var partner = project.ManagedByPartner;

            var projectDto = new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                ClientName = project.ClientName,
                ProjectValue = project.ProjectValue,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = project.Status.ToString(),
                // ManagedByPartner = $"{partner.User.FirstName} {partner?.User.LastName}"
                ManagedByPartner = partner?.User != null
                ? $"{partner.User.FirstName} {partner.User.LastName}"
                : "Unassigned Partner"

            };

            return Ok(ApiResponse<ProjectDto>.SuccessResult(projectDto));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving project {ProjectId}", id);
            return StatusCode(500, ApiResponse<ProjectDto>.ErrorResult("Failed to retrieve project"));
        }
    }

    
    [HttpPost]
    //[Authorize(Policy = "AdminOrPartner")]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> CreateProject([FromBody] CreateProjectDto request)
    {
        try
        {
            var project = new Project
            {
                Name = request.Name,
                Description = request.Description,
                ClientName = request.ClientName,
                ProjectValue = request.ProjectValue,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = ProjectStatus.Active,
                ManagedByPartnerId = request.ManagedByPartnerId
            };

            var createdProject = await _projectRepository.CreateAsync(project);
            var fullProject = await _projectRepository.GetByIdAsync(createdProject.Id);
            var partner = fullProject?.ManagedByPartner;

            var projectDto = new ProjectDto
            {
                Id = createdProject.Id,
                Name = createdProject.Name,
                ClientName = createdProject.ClientName,
                ProjectValue = createdProject.ProjectValue,
                StartDate = createdProject.StartDate,
                EndDate = createdProject.EndDate,
                Status = createdProject.Status.ToString(),
                ManagedByPartner = partner?.User != null
                ? $"{partner.User.FirstName} {partner.User.LastName}"
                : "Unassigned Partner"

            };

            return CreatedAtAction(nameof(GetProject), new { id = createdProject.Id }, 
                ApiResponse<ProjectDto>.SuccessResult(projectDto, "Project created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project");
            return StatusCode(500, ApiResponse<ProjectDto>.ErrorResult("Failed to create project"));
        }
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("{id}/assign-employee")]
    public async Task<ActionResult<ApiResponse<string>>> AssignEmployee(int id, [FromBody] AssignEmployeeDto request)
    {
        try
        {
            await _projectRepository.AssignEmployeeAsync(id, request.EmployeeId, request.Role);
            return Ok(ApiResponse<string>.SuccessResult("Employee assigned successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning employee to project");
            return StatusCode(500, ApiResponse<string>.ErrorResult("Failed to assign employee"));
        }
    }

    [Authorize(Policy = "AdminOrPartner")]
    [HttpGet("partner/{partnerId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProjectDto>>>> GetProjectsByPartner(int partnerId)
    {
        try
        {
            var projects = await _projectRepository.GetByPartnerAsync(partnerId);
            
            var projectDtos = projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                ClientName = p.ClientName,
                ProjectValue = p.ProjectValue,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Status = p.Status.ToString(),
                ManagedByPartner = "Current Partner"
            });

            return Ok(ApiResponse<IEnumerable<ProjectDto>>.SuccessResult(projectDtos));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving projects for partner {PartnerId}", partnerId);
            return StatusCode(500, ApiResponse<IEnumerable<ProjectDto>>.ErrorResult("Failed to retrieve partner projects"));
        }
    }
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
}

public class AssignEmployeeDto
{
    public int EmployeeId { get; set; }
    public string? Role { get; set; }
}


