using FinanceManagement.Application.Common;
using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FinanceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerController : ControllerBase
    {
        private readonly IPartnerService _service;

        public PartnerController(IPartnerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PartnerResponseDto>>>> GetAll()
        {
            var partners = await _service.GetAllAsync();

            return Ok(ApiResponse<IEnumerable<PartnerResponseDto>>
                .SuccessResult(partners, "Partners fetched successfully."));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PartnerResponseDto>>> GetById(int id)
        {
            var partner = await _service.GetByIdAsync(id);

            if (partner == null)
            {
                return NotFound(ApiResponse<PartnerResponseDto>
                    .ErrorResult("Partner not found."));
            }

            return Ok(ApiResponse<PartnerResponseDto>
                .SuccessResult(partner, "Partner fetched successfully."));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Create(CreatePartnerDto dto)
        {
            var id = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById),
                new { id },
                ApiResponse<int>.SuccessResult(id, "Partner created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Update(int id, UpdatePartnerDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            if (!result)
            {
                return NotFound(
                    ApiResponse<string>.ErrorResult("Partner not found or already deleted.")
                );
            }

            return Ok(
                ApiResponse<string>.SuccessResult(null, "Partner updated successfully.")
            );
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(ApiResponse<string>
                .SuccessResult(null, "Partner deleted successfully."));
        }

    }
}
