using Microsoft.AspNetCore.Mvc;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Common;

namespace FinanceManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeeDto>>>> GetAll(
            [FromQuery] string? search = null)
        {
            var employees = await _employeeRepository.GetAllAsync(search);

            return Ok(ApiResponse<IEnumerable<EmployeeDto>>.SuccessResult(employees, "Employees retrieved successfully"));
        }
       
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<EmployeeDto>>> GetById(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return NotFound(ApiResponse<EmployeeDto>.ErrorResult("Employee not found"));

            return Ok(ApiResponse<EmployeeDto>.SuccessResult(employee, "Employee retrieved successfully"));
        }
      
        [HttpPost]
        public async Task<ActionResult<ApiResponse<EmployeeDto>>> Create([FromBody] CreateEmployeeDto request)
        {
            var employee = await _employeeRepository.CreateAsync(request);
            return Ok(ApiResponse<EmployeeDto>.SuccessResult(employee, "Employee created successfully"));
        }

        // ✅ PUT: api/employee/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse<EmployeeDto>>> Update(int id, [FromBody] UpdateEmployeeDto request)
        {
            var employee = await _employeeRepository.UpdateAsync(id, request);
            return Ok(ApiResponse<EmployeeDto>.SuccessResult(employee, "Employee updated successfully"));
        }

        // ✅ DELETE: api/employee/{id}
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            await _employeeRepository.DeleteAsync(id);
            return Ok(ApiResponse<object>.SuccessResult( "Employee deleted successfully"));
        }

        // ✅ GET: api/employee/{id}/projects
        [HttpGet("{id:int}/projects")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProjectDto>>>> GetEmployeeProjects(int id)
        {
            var projects = await _employeeRepository.GetEmployeeProjectsAsync(id);
            return Ok(ApiResponse<IEnumerable<ProjectDto>>.SuccessResult(projects, "Employee projects retrieved successfully"));
        }
    }
}
