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
    public async Task<ActionResult<ApiResponse<IEnumerable<ProjectDto>>>> GetAllProjects()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        if (projects == null)
            return Content("No content");
        return Ok(ApiResponse<IEnumerable<ProjectDto>>.SuccessResult(projects));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> GetProject(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);

        if (project == null)
            return NotFound(ApiResponse<ProjectDto>.ErrorResult("Project not found"));

        return Ok(ApiResponse<ProjectDto>.SuccessResult(project));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> CreateProject([FromBody] CreateProjectDto request)
    {
        var project = await _projectService.CreateProjectAsync(request);

        return CreatedAtAction(nameof(GetProject), new { id = project.Id },
            ApiResponse<ProjectDto>.SuccessResult(project, "Project created successfully"));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> UpdateProject(int id, [FromBody] UpdateProjectDto request)
    {
        var project = await _projectService.UpdateProjectAsync(id, request);

        return Ok(ApiResponse<ProjectDto>.SuccessResult(project, "Project updated successfully"));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteProject(int id)
    {
        var result = await _projectService.DeleteProjectAsync(id);

        if (!result)
            return NotFound(ApiResponse<string>.ErrorResult("Project not found"));

        return Ok(ApiResponse<string>.SuccessResult("Project deleted successfully"));
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
    public async Task<ActionResult<ApiResponse<IEnumerable<ProjectEmployeeDto>>>> GetProjectEmployees(int projectId)
    {
        var employees = await _projectService.GetProjectEmployeesAsync(projectId);
        return Ok(ApiResponse<IEnumerable<ProjectEmployeeDto>>.SuccessResult(employees));
    }

    [HttpGet("partner/{partnerId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProjectDto>>>> GetProjectsByPartner(int partnerId)
    {
        var projects = await _projectService.GetProjectsByPartnerAsync(partnerId);
        return Ok(ApiResponse<IEnumerable<ProjectDto>>.SuccessResult(projects));
    }
}