using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.DTOs
{
    public record EquipmentPlacementContractDto
    {
        [Required(ErrorMessage = "Production facility code is required."), DefaultValue(0000)]
        public int ProductionFacilityCode { get; set; } // Код производственного помещения

        [Required(ErrorMessage = "Process equipment type code is required."), DefaultValue(0000)]
        public int ProcessEquipmentCode { get; set; } // Код типа оборудования

        [Range(1, int.MaxValue, ErrorMessage = "Equipment quantity must be greater than zero.") , DefaultValue(1)]
        public int Quantity { get; set; } // Количество оборудования
    }
}
