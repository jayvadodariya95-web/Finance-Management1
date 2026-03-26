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

        public async Task<IEnumerable<ProfileDto>> GetAllAsync()
        {
            var profiles = await _profileRepo.GetAllAsync();

            return profiles.Select(p => new ProfileDto
            {
                Id = p.Id,
                UserId = p.UserId,
                IsPaid = p.IsPaid,
                Amount = p.Amount,
                CreatedAt = p.CreatedAt
            });
        }

        public async Task<ProfileDto?> GetByIdAsync(int id)
        {
            var p = await _profileRepo.GetByIdAsync(id);

            if (p == null) return null;

            return new ProfileDto
            {
                Id = p.Id,
                UserId = p.UserId,
                IsPaid = p.IsPaid,
                Amount = p.Amount,
                CreatedAt = p.CreatedAt
            };
        }

        public async Task<ProfileDto> CreateAsync(CreateProfileDto dto)
        {
            // ✅ FIX: validate user exists
            var userExists = await _profileRepo
                .GetAllAsync()
                .ContinueWith(t => t.Result.Any(u => u.UserId == dto.UserId));

            if (!userExists)
                throw new Exception($"UserId {dto.UserId} not found");

            var entity = new Profile
            {
                UserId = dto.UserId,
                IsPaid = dto.IsPaid,
                Amount = dto.Amount,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _profileRepo.CreateAsync(entity);

            return new ProfileDto
            {
                Id = result.Id,
                UserId = result.UserId,
                IsPaid = result.IsPaid,
                Amount = result.Amount,
                CreatedAt = result.CreatedAt
            };
        }
        public async Task<ProfileDto> UpdateAsync(int id, UpdateProfileDto dto)
        {
            var existing = await _profileRepo.GetByIdAsync(id);

            if (existing == null)
                throw new Exception("Profile not found");

            existing.UserId = dto.UserId;
            existing.IsPaid = dto.IsPaid;
            existing.Amount = dto.Amount;
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _profileRepo.UpdateAsync(existing);

            return new ProfileDto
            {
                Id = updated.Id,
                UserId = updated.UserId,
                IsPaid = updated.IsPaid,
                Amount = updated.Amount,
                CreatedAt = updated.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _profileRepo.DeleteAsync(id);
        }
    }

}
