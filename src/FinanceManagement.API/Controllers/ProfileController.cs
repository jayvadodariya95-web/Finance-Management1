using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Profile>>> GetAllProfiles()
    {
        var profiles = await _profileService.GetAllAsync();
        return Ok(profiles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProfileDto>> GetProfile(int id)
    {
        var profile = await _profileService.GetByIdAsync(id);
        return Ok(profile);
    }

    [HttpPost]
    public async Task<ActionResult<ProfileDto>> CreateProfile([FromBody] CreateProfileDto request)
    {
        var result = await _profileService.CreateAsync(request);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProfileDto>> UpdateProfile(int id, [FromBody] UpdateProfileDto request)
    {
        var profile = await _profileService.UpdateAsync(id, request);
        return Ok(profile);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> DeleteProfile(int id)
    {
        var result = await _profileService.DeleteAsync(id);
        return Ok(result);
    }
}