using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ToutDoux.Controllers
{
    [ApiController]
    [Route("ToutDoux")]
    public class ToutDouxController : ControllerBase
    {
        private readonly ILogger<ToutDouxController> _logger;

        public ToutDouxController(ILogger<ToutDouxController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public void Get()
        {
            Console.WriteLine(ToutDoux.MessageGET);
        }
        [HttpPost]
        public async Task<string> Post()
        {
            Console.WriteLine(ToutDoux.MessagePOST);
            var request = HttpContext.Request;
            using var sr = new StreamReader(request.Body);
            return await sr.ReadToEndAsync();
        }
        [HttpDelete]
        public void Delete()
        {
            Console.WriteLine(ToutDoux.MessageDELETE);
        }
    }
}
