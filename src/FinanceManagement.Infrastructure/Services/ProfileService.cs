using FinanceManagement.Application.Common;
using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepo;

        public ProfileService(IProfileRepository profileRepo)
        {
            _profileRepo = profileRepo;
        }

        public async Task<ApiResponse<IEnumerable<ProfileDto>>> GetAllAsync()
        {
            var profiles = await _profileRepo.GetAllAsync();

            var data = profiles.Select(p => new ProfileDto
            {
                Id = p.Id,
                UserId = p.UserId,
                IsPaid = p.IsPaid,
                Amount = p.Amount,
                CreatedAt = p.CreatedAt
            });
            return ApiResponse<IEnumerable<ProfileDto>>
                .SuccessResult(data, "Profile Fetched Successfully");
        }

        public async Task<ApiResponse<ProfileDto?>> GetByIdAsync(int id)
        {
            var p = await _profileRepo.GetByIdAsync(id);

            if (p == null) return null;
            {
                return ApiResponse<ProfileDto>.ErrorResult(
                    "Profile not found"
                );
            }
            var dto = new ProfileDto
            {
                Id = p.Id,
                UserId = p.UserId,
                IsPaid = p.IsPaid,
                Amount = p.Amount,
                CreatedAt = p.CreatedAt
            };
            return ApiResponse<ProfileDto>
                .SuccessResult(dto, "Profile fetched successfully");

        }

        public async Task<ApiResponse<ProfileDto>> CreateAsync(CreateProfileDto dto)
        {

            var userExists = await _profileRepo
                .GetAllAsync()
                .ContinueWith(t => t.Result.Any(u => u.UserId == dto.UserId));

            if (userExists == null)
                throw new Exception($"UserId {dto.UserId} not found");

            var entity = new Profile
            {
                UserId = dto.UserId,
                IsPaid = dto.IsPaid,
                Amount = dto.Amount,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _profileRepo.CreateAsync(entity);

            var responseDto =  new ProfileDto
            {
                Id = result.Id,
                UserId = result.UserId,
                IsPaid = result.IsPaid,
                Amount = result.Amount,
                CreatedAt = result.CreatedAt
            };
            return ApiResponse<ProfileDto>
                .SuccessResult(responseDto, "Profile created successfully");

        }
        public async Task<ApiResponse<ProfileDto>> UpdateAsync(int id, UpdateProfileDto dto)
        {
            var existing = await _profileRepo.GetByIdAsync(id);

            if (existing == null)
                throw new Exception("Profile not found");

            existing.UserId = dto.UserId;
            existing.IsPaid = dto.IsPaid;
            existing.Amount = dto.Amount;
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _profileRepo.UpdateAsync(existing);

            var responseDto = new ProfileDto
            {
                Id = updated.Id,
                UserId = updated.UserId,
                IsPaid = updated.IsPaid,
                Amount = updated.Amount,
                CreatedAt = updated.CreatedAt
            };
            return ApiResponse<ProfileDto>
                .SuccessResult(responseDto, "Profile updated successfully");
        }

        public async Task<ApiResponse<bool>>DeleteAsync(int id)
        {
            var deleted =  await _profileRepo.DeleteAsync(id);
            if (!deleted)
            {
                return ApiResponse<bool>.ErrorResult(
                    "Profile not found"
                );
            }
            return ApiResponse<bool>
                .SuccessResult(true, "Profile deleted successfully");
        }
    }

}
