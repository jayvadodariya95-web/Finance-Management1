using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDocumentController : ControllerBase
    {
        private readonly IEmployeeDocumentService _employeeDocumentService;

        public EmployeeDocumentController(IEmployeeDocumentService employeeDocumentService)
        {
            _employeeDocumentService = employeeDocumentService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadEmployeeDocumentDto dto)
        {
            try
            {
                await _employeeDocumentService.UploadAsync(dto);

                return Ok(new
                {
                    message = "Document uploaded successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
