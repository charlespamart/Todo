using Microsoft.AspNetCore.Mvc.Routing;

namespace Todo.Models
{
    public class TodoTask
    {
        public long Id{ get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
        public string Url
        {
            get
            {
                return $"https://localhost:5001/todotasks/{Id}"; // TODO: Change this atrocity
            }
        }
        public long Order { get; set; }
    }
}
