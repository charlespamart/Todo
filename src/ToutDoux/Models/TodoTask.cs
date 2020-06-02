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
                return $"https://localhost:5001/todos/{Id}";
            }
        }
    }

    public class TodoTaskForCreationDTO
    {
        public string Title { get; set; }
    }
}
