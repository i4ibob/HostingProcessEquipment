using JuniorDotNetTestTaskServiceHostingProcessEquipment.Data;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Models;

namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductionFacilityRepository ProductionFacilityRepository { get; }
        IRepository<ProcessEquipment> ProcessEquipmentRepository { get; }
        IEquipmentPlacementContractRepository EquipmentPlacementContractRepository { get; }
        Task<bool> SaveAsync();
    }

}
