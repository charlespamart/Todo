using Microsoft.AspNetCore.Http;
using System;

namespace Todo.Domain.Models
{
    public class TodoTaskData
    {
        public Guid Id{ get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
        public Uri Url
        {
            get
            {
                return new Uri($"https://localhost:5001/todotasks/{Id}"); // TODO: Change this atrocity
            }
        }
        public int Order { get; set; }
    }
}
