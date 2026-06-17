using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeSalaryController : ControllerBase
    {
        private readonly IEmployeeSalaryService _service;

        public EmployeeSalaryController(IEmployeeSalaryService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAvailableEmployees()
        {
            var employees= await _service.GetAvailableEmployeesAsync();

            return Ok(employees);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeSalaryDto dto)
        {
            var result = await _service.AddAsync(dto);
            return Ok(result);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsert(List<CreateEmployeeSalaryDto> dtos)
        {
            var result= await _service.AddBulkAsync(dtos);
            return Ok(result);
        }
    }
}
