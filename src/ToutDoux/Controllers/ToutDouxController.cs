using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using ToutDoux.Models;

namespace ToutDoux.Controllers
{
    [ApiController]
    [Route("ToutDoux")]
    public class ToutDouxController : ControllerBase
    {
        private readonly ILogger<ToutDouxController> _logger;
        private readonly ToutDouxContext _DBContext;

        public ToutDouxController(ILogger<ToutDouxController> logger, ToutDouxContext DBContext)
        {
            _logger = logger;
            _DBContext = DBContext;
        }

        [HttpGet]
        public void Get()
        {
            Console.WriteLine(ToutDoux.MessageGET);
        }

        [HttpGet("{id}")]
        public void GetById()
        {
            Console.WriteLine(ToutDoux.MessageGET);
        }

        [HttpPost]
        public string Post(ToutDouxTask toutDouxTask)
        {
            _DBContext.ToutDouxTasks.Add(toutDouxTask);
            _DBContext.SaveChanges();

            return JsonSerializer.Serialize(toutDouxTask);
        }
        [HttpDelete]
        public void Delete()
        {
            _DBContext.ToutDouxTasks.RemoveRange(_DBContext.ToutDouxTasks);
            _DBContext.SaveChangesAsync();
        }
    }
}
