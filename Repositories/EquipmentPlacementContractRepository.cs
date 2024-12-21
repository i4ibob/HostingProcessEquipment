using JuniorDotNetTestTaskServiceHostingProcessEquipment.Data;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Models;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.Repositories
{
    public class EquipmentPlacementContractRepository : Repository<EquipmentPlacementContract>, IEquipmentPlacementContractRepository
    {
        private readonly AppDbContext _context;

        public EquipmentPlacementContractRepository(AppDbContext context) : base(context)
        {
            _context = context; // Инициализация _context
        }

        
    }

}
