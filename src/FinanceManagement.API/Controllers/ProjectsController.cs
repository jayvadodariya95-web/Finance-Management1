using Microsoft.AspNetCore.Mvc;
using FinanceManagement.Application.Common;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Application.DTOs;

namespace FinanceManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjects()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        if (projects == null)
            return Content("No content");
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetProject(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> CreateProject([FromBody] CreateProjectDto request)
    {
        var result = await _projectService.CreateProjectAsync(request);

        if (result == null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProjectDto>> UpdateProject(int id, [FromBody] UpdateProjectDto request)
    {
        var project = await _projectService.UpdateProjectAsync(id, request);
        return Ok(project);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> DeleteProject(int id)
    {
        var result = await _projectService.DeleteProjectAsync(id);
        return Ok(result);
    }

    [HttpPost("assign-employee")]
    public async Task<ActionResult<ApiResponse<ProjectEmployeeDto>>> AssignEmployee([FromBody] AssignEmployeeDto request)
    {
        var result = await _projectService.AssignEmployeeToProjectAsync(request);
        return Ok(ApiResponse<ProjectEmployeeDto>.SuccessResult(result, "Employee assigned successfully"));
    }

    [HttpPost("unassign-employee")]
    public async Task<ActionResult<ApiResponse<string>>> UnassignEmployee(int projectId, int employeeId)
    {
        var result = await _projectService.UnassignEmployeeFromProjectAsync(projectId, employeeId);

        if (!result)
            return NotFound(ApiResponse<string>.ErrorResult("Assignment not found"));

        return Ok(ApiResponse<string>.SuccessResult("Employee unassigned successfully"));
    }

    [HttpGet("{projectId}/employees")]
    public async Task<ActionResult<ProjectEmployeeDto>> GetProjectEmployees(int projectId)
    {
        var employees = await _projectService.GetProjectEmployeesAsync(projectId);
        return Ok(employees);
    }

    [HttpGet("partner/{partnerId}")]
    public async Task<ActionResult<ProjectDto>> GetProjectsByPartner(int partnerId)
    {
        var projects = await _projectService.GetProjectsByPartnerAsync(partnerId);
        return Ok(projects);
    }
}