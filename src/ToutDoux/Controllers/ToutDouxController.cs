using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            Console.WriteLine(ToutDoux.Message);
        }
    }
}
