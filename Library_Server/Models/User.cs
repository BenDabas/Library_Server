using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Library_Server.Models
{
    public class User : IdentityUser
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string HashPassword { get; set; }
        public ICollection<WishlistItem> Wishlist { get; set; } = new List<WishlistItem>();

        public User(string userName, string name, string email, int age, string hashPassword)
        {
            Id = Guid.NewGuid().ToString();
            UserName = userName;
            Name = name;
            Email = email;
            Age = age;
            HashPassword = hashPassword;
            Wishlist = new List<WishlistItem>();
        }
    }
}
