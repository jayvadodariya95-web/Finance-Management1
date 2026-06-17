using FinanceManagement.Application.Common;
using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Services
{
    public class EmployeeSalaryService : IEmployeeSalaryService
    {
        private readonly IEmployeeSalaryRepository context;

        public EmployeeSalaryService(IEmployeeSalaryRepository  context)
        {
            this.context = context;
        }

        public async Task<ApiResponse<EmployeeSalary>> AddAsync(CreateEmployeeSalaryDto dto)
        {
            var response = new ApiResponse<EmployeeSalary>();
            try
            {

                var entity = new EmployeeSalary
                {
                    EmployeeId = dto.EmployeeId,
                    PartnerId = dto.PartnerId,
                    Amount = dto.Amount,
                    SalaryDate = dto.SalaryDate
                };

                var entitys = new MonthlyExpense
                {
                    Description = dto.Description,
                    Amount = dto.Amount,
                    Month = dto.SalaryDate.Month,
                    Year = dto.SalaryDate.Year,
                    IsRecurring = dto.IsRecurring,
                    AssetId = dto.AssetId,
                    EmployeeId = dto.EmployeeId,
                    PartnerId = dto.PartnerId,

                    CategoryId = dto.CategoryId 
                };

                await context.AddAsyncs(entity,entitys);

                response.Success = true;
                response.Message = "Employee salary added successfully.";
                response.Data = entity;
                response.Timestamp = DateTime.UtcNow;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while adding the employee salary.";
                response.Errors = new List<string> { ex.Message };
                response.Timestamp = DateTime.UtcNow;
                return response;
            }
        }

        public async Task<ApiResponse<List<EmployeeSalary>>> AddBulkAsync(List<CreateEmployeeSalaryDto> dtos)
        {
            var response = new ApiResponse<List<EmployeeSalary>>();

            try
            { 
                var totalAmount = dtos.Sum(x => x.Amount);

                var first = dtos.First();
                var monthlyExpense = new MonthlyExpense
                {
                    Description = first.Description,
                    Amount = totalAmount,
                    Month = first.SalaryDate.Month,
                    Year = first.SalaryDate.Year,
                    IsRecurring = first.IsRecurring,
                    AssetId = first.AssetId,
                    EmployeeId = null,
                    PartnerId = first.PartnerId,
                    CategoryId = first.CategoryId
                };
                var salaries = new List<EmployeeSalary>();

                foreach (var dto in dtos)
                {
                    var salary = new EmployeeSalary
                    {
                        EmployeeId = dto.EmployeeId,
                        PartnerId = dto.PartnerId,
                        Amount = dto.Amount,
                        SalaryDate = dto.SalaryDate,
                    };

                    salaries.Add(salary);
                }
                var result = await context.AddBulkAsync(monthlyExpense, salaries);
                response.Success = true;
                response.Data = salaries;
                response.Message = "Bulk insert successful";
            }
            catch (Exception ex)
            {

                response.Success = false;
                response.Message = "Bulk insert failed";
                response.Errors = new List<string> { ex.Message };

            }
            return response;

        }

        public async Task<ApiResponse<List<Employee>>> GetAvailableEmployeesAsync()
        {
            var response = new ApiResponse<List<Employee>>();
            try
            {
                var employees = await context.GetAvailableEmployeesAsync();
                response.Success = true;
                response.Data = employees;
                response.Message = "Available employees fetched successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while fetching available employees.";
                response.Errors = new List<string> { ex.Message };
            }
            return response;
        }
    }
    
}
