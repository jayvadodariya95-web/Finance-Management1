using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Domain.Enums;
using FinanceManagement.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IMonthlyExpenseRepository _repository;
        private readonly IPartnerRepository _partner;

        public ExpenseService(IMonthlyExpenseRepository repository,IPartnerRepository partner)
        {
            _repository = repository;
            this._partner = partner;
        }

        public async Task<IEnumerable<ExpenseDto>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();
            return data.Select(MapToDto);
        }

        public async Task<ExpenseDto?> GetByIdAsync(int id)
        {
            var data = await _repository.GetByIdAsync(id);
            return data == null ? null : MapToDto(data);
        }

        public async Task<ExpenseDto> CreateAsync(CreateMonthlyExpenseDto dto)
        {
            var entity = new MonthlyExpense
            {
                AssetId = dto.AssetId,
                PartnerId = dto.PartnerId,
                EmployeeId = dto.EmployeeId == 0 ? null : dto.EmployeeId,
                Description = dto.Description,
                Amount = dto.Amount,
                CategoryId = dto.CategoryId,
                Month = dto.Month,
                Year = dto.Year,
                IsRecurring = dto.IsRecurring
            };

            var x = await _repository.AddAsync(entity);

            return new ExpenseDto
            {
                Id = x.Id,
                PartnerId = x.PartnerId,
                AssetId = x.AssetId,
                EmployeeId = x.EmployeeId,
                Description = x.Description,
                Amount = x.Amount,
                CategoryId = x.CategoryId,
                Month = x.Month,
                Year = x.Year,
                IsRecurring = x.IsRecurring,
                ApprovedBy = x.ApprovedBy,
                ApprovedDate = x.ApprovedDate
            };
        }

        public async Task<ExpenseDto?> UpdateAsync(int id, UpdateMonthlyExpenseDto dto)
        {
            var entity = new MonthlyExpense
            {
                Id = id,
                AssetId = dto.AssetId,
                PartnerId = dto.PartnerId,
                EmployeeId = dto.EmployeeId,
                Description = dto.Description,
                Amount = dto.Amount,
                CategoryId = dto.CategoryId,
                Month = dto.Month,
                Year = dto.Year,
                IsRecurring = dto.IsRecurring
            };

            var updated = await _repository.UpdateAsync(entity);
            return updated == null ? null : MapToDto(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ExpenseDto>> GetByMonthYearAsync(int month, int year)
        {
            var data = await _repository.GetByMonthYearAsync(month, year);
            return data.Select(MapToDto);
        }
        public async Task<ExpenseDto?> PatchAsync(int id, PatchMonthlyExpenseDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            // Only update fields if they are provided (not null)

            if (dto.AssetId.HasValue)
                existing.AssetId = dto.AssetId.Value;

            if (dto.PartnerId.HasValue)
                existing.PartnerId = dto.PartnerId.Value;

            if (dto.EmployeeId.HasValue)
                existing.EmployeeId = dto.EmployeeId == 0 ? null : dto.EmployeeId;

            if (!string.IsNullOrWhiteSpace(dto.Description))
                existing.Description = dto.Description;

            if (dto.Amount.HasValue)
                existing.Amount = dto.Amount.Value;

            if (dto.CategoryId != 0)
                existing.CategoryId = dto.CategoryId;

            if (dto.Month.HasValue)
                existing.Month = dto.Month.Value;

            if (dto.Year.HasValue)
                existing.Year = dto.Year.Value;

            if (dto.IsRecurring.HasValue)
                existing.IsRecurring = dto.IsRecurring.Value;

            var updated = await _repository.UpdateAsync(existing);

            return updated == null ? null : MapToDto(updated);
        }

        public async Task<ExpenseDto?> ApproveAsync(int id, int currentUserId)
        {
            var expense = await _repository.GetByIdAsync(id);
            if (expense == null)
                return null;

            var partner = await _partner.GetByUserID(currentUserId);
            if (partner == null)
                throw new Exception("Only partners can approve.");

            if (expense.ApprovedBy == currentUserId.ToString())
                throw new Exception("You cannot approve your own expense.");

            if (expense.ApprovedBy != null)
                throw new Exception("Expense already approved.");

            expense.ApprovedBy = currentUserId.ToString();
            expense.ApprovedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(expense);

            return MapToDto(expense);
        }

        private ExpenseDto MapToDto(MonthlyExpense x)
        {
            return new ExpenseDto
            {
                Id = x.Id,
                PartnerId = x.PartnerId,
                AssetId = x.AssetId,
                EmployeeId = x.EmployeeId,
                Description = x.Description,
                Amount = x.Amount,
                CategoryId = x.CategoryId,
                Month = x.Month,
                Year = x.Year,
                IsRecurring = x.IsRecurring,
                ApprovedBy = x.ApprovedBy,
                ApprovedDate = x.ApprovedDate,

            };
        }
    }
}
