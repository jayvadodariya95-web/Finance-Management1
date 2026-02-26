using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Infrastructure.Repositories;
using Humanizer;
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

        public async Task<IEnumerable<GetAllEmployeeDto>> GetAllAsync()
        {
            var data = await repository.GetAllAsync();

            return data.Select(e => new GetAllEmployeeDto
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
        }

        public async Task<GetAllEmployeeDto> GetByIdAsync(int id)
        {
            var data = await repository.GetByIdAsync(id);
            if (data == null)
                return null;

            return new GetAllEmployeeDto
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
        }
        public async Task<EmployeeCreateDto> CreateAsync(EmployeeCreateDto dto)
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
                IsActive = dto.IsActive,

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
                IsActive = data.IsActive,
            };
            return newDTO;
        }
        public async Task<EmployeeUpdateDto> UpdateAsync(EmployeeUpdateDto dto, int id)
        {

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
            return newDto;
        }

        public async Task<GetAllEmployeeDto> DeleteAsync(int id)
        {
            var employee = await repository.GetByIdAsync(id);

            if (employee == null)
            {
                return null;
            }

            if (!employee.IsDeleted)
            {
                var data = await repository.DeleteAsync(employee, id);
                return new GetAllEmployeeDto
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
            }
            return null;
        }
    }
}
