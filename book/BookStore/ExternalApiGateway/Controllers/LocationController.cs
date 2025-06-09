using Microsoft.AspNetCore.Mvc;

namespace ExternalApiGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILogger<LocationController> _logger;

        public LocationController(ILogger<LocationController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<string> Get()
        {
            var locationList = new List<string>()
            {
                "city1",
                "city2",
                "city3"
            };

            return locationList;
        }
    }
}
