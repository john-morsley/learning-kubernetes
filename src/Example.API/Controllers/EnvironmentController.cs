using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Example.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnvironmentController : ControllerBase
    {
        private readonly ILogger<EnvironmentController> _logger;

        public EnvironmentController(ILogger<EnvironmentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public EnvironmentResult Get()
        {
            _logger.LogDebug("Getting Environment...");
            var environment = new EnvironmentResult
            {
                MachineName = Environment.MachineName
            };
            return environment;
        }
    }
}