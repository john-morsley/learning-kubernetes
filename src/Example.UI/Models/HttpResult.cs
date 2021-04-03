using System;
using System.Net;

namespace Example.UI.Models
{
    public class HttpResult<T>
    {
        public T Object { get; set; }
        public HttpStatusCode HttpResponseCode { get; set; }
        public string ReasonPhrase { get; set; }
        public string Endpoint { get; set; }
        public Exception Error { get; set; }
    }
}