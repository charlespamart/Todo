using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace Todo.API.Configuration
{
    public class ApiVersioningCustomError : DefaultErrorResponseProvider
    {
        public override IActionResult CreateResponse(ErrorResponseContext context)
        {
            var errorResponse = new
            {
                ResponseMessages = "Header X-Version is missing or wrongly typed. Version available : 1.0"
            };

            return new ObjectResult(errorResponse) { StatusCode = (int) HttpStatusCode.BadRequest };;
        }
    }
}