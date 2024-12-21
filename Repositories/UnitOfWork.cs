using JuniorDotNetTestTaskServiceHostingProcessEquipment.Data;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Models;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Repositories.Interfaces;

namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private ProductionFacilityRepository _productionFacilityRepository;
        private Repository<ProcessEquipment> _processEquipmentRepository;
        private EquipmentPlacementContractRepository _equipmentPlacementContractRepository;

        public UnitOfWork(AppDbContext context) => _context = context;

        public IProductionFacilityRepository ProductionFacilityRepository =>
            _productionFacilityRepository ??= new ProductionFacilityRepository(_context);

        public IRepository<ProcessEquipment> ProcessEquipmentRepository =>
            _processEquipmentRepository ??= new Repository<ProcessEquipment>(_context);

        public IEquipmentPlacementContractRepository EquipmentPlacementContractRepository =>
            _equipmentPlacementContractRepository ??= new EquipmentPlacementContractRepository(_context);

        public async Task<bool> SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void Dispose() => _context.Dispose();
    }
}
