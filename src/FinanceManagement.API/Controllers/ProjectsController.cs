using Microsoft.AspNetCore.Mvc;
using FinanceManagement.Application.Common;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Application.DTOs;
using FinanceManagement.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace FinanceManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService projectService;
    private readonly IProjectRepository _projectRepository;
    private readonly IPartnerRepository _partnerRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IProjectService _projectService;
    private readonly ILogger<ProjectsController> _logger;
    private ProjectDto projectDto;

    public ProjectsController( IProjectService projectService)
    {
        this._projectService = projectService;
    }
 
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> GetProjectbyid(int id)
    {
      var projectDto = await _projectService.GetProjectByIdAsync(id);
      if (projectDto == null)
       {
          return NotFound(ApiResponse<ProjectDto>.ErrorResult("Project not found"));
       }
          return Ok(ApiResponse<ProjectDto>.SuccessResult(projectDto));   
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> CreateProject([FromBody] CreateProjectDto request)
    {
      var result = await _projectService.CreateProjectAsync(request);
      return Ok(ApiResponse<ProjectDto>.SuccessResult(result, "Project created successfully"));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> UpdateProject(int id, [FromBody] CreateProjectDto request)
    {
  
        {
            var updateproject = await _projectService.UpdateProjectAsync(id, request);
            if (updateproject == null)
            {
                return NotFound(ApiResponse<ProjectDto>.ErrorResult("Record Not Found On this Id"));
            }
            return Ok(ApiResponse<ProjectDto>.SuccessResult(updateproject, "project update successfully"));
        }
      
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> DeleteProject(int id)
    { 
            var deleteproject = await _projectService.DeleteProjectAsync(id);
            if(!deleteproject)
            {
                return NotFound(ApiResponse<ProjectDto>.ErrorResult("Project Not Found "));
            }
            return Ok(ApiResponse<ProjectDto>.SuccessResult("project is deleted"));  
    }

}
