using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using ToutDoux.Models;

namespace ToutDoux.Controllers
{
    [ApiController]
    [Route("ToutDoux")]
    public class ToutDouxController : ControllerBase
    {
        private readonly ILogger<ToutDouxController> _logger;
        private readonly ToutDouxContext _dbContext;

        public ToutDouxController(ILogger<ToutDouxController> logger, ToutDouxContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public void Get()
        {
        }

        [HttpGet("{id}")]
        public void GetById(int id)
        {
            _logger.LogInformation("ToutDouxTask {ToutDouxTaskId} was requested", id);
        }

        [HttpPost]
        public string Post(ToutDouxTask toutDouxTask)
        {
            _dbContext.ToutDouxTasks.Add(toutDouxTask);
            _dbContext.SaveChanges();

            return JsonSerializer.Serialize(toutDouxTask);
        }
        [HttpDelete]
        public void Delete()
        {
            _dbContext.ToutDouxTasks.RemoveRange(_dbContext.ToutDouxTasks);
            _dbContext.SaveChangesAsync();
        }
    }
}
