using Library_Server.DB;
using Library_Server.Models;
using Library_Server.Models.BookApi;
using Newtonsoft.Json;
using System.Globalization;

namespace Library_Server.Services
{
    public class BookService
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public BookService(ILogger<BookService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<List<Book>> GetBooks()
        {
            _logger.LogInformation("Start: BookService/GetBooks");
            var apiKey = "AIzaSyAAofAsb91eqLbY7B-I_NHpunsyhJTZ25g";
            //var query = "flowers+inauthor:keyes";
            var query = "subject=thriller";
            var url = $"https://www.googleapis.com/books/v1/volumes?q={query}&key={apiKey}&maxResults=10";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SearchResult>(json);

            var books = new List<Book>();

            foreach (var item in result.Items)
            {
                DateTime publishedDate;
                DateTime.TryParseExact(item.VolumeInfo.PublishedDate, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out publishedDate);

                var book = new Book(
                    item.Id,
                    item.VolumeInfo.Title,
                    item.VolumeInfo.Description,
                    item.VolumeInfo.ImageLinks?.Thumbnail,
                    item.VolumeInfo.Publisher,
                    item.VolumeInfo.Authors,
                    new List<Review>(),
                    publishedDate);

                books.Add(book);
            }
            _logger.LogInformation("End: BookService/GetBooks");
            return books;
        }

    }
}
