using JuniorDotNetTestTaskServiceHostingProcessEquipment.DTOs;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Models;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Repositories.Interfaces;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Services;
using Microsoft.AspNetCore.Mvc;

namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentPlacementContractController : ControllerBase
    {
        private readonly EquipmentPlacementContractService _placementService;

        public EquipmentPlacementContractController(EquipmentPlacementContractService placementService)
        {
            _placementService = placementService;
        }

        // Создать контракт
        [HttpPost("Create")]
        public async Task<IActionResult> CreateEquipmentPlacementContract([FromBody] EquipmentPlacementContractDto contractDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, errorMessage, contract) = await _placementService.CreateContractAsync(contractDto);

            if (!success)
                return BadRequest(new { Error = errorMessage });

            var contractDtoResponse = await _placementService.GetContractDtoByIdAsync(contract.Id);

            return CreatedAtAction(nameof(GetById), new { id = contract.Id }, contractDtoResponse);
        }

        // Получить контракт по ID
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contractDto = await _placementService.GetContractDtoByIdAsync(id);

            if (contractDto == null)
                return NotFound($"Contract with ID {id} not found.");

            return Ok(contractDto);
        }

        // Получить все контракты
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var contractDtos = await _placementService.GetAllContractsAsync();

            if (!contractDtos.Any())
                return NotFound("No contracts found.");

            return Ok(contractDtos);
        }

        // Удалить контракт по ID
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _placementService.DeleteContractAsync(id);

            if (!deleted)
                return NotFound($"Contract with ID {id} not found.");

            return NoContent();
        }

        // Получить контракты по ID производственного объекта
        [HttpGet("GetByFacility/{id}")]
        public async Task<IActionResult> GetContractsByProductionFacilityId(int id)
        {
            var contractDto = await _placementService.GetContractDtoByIdAsync(id);

            if (contractDto == null)
                return NotFound($"Contract with ID {id} not found.");

            return Ok(contractDto);
        }
    }
}