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
    public class PartnerService : IPartnerService
    {
        private readonly IPartnerRepository _repository;

        public PartnerService(IPartnerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PartnerResponseDto>> GetAllAsync()
        {
            var partners = await _repository.GetAllAsync();

            return partners.Select(p => new PartnerResponseDto
            {
                Id = p.Id,
                UserId = p.UserId,
                FullName = $"{p.User.FirstName} {p.User.LastName}",
                Email = p.User.Email,
                PartnershipType = p.PartnershipType,
                SharePercentage = p.SharePercentage,
                BranchId = p.BranchId,
                BranchName = p.Branch != null ? p.Branch.Name : null,
                IsMainPartner = p.IsMainPartner
            });
        }

        public async Task<PartnerResponseDto?> GetByIdAsync(int id)
        {
            var p = await _repository.GetByIdAsync(id);
            if (p == null) return null;

            return new PartnerResponseDto
            {
                Id = p.Id,
                UserId = p.UserId,
                FullName = $"{p.User.FirstName} {p.User.LastName}",
                Email = p.User.Email,
                PartnershipType = p.PartnershipType,
                SharePercentage = p.SharePercentage,
                BranchId = p.BranchId,
                BranchName = p.Branch != null ? p.Branch.Name : null,
                IsMainPartner = p.IsMainPartner
            };
        }

        public async Task<int> CreateAsync(CreatePartnerDto dto)
        {
            var partner = new Partner
            {
                UserId = dto.UserId,
                PartnershipType = dto.PartnershipType,
                SharePercentage = dto.SharePercentage,
                BranchId = dto.BranchId,
                IsMainPartner = dto.IsMainPartner
            };

            await _repository.AddAsync(partner);
            return partner.Id;
        }

        public async Task<bool> UpdateAsync(int id, UpdatePartnerDto dto)
        {
            var partner = await _repository.GetByIdAsync(id);
            if (partner == null) return false;

            partner.PartnershipType = dto.PartnershipType;
            partner.SharePercentage = dto.SharePercentage;
            partner.BranchId = dto.BranchId;
            partner.IsMainPartner = dto.IsMainPartner;

            await _repository.UpdateAsync(partner);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var partner = await _repository.GetByIdAsync(id);

            if (partner == null)
                return false;
             partner.IsDeleted = true;
            await _repository.UpdateAsync(partner);
            return true;
        }
    }
}
