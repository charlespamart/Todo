using System;
using System.Collections.Generic;
using System.Linq;
using ToutDoux.Interfaces;
using ToutDoux.Models;

namespace ToutDoux.Service
{
    public class ToutDouxRepository : IToutDouxRepository
    {
        private readonly ToutDouxContext _dbContext;

        public ToutDouxRepository(ToutDouxContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public List<ToutDouxTask> GetToutDoux()
        {
            return _dbContext.ToutDouxTasks.ToList();
        }

        public void Add(ToutDouxTask toutDouxTask)
        {
            _dbContext.ToutDouxTasks.Add(toutDouxTask);
            _dbContext.SaveChanges();
        }

        public void Remove(ToutDouxTask toutDouxTask)
        {
            _dbContext.ToutDouxTasks.Remove(toutDouxTask);
            _dbContext.SaveChanges();
        }
        public void RemoveAll()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}
