using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace FinanceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueController : ControllerBase
    {
        private readonly IRevenueService _revenueService;

        public RevenueController(IRevenueService revenueService)
        {
            this._revenueService = revenueService;
        }

        [HttpGet]
        public async Task <IActionResult> GetAll()
        {
            var assets = await _revenueService.GetAllAsync();
            return Ok(assets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            
            var revenue = await _revenueService.GetByIdAsync(id);
            if (revenue == null)
            {
                return NotFound("Revenue not found ");
            }

            return Ok(revenue);
        }

        [HttpPost]

        public async Task<IActionResult> CreateRevenue([FromBody] RevenueDTO revenue)
        {
                var result = await _revenueService.CreateAsync(revenue);
                return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRevenue(int id, [FromBody] RevenueDTO revenue)
        {
            var updateRevenue = await _revenueService.UpdateAsync(id, revenue);
            return Ok(updateRevenue);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRevenue(int id)
        {
            var deleteRevenue = await _revenueService.DeleteAsync(id);
            return Ok(deleteRevenue);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] PatchRevenueDTO dto)
        {
                var updated = await _revenueService.PatchAsync(id, dto);

                if (updated == null)
                    return NotFound(new { message = "Revenue not found" });

                return Ok(updated);
        }
    }
}
