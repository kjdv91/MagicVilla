using Microsoft.Identity.Client;
using System.Net;

namespace Village_API.Models
{
    public class ApiResponse
    {
        public HttpStatusCode statusCode { get; set; }
        public bool isValid { get; set; } = true;
        public List<string>ErrorMessages { get; set; }
        public object Result { get; set; }
    }
}
