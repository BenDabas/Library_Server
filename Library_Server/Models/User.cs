using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Library_Server.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
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
