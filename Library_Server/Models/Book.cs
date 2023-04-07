namespace Library_Server.Models
{
    public class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Publisher { get; set; }
        public List<string> Authors { get; set; }
        public List<Review> Reviews { get; set; }
        public DateTime PublisedDate { get; set; }

        public Book(string id, string title, string description, string image, string publisher, List<string> authors, List<Review> reviews, DateTime publisedDate)
        {
            Id = id;
            Title = title;
            Description = description;
            Image = image;
            Publisher = publisher;
            Authors = authors;
            Reviews = reviews;
            PublisedDate = publisedDate;
        }
    }
}
