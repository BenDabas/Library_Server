using Newtonsoft.Json;
using static Library_Server.Services.BookService;

namespace Library_Server.Models.BookApi
{
    public class Volume
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("volumeInfo")]
        public VolumeInfo VolumeInfo { get; set; }
    }
}
