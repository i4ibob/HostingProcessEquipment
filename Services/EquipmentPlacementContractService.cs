using JuniorDotNetTestTaskServiceHostingProcessEquipment.DTOs;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Models;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Repositories.Interfaces;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Services.JuniorDotNetTestTaskServiceHostingProcessEquipment.Services;
using Microsoft.EntityFrameworkCore;

namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.Services
{
    public class EquipmentPlacementContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBackgroundTaskQueue _taskQueue;

        public EquipmentPlacementContractService(IUnitOfWork unitOfWork, IBackgroundTaskQueue taskQueue)
        {
            _unitOfWork = unitOfWork;
            _taskQueue = taskQueue;
        }



        // создаём контракт на размещение оборудования
        public async Task<(bool Success, string ErrorMessage, EquipmentPlacementContract Contract)> CreateContractAsync(EquipmentPlacementContractDto contractDto)
        {
            if (contractDto == null)
                return (false, "Invalid contract data.", null);

            var productionFacility = await GetProductionFacilityByCodeAsync(contractDto.ProductionFacilityCode);
            if (productionFacility == null)
                return (false, "Production facility not found.", null);

            var processEquipment = await GetProcessEquipmentByCodeAsync(contractDto.ProcessEquipmentCode);
            if (processEquipment == null)
                return (false, "Process equipment not found.", null);

            // проверка на наличие свободной площади на производственном объекте
            if (!await IsSufficientAreaAvailableAsync(productionFacility.Id, processEquipment.Area, contractDto.Quantity))
                return (false, "Not enough available area on the production facility.", null);

            if (contractDto.Quantity <= 0)
                return (false, "Quantity must be greater than zero.", null);

            var contract = new EquipmentPlacementContract
            {
                ProductionFacilityId = productionFacility.Id,
                ProcessEquipmentId = processEquipment.Id,
                Quantity = contractDto.Quantity
            };

            await _unitOfWork.EquipmentPlacementContractRepository.AddAsync(contract);
            await _unitOfWork.SaveAsync();

            // Добавление задачи в очередь
            await _taskQueue.QueueBackgroundWorkItemAsync(async token =>
            {
                // Пример фоновой обработки: логирование
                Console.WriteLine($"Contract {contract.Id} created for Production Facility {productionFacility.Name} with {contractDto.Quantity} units of {processEquipment.Name}.");
                await Task.CompletedTask;
            });

            return (true, null, contract);
        }



        public async Task<ContractDto> GetContractDtoByIdAsync(int id)
        {
            var contract = await _unitOfWork.EquipmentPlacementContractRepository
                .Query()
                .Include(c => c.ProductionFacility)
                .Include(c => c.ProcessEquipment)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null) return null;

            return new ContractDto
            {
                ContractID = contract.Id,
                ProductionFacilityName = contract.ProductionFacility?.Name,
                ProcessEquipmentName = contract.ProcessEquipment?.Name,
                Quantity = contract.Quantity
            };
        }



        public async Task<IEnumerable<ContractDto>> GetAllContractsAsync()
        {
            return await _unitOfWork.EquipmentPlacementContractRepository
                .Query()
                .Include(c => c.ProductionFacility)
                .Include(c => c.ProcessEquipment)
                .Select(c => new ContractDto
                {
                    ContractID = c.Id,
                    ProductionFacilityName = c.ProductionFacility.Name,
                    ProcessEquipmentName = c.ProcessEquipment.Name,
                    Quantity = c.Quantity
                })
                .ToListAsync();
        }

        public async Task<bool> DeleteContractAsync(int id)
        {
            var contract = await _unitOfWork.EquipmentPlacementContractRepository.GetByIdAsync(id);
            if (contract == null) return false;

            await _unitOfWork.EquipmentPlacementContractRepository.DeleteAsync(contract);
            await _unitOfWork.SaveAsync();

            return true;
        }

        private async Task<ProductionFacility> GetProductionFacilityByCodeAsync(int code)
        {
            return await _unitOfWork.ProductionFacilityRepository.FirstOrDefaultAsync(f => f.Code == code);
        }

        private async Task<ProcessEquipment> GetProcessEquipmentByCodeAsync(int code)
        {
            return await _unitOfWork.ProcessEquipmentRepository.FirstOrDefaultAsync(e => e.Code == code);
        }


        // проверка на наличие свободной площади на производственном объекте
        private async Task<bool> IsSufficientAreaAvailableAsync(int facilityId, decimal equipmentArea, int quantity)
        {
            if (equipmentArea <= 0 || quantity <= 0)
                return false;

            var availableArea = await _unitOfWork.ProductionFacilityRepository.GetAvailableAreaAsync(facilityId);
            return availableArea >= equipmentArea * quantity;
        }

        public async Task<decimal?> GetAvailableAreaAsync(int productionFacilityId)
        {
            // Получаем производственный объект
            var facility = await _unitOfWork.ProductionFacilityRepository.GetByIdAsync(productionFacilityId);
            if (facility == null)
                return null;

            // Получаем все контракты, связанные с объектом
            var contracts = await _unitOfWork.EquipmentPlacementContractRepository
                .FindAsync(c => c.ProductionFacilityId == productionFacilityId);

            // Суммируем занятое пространство
            var occupiedArea = contracts.Sum(c => c.Quantity * c.ProcessEquipment.Area);

            // Рассчитываем свободное пространство
            return facility.StandardArea - occupiedArea;
        }

    }
}

