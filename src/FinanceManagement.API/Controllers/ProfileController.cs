using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    // GET: api/profile
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var profiles = await _profileService.GetAllAsync();
        return Ok(profiles);
    }

    // GET: api/profile/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var profile = await _profileService.GetByIdAsync(id);
        if (profile == null) return NotFound();
        return Ok(profile);
    }

    // POST: api/profile
    [HttpPost]
    public async Task<IActionResult> Create(CreateProfileDto dto)
    {
        try
        {
            var result = await _profileService.CreateAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    // PUT: api/profile/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProfileDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _profileService.UpdateAsync(id, dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    // DELETE: api/profile/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _profileService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}