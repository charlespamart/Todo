using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Todo.Domain.Models;

namespace Todo.DAL.Models
{
    public class TodoTaskData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
        public int Order { get; set; }

        public TodoTask ToDomain()
        {
            return new TodoTask(Id, Title, Completed, Order);
        }
    }
}
