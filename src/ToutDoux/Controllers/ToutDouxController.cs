﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using ToutDoux.Interfaces;
using ToutDoux.Models;

namespace ToutDoux.Controllers
{
    [ApiController]
    [Route("ToutDoux")]
    public class ToutDouxController : ControllerBase
    {
        private readonly ILogger<ToutDouxController> _logger;
        private readonly IToutDouxRepository _toutDouxRepository;

        public ToutDouxController(ILogger<ToutDouxController> logger, IToutDouxRepository toutDouxRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _toutDouxRepository = toutDouxRepository ?? throw new ArgumentNullException(nameof(toutDouxRepository));
        }

        [HttpGet]
        public ToutDouxTask[] Get()
        {
            return _toutDouxRepository.GetToutDouxTasks();
        }

        [HttpGet("{id}", Name = "GetById")]
        public ToutDouxTask GetById(int id)
        {
            return _toutDouxRepository.GetToutDouxTask(id);
        }

        [HttpPost]
        public IActionResult Post(ToutDouxTask toutDouxTask)
        {
            _toutDouxRepository.Add(toutDouxTask);
            return CreatedAtRoute("GetById", new { id = toutDouxTask.Id }, toutDouxTask);
        }

        [HttpDelete]
        public void Delete()
        {
            _toutDouxRepository.RemoveAll();
        }
    }
}
