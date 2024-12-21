using JuniorDotNetTestTaskServiceHostingProcessEquipment.DTOs.ProductFacility;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Services;
using Microsoft.AspNetCore.Mvc;

namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductionFacilityController : ControllerBase
    {
        private readonly ProductionFacilityService _service;

        public ProductionFacilityController(ProductionFacilityService service)
        {
            _service = service;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ProductionFacilityDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, errorMessage) = await _service.CreateAsync(dto);
            if (!success)
                return Conflict(new { Error = errorMessage });

            return Ok("Production facility created successfully.");
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var facility = await _service.GetByIdAsync(id);

            if (facility == null)
                return NotFound($"Production facility with ID {id} not found.");

            return Ok(facility);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var facilities = await _service.GetAllAsync();
            return Ok(facilities);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductionFacilityDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, errorMessage) = await _service.UpdateAsync(id, dto);

            if (!success)
                return Conflict(new { Error = errorMessage });

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Production facility with ID {id} not found.");

            return NoContent();
        }

        [HttpGet("GetAvailableArea")]
        public async Task<IActionResult> GetAvailableAreaAsync(int id)
        {
            var area = await _service.GetAvailableAreaAsync(id);
            return Ok(area);
        }


 

        [HttpGet("GetAllAvailableArea")]
        public async Task<IActionResult> GetAllAvailableAreaAsync()
        {
            var area = await _service.GetAllAvailableAreaAsync();
            return Ok(area);
        }
    }
}
