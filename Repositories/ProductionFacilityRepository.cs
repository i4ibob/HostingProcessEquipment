

using JuniorDotNetTestTaskServiceHostingProcessEquipment.Data;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.DTOs;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Models;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.Repositories
{
    public class ProductionFacilityRepository : Repository<ProductionFacility>, IProductionFacilityRepository
    {
        private readonly AppDbContext _context;

        public ProductionFacilityRepository(AppDbContext context) : base(context)
        {
            _context = context; // Инициализация _context
        }



        // Возвращает список производственных объектов, площадь которых больше или равна заданной площади
        public async Task<IEnumerable<ProductionFacility>> GetAvailableFacilitiesAsync(decimal requiredArea) =>
            await _dbSet.Where(f => f.StandardArea >= requiredArea).ToListAsync();


        // Возвращает свободную площадь конкретного производственного объекта
        public async Task<decimal> GetAvailableAreaAsync(int productionFacilityId)
        {


            var facility = await _context.ProductionFacilities
                .Include(f => f.Contracts)
                .ThenInclude(c => c.ProcessEquipment)
                .FirstOrDefaultAsync(f => f.Id == productionFacilityId);

            if (facility == null)
                throw new ArgumentException($"Production facility with ID {productionFacilityId} not found.");

            decimal occupiedArea = facility.Contracts.Sum(c => c.ProcessEquipment.Area * c.Quantity);

            return facility.StandardArea - occupiedArea;
        }

        // Возвращает список всех производственных объектов и их доступную площадь
        public async Task<IEnumerable<AvailableAreaDto>> GetAllAvailableAreasAsync()
        {
            var facilities = await _context.ProductionFacilities
                .Include(f => f.Contracts)
                .ThenInclude(c => c.ProcessEquipment)
                .ToListAsync();

            return facilities.Select(facility => new AvailableAreaDto
            {
                Id = facility.Id,
                Name = facility.Name,
                StandardArea = facility.StandardArea,
                AvailableArea = facility.StandardArea - facility.Contracts.Sum(c => c.ProcessEquipment.Area * c.Quantity)
            }).ToList();
        }
    }
}
