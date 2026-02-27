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
                var employees = await repository.GetAllAsync();

                var employeeDtos = employees.Select(e => new GetAllEmployeeDto
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

                response.Data = employeeDtos;
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
                var employee = await repository.GetByIdAsync(id);
                if (employee == null)
                {
                    response.Message = "Employee does not exist";
                    response.Success = false;
                    return response;
                }

                var employeeDto = new GetAllEmployeeDto
                {
                    Id = employee.Id,
                    UserId = employee.UserId,
                    BranchId = employee.BranchId,
                    EmployeeCode = employee.EmployeeCode,
                    Department = employee.Department,
                    Position = employee.Position,
                    MonthlySalary = employee.MonthlySalary,
                    PreviousCTC = employee.PreviousCTC,
                    CurrentCTC = employee.CurrentCTC,
                    JoinDate = employee.JoinDate,
                    RelievingDate = employee.RelievingDate,
                    TakenLeave = employee.TakenLeave,
                    IsActive = employee.IsActive
                };

                response.Data = employeeDto;
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
        public async Task<ApiResponse<EmployeeCreateDto>> CreateAsync(EmployeeCreateDto employeeCreateDto)
        {
            var response = new ApiResponse<EmployeeCreateDto>();
            try
            {
                var employeeEntity = new Employee
                {
                    UserId = employeeCreateDto.UserId,
                    BranchId = employeeCreateDto.BranchId,
                    EmployeeCode = employeeCreateDto.EmployeeCode,
                    Department = employeeCreateDto.Department,
                    Position = employeeCreateDto.Position,
                    MonthlySalary = employeeCreateDto.MonthlySalary,
                    PreviousCTC = employeeCreateDto.PreviousCTC,
                    CurrentCTC = employeeCreateDto.CurrentCTC,
                    JoinDate = employeeCreateDto.JoinDate,
                    TakenLeave = employeeCreateDto.TakenLeave,
                    IsActive = employeeCreateDto.IsActive
                };

                var createdEmployee = await repository.CreateAsync(employeeEntity);

                var employeeDto = new EmployeeCreateDto
                {
                    UserId = createdEmployee.UserId,
                    BranchId = createdEmployee.BranchId,
                    EmployeeCode = createdEmployee.EmployeeCode,
                    Department = createdEmployee.Department,
                    Position = createdEmployee.Position,
                    MonthlySalary = createdEmployee.MonthlySalary,
                    PreviousCTC = createdEmployee.PreviousCTC,
                    CurrentCTC = createdEmployee.CurrentCTC,
                    JoinDate = createdEmployee.JoinDate,
                    TakenLeave = createdEmployee.TakenLeave,
                    IsActive = createdEmployee.IsActive
                };
                response.Data = employeeDto;
                response.Message = "Employee created successfully";
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
        public async Task<ApiResponse<EmployeeUpdateDto>> UpdateAsync(EmployeeUpdateDto employeeUpdateDto, int id)
        {
            var response = new ApiResponse<EmployeeUpdateDto>();
            try
            {
                var employeeId = await repository.GetByIdAsync(id);
                
                if (employeeId == null || employeeId.IsDeleted)
                {
                    response.Success = false;
                    response.Message = "Employee not found";
                    return response;
                }

                var employeeEntity = new Employee
                {
                    EmployeeCode = employeeUpdateDto.EmployeeCode,
                    Department = employeeUpdateDto.Department,
                    Position = employeeUpdateDto.Position,
                    MonthlySalary = employeeUpdateDto.MonthlySalary,
                    PreviousCTC = employeeUpdateDto.PreviousCTC,
                    CurrentCTC = employeeUpdateDto.CurrentCTC,
                    JoinDate = employeeUpdateDto.JoinDate,
                    RelievingDate = employeeUpdateDto.RelievingDate,
                    TakenLeave = employeeUpdateDto.TakenLeave,
                    IsActive = employeeUpdateDto.IsActive
                };

                var updateEmployee = await repository.UpdateAsync(employeeEntity, id);

                var employeeDto = new EmployeeUpdateDto
                {
                    EmployeeCode = updateEmployee.EmployeeCode,
                    Department = updateEmployee.Department,
                    Position = updateEmployee.Position,
                    MonthlySalary = updateEmployee.MonthlySalary,
                    PreviousCTC = updateEmployee.PreviousCTC,
                    CurrentCTC = updateEmployee.CurrentCTC,
                    JoinDate = updateEmployee.JoinDate,
                    RelievingDate = updateEmployee.RelievingDate,
                    TakenLeave = updateEmployee.TakenLeave,
                    IsActive = updateEmployee.IsActive
                };

                response.Data = employeeDto;
                response.Message = "Record successfully updated";
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

        public async Task<ApiResponse<Employee>> DeleteAsync(int id)
        {
            var response = new ApiResponse<Employee>();

            try
            {
                var employeeData = await repository.GetByIdAsync(id);
                if (employeeData == null || employeeData.IsDeleted)
                {
                    response.Success = false;
                    response.Message = "Employee not found";
                    return response;
                }

                await repository.DeleteAsync(employeeData, id);

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
