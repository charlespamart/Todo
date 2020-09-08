namespace Todo.API.Models
{
    public class TodoTaskUpdate
    {
        public string Title { get; set; }
        public bool? Completed { get; set; }
        public int? Order { get; set; }
    }
}
