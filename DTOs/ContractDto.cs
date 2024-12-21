namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.DTOs
{
    public record ContractDto
    {  public int ContractID { get; set; }
        public string ProductionFacilityName { get; set; }
        public string ProcessEquipmentName { get; set; }
        public int Quantity { get; set; }
    }
}