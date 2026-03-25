using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using Microsoft.AspNetCore.Hosting;


namespace FinanceManagement.Infrastructure.Services
{
    public class EmployeeDocumentService : IEmployeeDocumentService
    {
        private readonly IEmployeeDocumentRepository _employeeDocumentRepository;
        private readonly IWebHostEnvironment _uploadEnvironment;

        public EmployeeDocumentService(IEmployeeDocumentRepository employeeDocumentRepository, IWebHostEnvironment uploadEnvironment)
        {
            _employeeDocumentRepository = employeeDocumentRepository;
            _uploadEnvironment = uploadEnvironment;
        }

        public async Task UploadAsync(UploadEmployeeDocumentDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
            {
                throw new FileNotFoundException("File is empty");
            }

            var folderpath = Path.Combine(
                _uploadEnvironment.WebRootPath,
                "uploads",
                "employees",
                dto.EmployeeId.ToString());

            if(!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(dto.File.FileName);
            var fullPath = Path.Combine(folderpath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            var document = new Documents
            {
                DocType_Id = dto.DocType_Id,
                Link = $"/uploads/employees/{dto.EmployeeId}/{fileName}"
            };
            await _employeeDocumentRepository.AddDocumentAsync(document);
            var employeeDocument = new EmployeeDocument
            {
                EmployeeId = dto.EmployeeId,
                DocumentId = document.Id
            };

            await _employeeDocumentRepository.AddEmployeeDocumentAsync(employeeDocument);
        }
    }
}
