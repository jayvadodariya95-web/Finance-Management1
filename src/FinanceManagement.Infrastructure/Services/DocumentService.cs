using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Services
{
    public class DocTypeService : IDocTypeService
    {
        private readonly IDocTypeRepository _repository;

        public DocTypeService(IDocTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DocType>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<DocType?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<DocType> CreateAsync(CreateDocTypeDto docType)
        {
            var entity = new DocType
            {
                TypeName = docType.TypeName
            };

            var created = await _repository.AddAsync(entity);
            return created;
        }

        public async Task<DocType?> UpdateAsync(int id, DocType docType)
        {
            docType.Id = id;
            return await _repository.UpdateAsync(docType);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<DocType?> PatchAsync(int id, string? typeName)
        {
            return await _repository.PatchAsync(id, typeName);
        }
    }
}
