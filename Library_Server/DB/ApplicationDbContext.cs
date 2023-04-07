using Library_Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Library_Server.DB
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Review> Reviews { get; set; }  
        public DbSet<WishlistItem> Wishlists { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) 
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<WishlistItem>()
                .HasIndex(w => new { w.UserId, w.BookId })
                .IsUnique();
        }
    }
}
