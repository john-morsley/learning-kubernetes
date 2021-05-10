using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Example.API.Controllers.v2
{
    [ApiController]
    //[Route("api/v{version:apiVersion}/environment")]
    [Route("api/v2/environment")]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(GroupName = "v2")]
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
            var environment = new EnvironmentResult
            {
                MachineName = Environment.MachineName
            };
            return environment;
        }
    }
}