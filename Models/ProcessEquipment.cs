namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.Models
{
    public class ProcessEquipment // Type of process equipment
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; } = null!;
        public decimal Area { get; set; }
        public ICollection<EquipmentPlacementContract> Contracts { get; set; } = new List<EquipmentPlacementContract>();
    }

}
