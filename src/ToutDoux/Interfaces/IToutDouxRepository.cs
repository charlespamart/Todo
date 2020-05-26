using System.Collections.Generic;
using ToutDoux.Models;

namespace ToutDoux.Interfaces
{
    public interface IToutDouxRepository
    {
        ToutDouxTask[] GetToutDouxTasks();
        ToutDouxTask GetToutDouxTask(long id);
        void Add(ToutDouxTask toutDouxTask);
        void Remove(ToutDouxTask toutDouxTask);
        void RemoveAll();
    }
}
