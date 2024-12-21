using JuniorDotNetTestTaskServiceHostingProcessEquipment.DTOs;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Models;

namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.Repositories.Interfaces
{
    public interface IProductionFacilityRepository : IRepository<ProductionFacility>
    {
        Task<IEnumerable<ProductionFacility>> GetAvailableFacilitiesAsync(decimal requiredArea);
        Task<decimal> GetAvailableAreaAsync(int productionFacilityId);
        Task<IEnumerable<AvailableAreaDto>> GetAllAvailableAreasAsync();
    }
}
