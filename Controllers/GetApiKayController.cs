using JuniorDotNetTestTaskServiceHostingProcessEquipment.ApiAccess;
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.Controllers
{
    namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class GetAppSettingsController : ControllerBase
        {
            private readonly IConfiguration _configuration;

            public GetAppSettingsController(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            [HttpGet("apiKey")]  
            [AllowAnonymous]
            public IActionResult GetApiKey()
            {
             
                var apiKey = _configuration["ApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                    return NotFound("API key is not configured.");

                return Ok(new { ApiKey = apiKey });
            }

            [HttpGet("dbConnection")]  
            [AllowAnonymous]
            public IActionResult GetDbConnectionParam()
            {
                
                var connectionDb = new
                {
                    ServerName = "i4ibobserver.database.windows.net",
                    NameUser = "CloudSA2e1ee77b",
                    DbPassword = "q1W2E3r4t%"
                };

                return Ok(new { ConnectionDB = connectionDb });
            }
        }

    }
}
