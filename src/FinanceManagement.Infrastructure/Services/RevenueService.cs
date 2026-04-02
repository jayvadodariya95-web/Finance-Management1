using FinanceManagement.Application.Common;
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

        public async Task<ApiResponse<Revenue>> CreateAsync(RevenueDTO revenue)
        {
            var response = new ApiResponse<Revenue>();
            try
            {

                if (revenue == null)
                {
                    response.Success = false;
                    response.Message = "Revenue data is required.";
                }
                if (revenue.Amount <= 0)
                {
                    response.Success = false;
                    response.Message = "Amount must be greater than zero.";
                }

                if (revenue.ProjectId == 0)
                {
                    response.Success = false;
                    response.Message = "ProjectId is set to null because it was provided as 0.";
                }

                var newRevenue = new Revenue

        public async Task<IEnumerable<RevenueDTO>> GetAllAsync()
        {
            var result = await _revenueRepository.GetAllAsync();
            return result;
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var response = new ApiResponse<bool>();
            try
            {
                var result = await _revenueRepository.DeleteAsync(id);
                if (result == false)
                {
                    response.Success = false;
                    response.Message = "Revenue not found";
                }
                else
                {
                    response.Success = true;
                    response.Message = "Revenue deleted successfully.";
                    response.Timestamp = DateTime.UtcNow;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while deleting the revenue.";
                response.Timestamp = DateTime.UtcNow;
            }
            return response;
        }

        public async Task<ApiResponse<IEnumerable<Revenue>>> GetAllAsync()
        {
            var response = new ApiResponse<IEnumerable<Revenue>>();
            try
            {
                var result = await _revenueRepository.GetAllAsync();
                response.Success = true;
                response.Message = "Revenues retrieved successfully.";
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while retrieving revenues.";
                response.Timestamp = DateTime.UtcNow;
                return response;
            }
            return response;
        }

        public async Task<ApiResponse<Revenue?>> GetByIdAsync(int id)
        {
            var response = new ApiResponse<Revenue?>();
            try
            {

                var result = await _revenueRepository.GetByIdAsync(id);
                response.Success = true;
                response.Message = "Revenue retrieved successfully.";
                response.Timestamp = DateTime.UtcNow;
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while retrieving the revenue.";
                response.Timestamp = DateTime.UtcNow;
            }
            return response;
        }

        public async Task<ApiResponse<Revenue?>> UpdateAsync(int id, RevenueDTO revenue)
        {
            var response = new ApiResponse<Revenue?>();
            try
            {
                if (revenue == null)
                {
                    response.Success = false;
                    response.Message = "Revenue data is required.";
                }
                var ExisitngRevenue = await _revenueRepository.GetByIdAsync(id);
                if (ExisitngRevenue == null)
                {
                    response.Success = false;
                    response.Message = "Revenue not found.";
                    response.Timestamp = DateTime.UtcNow;

                }

                ExisitngRevenue.PartnerId = revenue.PartnerId;
                ExisitngRevenue.ProjectId = revenue.ProjectId;
                ExisitngRevenue.Amount = revenue.Amount;
                ExisitngRevenue.Date = DateTime.Now;
                ExisitngRevenue.Revenue_From = revenue.Revenue_From;
                ExisitngRevenue.Notes = revenue.Notes;

                var result = await _revenueRepository.UpdateAsync(ExisitngRevenue);

                response.Success = true;
                response.Message = "Revenue updated successfully.";
                response.Timestamp = DateTime.UtcNow;
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while updating the revenue.";
                response.Timestamp = DateTime.UtcNow;
            }
            return response;
        }

        public async Task<ApiResponse<Revenue?>> PatchAsync(int id, PatchRevenueDTO dto)
        {
            var response = new ApiResponse<Revenue?>();
            try
            {
                if (dto == null)
                {
                    response.Success = false;
                    response.Message = "Revenue data is required.";
                }

                var existing = await _revenueRepository.GetByIdAsync(id);


                if (existing == null)
                {
                    response.Success = false;
                    response.Message = "Revenue not found.";
                    response.Timestamp = DateTime.UtcNow;
                    return response;
                }

                if (dto.PartnerId.HasValue)
                    existing.PartnerId = dto.PartnerId.Value;

                if (dto.ProjectId == null)
                {
                    response.Success = false;
                    response.Message = "ProjectId is set to null because it was provided as null.";
                    response.Timestamp = DateTime.UtcNow;
                }
                else
                {
                    existing.ProjectId = dto.ProjectId.Value;
                }

                if (dto.Amount.HasValue)
                {
                    if (dto.Amount <= 0)
                    {
                        response.Success = false;
                        response.Message = "Amount must be greater than zero.";
                    }

                    existing.Amount = dto.Amount.Value;
                }

                if (dto.Date.HasValue)
                    existing.Date = dto.Date.Value;

                if (dto.Revenue_From.HasValue)
                    existing.Revenue_From = dto.Revenue_From.Value;

                if (dto.Notes != null)
                    existing.Notes = dto.Notes;

                var result = await _revenueRepository.UpdateAsync(existing);
                response.Success = true;
                response.Message = "Revenue patched successfully.";
                response.Timestamp = DateTime.UtcNow;
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while patching the revenue.";
                response.Timestamp = DateTime.UtcNow;
            }
            return response;
        }
    }
}
