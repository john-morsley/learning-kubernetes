using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Example.API.Controllers.v1
{
    [ApiController]
    //[Route("api/v{version:apiVersion}/environment")]
    [Route("api/v1/environment")]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class EnvironmentController : ControllerBase
    {
        private readonly ILogger<EnvironmentController> _logger;
        private readonly IConfiguration _configuration;

        public EnvironmentController(
            ILogger<EnvironmentController> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Gets information about the environment that this service is running.
        /// </summary>
        /// <returns></returns>
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