using JuniorDotNetTestTaskServiceHostingProcessEquipment.DTOs.ProcessEquipmant;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Models;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Repositories;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Repositories.Interfaces;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessEquipmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProcessEquipmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Создать тип  оборудования
        [HttpPost("Create")]
        public async Task<IActionResult> CreateProcessEquipment([FromBody] CreateProcessEquipmentDto createProcessEquipmentDto )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingEquipment = await _unitOfWork.ProcessEquipmentRepository
                .FirstOrDefaultAsync(e => e.Code == createProcessEquipmentDto.Code);

            if (existingEquipment != null)
                return Conflict($"Process equipment with Code {createProcessEquipmentDto.Code} already exists.");

            var processEquipment = new ProcessEquipment
            {
                Code = createProcessEquipmentDto.Code,
                Name = createProcessEquipmentDto.Name,
                Area = createProcessEquipmentDto.Area
            };

            await _unitOfWork.ProcessEquipmentRepository.AddAsync(processEquipment);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetById), new { id = processEquipment.Id }, processEquipment);
        }

        // Получить тип оборудования по ID
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var equipment = await _unitOfWork.ProcessEquipmentRepository.GetByIdAsync(id);

            if (equipment == null)
                return NotFound($"Process equipment with ID {id} not found.");

            var equipmentDto = new ProcessEquipmentDto
            {
                
                Name = equipment.Name,
                Area = equipment.Area,
                Code = equipment.Code
            };

            return Ok(equipmentDto);
        }

        // Получить список всех типов оборудования
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var equipments = await _unitOfWork.ProcessEquipmentRepository.GetAllAsync();

            
           var equipmentDtos = equipments.Select(e => new ProcessEquipmentDto
            {   Id = e.Id,
                Name = e.Name,
                Area = e.Area,
                Code = e.Code
            }).ToList();

            return Ok(equipmentDtos);
        }

        // Обновить тип оборудования по ID
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProcessEquipmentDto processEquipmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingEquipment = await _unitOfWork.ProcessEquipmentRepository.GetByIdAsync(id);

            if (existingEquipment == null)
                return NotFound($"Process equipment with ID {id} not found.");

            existingEquipment.Name = processEquipmentDto.Name;
            existingEquipment.Code = processEquipmentDto.Code;
            existingEquipment.Area = processEquipmentDto.Area;

            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // Удалить тип оборудования по ID
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingEquipment = await _unitOfWork.ProcessEquipmentRepository.GetByIdAsync(id);

            if (existingEquipment == null)
                return NotFound($"Process equipment with ID {id} not found.");

            await _unitOfWork.ProcessEquipmentRepository.DeleteAsync(existingEquipment);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }

}