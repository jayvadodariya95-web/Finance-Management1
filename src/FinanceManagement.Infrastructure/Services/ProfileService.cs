using FinanceManagement.Application.Common;
using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;

namespace FinanceManagement.Infrastructure.Services;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _profileRepo;

    public ProfileService(IProfileRepository profileRepo)
    {
        _profileRepo = profileRepo;
    }

    public async Task<ApiResponse<IEnumerable<Profile>>> GetAllAsync()
    {
        var response = new ApiResponse<IEnumerable<Profile>>();
        try
        {
            var profiles = await _profileRepo.GetAllAsync();

            response.Data = profiles;
            response.Message = "Profiles fetched successfully";
            response.Success = true;
        }
        catch (Exception)
        {
            response.Message = "Failed to fetch profiles";
            response.Success = false;
        }

        return response;
    }

    public async Task<ApiResponse<ProfileDto>> GetByIdAsync(int id)
    {
        var response = new ApiResponse<ProfileDto>();

        try
        {
            var profile = await _profileRepo.GetByIdAsync(id);

            if (profile == null)
            {
                response.Message = "Profile not found";
                response.Success = false;
                return response;
            }

            var result = new ProfileDto
            {
                UserId = profile.UserId,
                IsPaid = profile.IsPaid,
                Amount = profile.Amount,
                CreatedAt = profile.CreatedAt
            };

            response.Data = result;
            response.Message = "Profile fetched successfully";
            response.Success = true;
        }
        catch (Exception)
        {
            response.Message = "Failed to fetch profile";
            response.Success = false;
        }

        return response;
    }

    public async Task<ApiResponse<ProfileDto>> CreateAsync(CreateProfileDto dto)
    {
        var response = new ApiResponse<ProfileDto>();

        try
        {
            var entity = new Profile
            {
                UserId = dto.UserId,
                IsPaid = dto.IsPaid,
                Amount = dto.Amount,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _profileRepo.CreateAsync(entity);

            var data = new ProfileDto
            {
                UserId = result.UserId,
                IsPaid = result.IsPaid,
                Amount = result.Amount,
                CreatedAt = result.CreatedAt
            };
            response.Data = data;
            response.Message = "Profile created successfully";
            response.Success = true;
        }
        catch (Exception)
        {
            response.Message = "Failed to create profile";
            response.Success = false;
        }

        return response;
    }

    public async Task<ApiResponse<ProfileDto>> UpdateAsync(int id, UpdateProfileDto dto)
    {
        var response = new ApiResponse<ProfileDto>();

        try
        {
            var existing = await _profileRepo.GetByIdAsync(id);

            if (existing == null)
            {
                response.Message = "Profile not found";
                response.Success = false;
            }

            existing.UserId = dto.UserId;
            existing.IsPaid = dto.IsPaid;
            existing.Amount = dto.Amount;
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _profileRepo.UpdateAsync(existing);

            var result = new ProfileDto
            {
                UserId = updated.UserId,
                IsPaid = updated.IsPaid,
                Amount = updated.Amount,
                CreatedAt = updated.CreatedAt
            };
            response.Data = result;
            response.Message = "Profile updated successfully";
            response.Success = true;
        }
        catch (Exception)
        {
            response.Message = "Failed to update profile";
            response.Success = false;
        }

        return response;
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        var response = new ApiResponse<bool>();

        try
        {
            var deleted = await _profileRepo.DeleteAsync(id);

            if (!deleted)
            {
                response.Message = "Profile not found";
                response.Success = false;
            }

            response.Data = true;
            response.Message = "Profile deleted successfully";
            response.Success = true;
        }
        catch (Exception)
        {
            response.Message = "Failed to delete profile";
            response.Success = false;
        }

        return response;
    }
}