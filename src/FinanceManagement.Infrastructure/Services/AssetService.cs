using FinanceManagement.Application.DTOs;
using FinanceManagement.Application.Interfaces;
using FinanceManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;

        public AssetService(IAssetRepository assetRepository)
        {
            this._assetRepository = assetRepository;
        }

        public async Task<IEnumerable<Asset>> GetAllAssetsAsync()
        {
            return await _assetRepository.GetAllAsync();
        }

        public async Task<Asset?> GetAssetByIdAsync(int id)
        {
            return await _assetRepository.GetByIdAsync(id);
        }

        public async Task<Asset> CreateAssetAsync(CreateAssetDto dto)
        {
            var asset = new Asset
            {
                Name = dto.Name,
                Description = dto.Description,
                Amount = dto.Amount,
                Purchase_Date = dto.PurchaseDate
            };

            var data = await _assetRepository.AddAsync(asset);

            return data;
        }

        public async Task<Asset> UpdateAssetAsync(int id, Asset asset)
        {
            var existing = await _assetRepository.GetByIdAsync(id);
            if (existing == null) throw new InvalidDataException("Assest not found!");

            existing.Name = asset.Name;
            existing.Description = asset.Description;
            existing.Amount = asset.Amount;
            existing.Purchase_Date = asset.Purchase_Date;

            await _assetRepository.Update(existing);
            

            return existing;
        }

        public async Task<bool> DeleteAssetAsync(int id)
        {
            var existing = await _assetRepository.GetByIdAsync(id);
            if (existing == null) return false;

            _assetRepository.Delete(existing);
            

            return true;
        }
    }
}
