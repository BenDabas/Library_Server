using Newtonsoft.Json;
using static Library_Server.Services.BookService;

namespace Library_Server.Models.BookApi
{
    public class SearchResult
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }

        [JsonProperty("items")]
        public List<Volume> Items { get; set; }
    }
}
