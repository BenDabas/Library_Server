using System.ComponentModel.DataAnnotations;

namespace Library_Server.Models
{
    public class WishlistItem
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string BookId { get; set; }

        public WishlistItem()
        {
            Id = Guid.NewGuid().ToString();
            UserId = string.Empty;
            BookId = string.Empty;
        }
        public WishlistItem(string userId, string bookId)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            BookId = bookId;
        }
    }
}
