using System.ComponentModel.DataAnnotations;

namespace Library_Server.Models
{
    public class Review
    {
        [Key]
        public string Id { get; set; } 
        public string UserId { get; set; }
        public string BookId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }

        public Review()
        {
            Id = Guid.NewGuid().ToString();
            UserId = string.Empty;
            BookId = string.Empty;
            Content = string.Empty;
            CreatedDate = DateTime.Now;
        }
        public Review(string userId, string bookId, string content, DateTime createdDate)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            BookId = bookId;
            Content = content;
            CreatedDate = createdDate;
        }
    }
}
