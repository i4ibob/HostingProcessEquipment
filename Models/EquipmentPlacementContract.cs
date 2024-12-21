namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.Models
{
    public class EquipmentPlacementContract // Equipment placement contract
    {
        public int Id { get; set; }
        public int ProductionFacilityId { get; set; }
        public ProductionFacility ProductionFacility { get; set; } = null!;
        public int ProcessEquipmentId { get; set; }
        public ProcessEquipment ProcessEquipment { get; set; } = null!;
        public int Quantity { get; set; }
    }

}
