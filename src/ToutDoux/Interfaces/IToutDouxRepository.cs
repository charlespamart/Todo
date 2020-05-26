using System.Collections.Generic;
using ToutDoux.Models;

namespace ToutDoux.Interfaces
{
    public interface IToutDouxRepository
    {
        List<ToutDouxTask> GetToutDoux();
        void Add(ToutDouxTask toutDouxTask);
        void Remove(ToutDouxTask toutDouxTask);
        void RemoveAll();
    }
}
