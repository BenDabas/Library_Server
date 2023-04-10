using Newtonsoft.Json;

namespace Library_Server.Models.BookApi
{
    public class ImageLinks
    {
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }
    }
}
