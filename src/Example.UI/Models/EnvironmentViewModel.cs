using System;
using System.Net;

namespace Example.UI.Models
{
    public class EnvironmentViewModel
    {
        public string UiMachineName { get; set; }
        public string ApiMachineName { get; set; }
        public string ApiEndPoint { get; set; }
        public HttpStatusCode ApiHttpResponseCode { get; set; }
        public Exception ApiError { get; set; }
    }
}