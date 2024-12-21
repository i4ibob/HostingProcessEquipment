using JuniorDotNetTestTaskServiceHostingProcessEquipment.DTOs;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.DTOs.ProductFacility;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Models;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Repositories.Interfaces;

namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.Services
{
    public class ProductionFacilityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductionFacilityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }




        public async Task<(bool Success, string ErrorMessage)> CreateAsync(ProductionFacilityDto dto)
        {
            var existingFacility = await _unitOfWork.ProductionFacilityRepository.FirstOrDefaultAsync(f => f.Code == dto.Code);
            if (existingFacility != null)
            {
                return (false, $"A production facility with Code {dto.Code} already exists.");
            }

            var productionFacility = new ProductionFacility
            {
                Code = dto.Code,
                Name = dto.Name,
                StandardArea = dto.StandardArea
            };

            await _unitOfWork.ProductionFacilityRepository.AddAsync(productionFacility);
            await _unitOfWork.SaveAsync();
            return (true, null);
        }



        public async Task<ProductionFacilityDto?> GetByIdAsync(int id)
        {
            var facility = await _unitOfWork.ProductionFacilityRepository.GetByIdAsync(id);
            if (facility == null) return null;

            return new ProductionFacilityDto
            {
                Id = facility.Id,
                Name = facility.Name,
                StandardArea = facility.StandardArea,
                Code = facility.Code,
                Contracts = facility.Contracts.Select(c => c.Id).ToList()
            };
        }

        public async Task<List<ProductionFacilityDto>> GetAllAsync()
        {
            var facilities = await _unitOfWork.ProductionFacilityRepository.GetAllAsync();
            return facilities.Select(f => new ProductionFacilityDto
            {
                Id = f.Id,
                Name = f.Name,
                StandardArea = f.StandardArea,
                Code = f.Code,
                Contracts = f.Contracts.Select(c => c.Id).ToList()
            }).ToList();
        }

        public async Task<(bool Success, string ErrorMessage)> UpdateAsync(int id, ProductionFacilityDto dto)
        {
            var facility = await _unitOfWork.ProductionFacilityRepository.GetByIdAsync(id);
            if (facility == null)
            {
                return (false, $"Production facility with ID {id} not found.");
            }

            if (facility.Code != dto.Code)
            {
                var facilityWithSameCode = await _unitOfWork.ProductionFacilityRepository.FirstOrDefaultAsync(f => f.Code == dto.Code);
                if (facilityWithSameCode != null)
                {
                    return (false, $"A production facility with Code {dto.Code} already exists.");
                }
            }

            facility.Name = dto.Name;
            facility.Code = dto.Code;
            facility.StandardArea = dto.StandardArea;

            await _unitOfWork.SaveAsync();
            return (true, null);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var facility = await _unitOfWork.ProductionFacilityRepository.GetByIdAsync(id);
            if (facility == null) return false;

            await _unitOfWork.ProductionFacilityRepository.DeleteAsync(facility);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<decimal> GetAvailableAreaAsync(int productionFacilityId)
        {
            var facility = await _unitOfWork.ProductionFacilityRepository.GetByIdAsync(productionFacilityId);
            if (facility == null)
            {
                throw new ArgumentException($"Production facility with ID {productionFacilityId} not found.");
            }
            decimal occupiedArea = facility.Contracts.Sum(c => c.ProcessEquipment.Area * c.Quantity);
            return facility.StandardArea - occupiedArea;
        }
        public async Task<List<AvailableAreaDto>> GetAllAvailableAreaAsync()
        {
            var availableAreas = await _unitOfWork.ProductionFacilityRepository.GetAllAvailableAreasAsync();
            return availableAreas.ToList();
        }


    }
}