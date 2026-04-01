using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _service;

        public ExpenseController(IExpenseService service)
        {
            this._service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null)
                return NotFound();

            return Ok(data);
        }

       
        [HttpPost]
        public async Task<IActionResult> Create(CreateMonthlyExpenseDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateMonthlyExpenseDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
        [HttpPatch("{id}/approve")]
        public async Task<IActionResult> Approve(int id,int approvedBy)
        {
            var result = await _service.ApproveAsync(id, approvedBy);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, PatchMonthlyExpenseDto dto)
        {
            var result = await _service.PatchAsync(id, dto);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
