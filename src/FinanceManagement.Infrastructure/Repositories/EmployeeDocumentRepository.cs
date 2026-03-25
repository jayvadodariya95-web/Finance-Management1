using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using FinanceManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Repositories
{
    public class EmployeeDocumentRepository : IEmployeeDocumentRepository
    {
        private readonly FinanceDbContext _context;
        public EmployeeDocumentRepository(FinanceDbContext _context) 
        {
            this._context = _context;
        }

        public async Task<Documents> AddDocumentAsync(Documents document)
        {
            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task<EmployeeDocument> AddEmployeeDocumentAsync(EmployeeDocument employeeDocument)
        {
            await _context.EmployeeDocuments.AddAsync(employeeDocument);
            await _context.SaveChangesAsync();
            return employeeDocument;
        }
    }
}
