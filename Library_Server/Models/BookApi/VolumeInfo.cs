using Newtonsoft.Json;
using static Library_Server.Services.BookService;

namespace Library_Server.Models.BookApi
{
    public class VolumeInfo
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("imageLinks")]
        public ImageLinks ImageLinks { get; set; }

        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        [JsonProperty("authors")]
        public List<string> Authors { get; set; }

        [JsonProperty("publishedDate")]
        public string PublishedDate { get; set; }
    }
}
