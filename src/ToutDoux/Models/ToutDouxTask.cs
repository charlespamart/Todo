using Microsoft.AspNetCore.Mvc.Routing;

namespace ToutDoux.Models
{
    public class ToutDouxTask
    {
        public long Id{ get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
        public string Url
        {
            get
            {
                return $"https://localhost:5001/toutdoux/{Id}";
            }
        }
    }

    public class ToutDouxTaskForCreationDTO
    {
        public string Title { get; set; }
    }
}
