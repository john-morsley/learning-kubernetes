using System;
using Example.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Example.UI.Models;

namespace Example.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var result = await GetApiMachineName();

            var vm = new HomeViewModel();
            vm.UiMachineName = Environment.MachineName;
            if (result.Object != null)
            {
                vm.ApiMachineName = result.Object.MachineName;
            }
            vm.ApiEndPoint = result.Endpoint;
            vm.ApiHttpResponseCode = result.HttpResponseCode;
            vm.ApiError = result.Error;

            return View(vm);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<HttpResult<EnvironmentResult>> GetApiMachineName()
        {
            using (var client = _clientFactory.CreateClient())
            {
                var result = new HttpResult<EnvironmentResult>();
                try
                {
                    var endpoint = Environment.GetEnvironmentVariable("API_ENDPOINT");
                    if (string.IsNullOrEmpty(endpoint)) throw new InvalidOperationException("API_ENDPOINT was not provided!");
                    result.Endpoint = endpoint;
                    var baseUri = new Uri(endpoint);
                    var uri = new Uri(baseUri, "Environment");
                    var request = new HttpRequestMessage(HttpMethod.Get, uri);
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var environmentResult = await response.Content.ReadFromJsonAsync<EnvironmentResult>();
                        result.Object = environmentResult;
                    }
                    //else
                    //{
                    //     $"Could not determine API machine name ({response.ReasonPhrase})";
                    //}

                    result.HttpResponseCode = response.StatusCode;
                    result.ReasonPhrase = response.ReasonPhrase;
                    return result;
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    _logger.LogError(e, "An unexpected error occurred whilst attempting to get API machine name.");
                    //return "An unexpected error occurred whilst attempting to get API machine name.";
                    result.Error = e;
                    return result;
                }
            }
        }


    }
}