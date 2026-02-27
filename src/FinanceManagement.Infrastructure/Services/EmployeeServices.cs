using FinanceManagement.Application.Common;
using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Infrastructure.Repositories;
using Humanizer;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Services
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly IEmployeeRepository repository;

        public EmployeeServices(IEmployeeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ApiResponse<IEnumerable<GetAllEmployeeDto>>> GetAllAsync()
        {
            var response = new ApiResponse<IEnumerable<GetAllEmployeeDto>>();
            try
            {
                var data = await repository.GetAllAsync();

                var result = data.Select(e => new GetAllEmployeeDto
                {
                    Id = e.Id,
                    UserId = e.UserId,
                    BranchId = e.BranchId,
                    EmployeeCode = e.EmployeeCode,
                    Department = e.Department,
                    Position = e.Position,
                    MonthlySalary = e.MonthlySalary,
                    PreviousCTC = e.PreviousCTC,
                    CurrentCTC = e.CurrentCTC,
                    JoinDate = e.JoinDate,
                    RelievingDate = e.RelievingDate,
                    TakenLeave = e.TakenLeave,
                    IsActive = e.IsActive
                });

                response.Data = result;
                response.Message = "Employees fetched successfully";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<ApiResponse<GetAllEmployeeDto>> GetByIdAsync(int id)
        {
            var response = new ApiResponse<GetAllEmployeeDto>();
            try
            {
                var data = await repository.GetByIdAsync(id);
                if (data == null)
                {
                    response.Message = "Employee is not exist";
                    response.Success = false;
                    return response;
                }

                var result = new GetAllEmployeeDto
                {
                    Id = data.Id,
                    UserId = data.UserId,
                    BranchId = data.BranchId,
                    EmployeeCode = data.EmployeeCode,
                    Department = data.Department,
                    Position = data.Position,
                    MonthlySalary = data.MonthlySalary,
                    PreviousCTC = data.PreviousCTC,
                    CurrentCTC = data.CurrentCTC,
                    JoinDate = data.JoinDate,
                    RelievingDate = data.RelievingDate,
                    TakenLeave = data.TakenLeave,
                    IsActive = data.IsActive
                };

                response.Data = result;
                response.Message = "Employees fetched successfully";
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Message = ex.Message;
                response.Success = false;
                return response;
            }

        }
        public async Task<ApiResponse<EmployeeCreateDto>> CreateAsync(EmployeeCreateDto dto)
        {
            var response = new ApiResponse<EmployeeCreateDto>();
            try
            {
                var employeeModel = new Employee
                {
                    UserId = dto.UserId,
                    BranchId = dto.BranchId,
                    EmployeeCode = dto.EmployeeCode,
                    Department = dto.Department,
                    Position = dto.Position,
                    MonthlySalary = dto.MonthlySalary,
                    PreviousCTC = dto.PreviousCTC,
                    CurrentCTC = dto.CurrentCTC,
                    JoinDate = dto.JoinDate,
                    TakenLeave = dto.TakenLeave,
                    IsActive = dto.IsActive
                };

                var data = await repository.CreateAsync(employeeModel);

                var newDTO = new EmployeeCreateDto
                {
                    UserId = data.UserId,
                    BranchId = data.BranchId,
                    EmployeeCode = data.EmployeeCode,
                    Department = data.Department,
                    Position = data.Position,
                    MonthlySalary = data.MonthlySalary,
                    PreviousCTC = data.PreviousCTC,
                    CurrentCTC = data.CurrentCTC,
                    JoinDate = data.JoinDate,
                    TakenLeave = data.TakenLeave,
                    IsActive = data.IsActive
                };
                response.Data = newDTO;
                response.Message = "Employee created successfully";
                response.Success = true;
            }
            catch (Exception e)
            {
                response.Data = null;
                response.Message = e.Message;
                response.Success = false;
            }

            return response;
        }
        public async Task<ApiResponse<EmployeeUpdateDto>> UpdateAsync(EmployeeUpdateDto dto, int id)
        {
            var response = new ApiResponse<EmployeeUpdateDto>();
            try
            {
                var employee = await repository.GetByIdAsync(id);
                
                if (employee == null || employee.IsDeleted)
                {
                    response.Success = false;
                    response.Message = "Employee not found";
                    return response;
                }

                var employeedto = new Employee
                {
                    EmployeeCode = dto.EmployeeCode,
                    Department = dto.Department,
                    Position = dto.Position,
                    MonthlySalary = dto.MonthlySalary,
                    PreviousCTC = dto.PreviousCTC,
                    CurrentCTC = dto.CurrentCTC,
                    JoinDate = dto.JoinDate,
                    RelievingDate = dto.RelievingDate,
                    TakenLeave = dto.TakenLeave,
                    IsActive = dto.IsActive
                };

                var data = await repository.UpdateAsync(employeedto, id);

                var newDto = new EmployeeUpdateDto
                {
                    EmployeeCode = data.EmployeeCode,
                    Department = data.Department,
                    Position = data.Position,
                    MonthlySalary = data.MonthlySalary,
                    PreviousCTC = data.PreviousCTC,
                    CurrentCTC = data.CurrentCTC,
                    JoinDate = data.JoinDate,
                    RelievingDate = data.RelievingDate,
                    TakenLeave = data.TakenLeave,
                    IsActive = data.IsActive
                };

                response.Data = newDto;
                response.Message = "Record successfully updated";
                response.Success = true;
                return response;
            }
            catch (Exception e)
            {
                response.Data = null;
                response.Message = e.Message;
                response.Success = false;
                return response;
            }
        }

        public async Task<ApiResponse<GetAllEmployeeDto>> DeleteAsync(int id)
        {
            var response = new ApiResponse<GetAllEmployeeDto>();

            try
            {
                var data = await repository.GetByIdAsync(id);
                if (data == null)
                {
                    response.Success = false;
                    response.Message = "Employee not found";
                    return response;
                }

                if (data.IsDeleted)
                {
                    response.Success = false;
                    response.Message = "Employee is not exist";
                    return response;
                }

                await repository.DeleteAsync(data, id);

                response.Success = true;
                response.Message = "Employee deleted successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
