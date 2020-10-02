using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.API.Models
{
    public class TodoTaskPatchAllCompletedState
    {
        public bool? Completed { get; set; }
    }
}
