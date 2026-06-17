using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Repositories
{
    public class EmployeeSalaryRepository : IEmployeeSalaryRepository
    {
        private readonly FinanceDbContext _context;

        public EmployeeSalaryRepository(FinanceDbContext context)
        {
            this._context = context;
        }

        public async Task<EmployeeSalary> AddAsyncs(EmployeeSalary emp, MonthlyExpense MonthlyExpense)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.MonthlyExpenses.AddAsync(MonthlyExpense);
                await _context.SaveChangesAsync();
                int expenseId = MonthlyExpense.Id;
                emp.ExpenseId = expenseId;
                await _context.EmployeeSalaries.AddAsync(emp);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
            return emp;
        }

        public async Task<List<EmployeeSalary>> AddBulkAsync(MonthlyExpense monthlyExpense,List<EmployeeSalary> salaries)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.MonthlyExpenses.AddAsync(monthlyExpense);
                await _context.SaveChangesAsync();
                int expenseId = monthlyExpense.Id;
                foreach (var salary in salaries)
                {
                    salary.ExpenseId = expenseId;
                }
                await _context.EmployeeSalaries.AddRangeAsync(salaries);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return salaries;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<Employee>> GetAvailableEmployeesAsync()
        {
            var employees = await _context.Employees
         .Where(e => !_context.EmployeeSalaries
             .Any(es => es.EmployeeId == e.Id && !es.IsDeleted)
             && !e.IsDeleted)
         .ToListAsync();
            return employees;
        }
    }
}
