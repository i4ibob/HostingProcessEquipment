using System;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.Models
{
    public class ProductionFacility // Production facility
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; } = null!;
        public decimal StandardArea { get; set; }
        public ICollection<EquipmentPlacementContract> Contracts { get; set; } = new List<EquipmentPlacementContract>();
        
    }

}
