using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinanceManagement.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly FinanceDbContext context;

        public EmployeeRepository(FinanceDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            var data = await context.Employees
                            .Include(e => e.User)
                            .Include(e => e.Branch)
                            .Where(e => !e.IsDeleted)
                            .ToListAsync();
            return data;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            var data = await context.Employees.Where(e => e.Id == id && !e.IsDeleted).FirstOrDefaultAsync();
            return data;
        }
        public async Task<Employee> CreateAsync(Employee employee)
        {
            context.Employees.Add(employee);
            await context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateAsync(Employee employee, int id)
        {
            var data = await context.Employees.Where(e => e.Id == id).FirstOrDefaultAsync();

            data.EmployeeCode = employee.EmployeeCode;
            data.Department = employee.Department;
            data.Position = employee.Position;
            data.MonthlySalary = employee.MonthlySalary;
            data.PreviousCTC = employee.PreviousCTC;
            data.CurrentCTC = employee.CurrentCTC;
            data.JoinDate = employee.JoinDate;
            data.RelievingDate = employee.RelievingDate;
            data.TakenLeave = employee.TakenLeave;
            data.IsActive = employee.IsActive;

            context.Employees.Update(data);
            await context.SaveChangesAsync();
            return data;
        }

        public async Task<Employee> DeleteAsync(Employee employee, int id)
        {
            var data = await context.Employees.Where(e => e.Id == id).FirstOrDefaultAsync();
           
            data.IsDeleted = true;

            context.Employees.Update(data);
            await context.SaveChangesAsync();
            return data;
        }
    }
}
