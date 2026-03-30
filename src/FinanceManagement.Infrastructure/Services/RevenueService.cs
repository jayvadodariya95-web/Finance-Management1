using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinanceManagement.Infrastructure.Services
{
    public class RevenueService : IRevenueService
    {
        private readonly IRevenueRepository _revenueRepository;
        public RevenueService(IRevenueRepository revenueRepository)
        {
            this._revenueRepository = revenueRepository;
        }

        public async Task<Revenue> CreateAsync(RevenueDTO revenue)
        {
            if (revenue == null)
            {
                throw new ArgumentNullException(nameof(revenue));
            }
            if (revenue.Amount <= 0)
            {
                throw new ArgumentException("Amount cannot be zero");
            }

            var NewRevenue = new Revenue
            {
                PartnerId = revenue.PartnerId,
                ProjectId = revenue.ProjectId,
                Amount = revenue.Amount,
                Date = DateTime.Now,
                Revenue_From = revenue.Revenue_From ?? true,
                Notes = revenue.Notes
            };

            return await _revenueRepository.CreateAsync(NewRevenue);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var revenue = await _revenueRepository.GetByIdAsync(id);

            if (revenue == null) return false;
            revenue.IsDeleted = true;
            return await _revenueRepository.DeleteAsync();
        }

        public async Task<IEnumerable<Revenue>> GetAllAsync()
        {
            return await _revenueRepository.GetAllAsync();
        }

        public async  Task<Revenue?> GetByIdAsync(int id)
        {
            return await _revenueRepository.GetByIdAsync(id);
        }

        public async Task<Revenue?> UpdateAsync(int id, RevenueDTO revenue)
        {
            
            if (revenue == null)
            {
                throw new ArgumentNullException(nameof(revenue));
            }
            var ExisitngRevenue = await _revenueRepository.GetByIdAsync(id);
            if (ExisitngRevenue == null)
            {
                return null;
            }

            ExisitngRevenue.PartnerId = revenue.PartnerId;
            ExisitngRevenue.ProjectId = revenue.ProjectId;
            ExisitngRevenue.Amount = revenue.Amount;
            ExisitngRevenue.Date = DateTime.Now;
            ExisitngRevenue.Revenue_From = revenue.Revenue_From ?? true;
            ExisitngRevenue.Notes = revenue.Notes;

            return await _revenueRepository.UpdateAsync(ExisitngRevenue);
        }

        public async Task<Revenue?> PatchAsync(int id, PatchRevenueDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var existing = await _revenueRepository.GetByIdAsync(id);
            if (existing == null)
                return null;

            if (dto.PartnerId.HasValue)
                existing.PartnerId = dto.PartnerId.Value;

            if (dto.ProjectId == null)
            {
                existing.ProjectId = null;
            }
            else
            {
                existing.ProjectId = dto.ProjectId.Value;
            }

            if (dto.Amount.HasValue)
            {
                if (dto.Amount <= 0)
                    throw new ArgumentException("Amount must be greater than zero.");

                existing.Amount = dto.Amount.Value;
            }

            if (dto.Date.HasValue)
                existing.Date = dto.Date.Value;

            if (dto.Revenue_From.HasValue)
                existing.Revenue_From = dto.Revenue_From.Value;

            if (dto.Notes != null)
                existing.Notes = dto.Notes;

            return await _revenueRepository.UpdateAsync(existing);
        }
    }
}
