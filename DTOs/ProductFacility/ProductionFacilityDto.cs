namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.DTOs.ProductFacility
{
    public class ProductionFacilityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal StandardArea { get; set; }
        public int Code { get; set; }
        public List<int> Contracts { get; set; } = new List<int>();

        public ProductionFacilityDto() { }

        public ProductionFacilityDto(string name, decimal standardArea, int code)
        {
            Name = name;
            StandardArea = standardArea;
            Code = code;
        }
    }

}
