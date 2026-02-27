using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FinanceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeServices services;

        public EmployeeController(IEmployeeServices services)
        {
            this.services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployee()
        {
            var data = await services.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllEmployeeById(int id)
        {
            var data = await services.GetByIdAsync(id);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeCreateDto employeeCreateDto)
        {
            var data = await services.CreateAsync(employeeCreateDto);
            return Ok(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeUpdateDto employeeUpdateDto, int id)
        {
            var data = await services.UpdateAsync(employeeUpdateDto, id);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await services.DeleteAsync(id);
            return Ok(data);
        }
    }
}
