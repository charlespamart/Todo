namespace ToutDoux.Models
{
    public class ToutDouxTask
    {
        public long Id{ get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
        public string Url { get; set; }
    }

    public class ToutDouxTaskForCreationDTO
    {
        public string Title { get; set; }
    }
}
