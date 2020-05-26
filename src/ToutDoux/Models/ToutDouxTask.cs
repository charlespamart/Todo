using System.Text.Json.Serialization;

namespace ToutDoux.Models
{
    public class ToutDouxTask
    {
        [JsonPropertyName("id")]
        public long Id{ get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("completed")]
        public bool Completed { get; set; }
    }
}
