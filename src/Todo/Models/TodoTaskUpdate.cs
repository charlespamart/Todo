using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.API.Models
{
    public class TodoTaskUpdate
    {
        public string Title { get; set; }
        public bool? Completed { get; set; }
        public int? Order { get; set; }
    }
}
