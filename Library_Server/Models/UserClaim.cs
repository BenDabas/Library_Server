using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Server.Models
{
    public class UserClaim : IdentityUserClaim<string>
    {
        public User User { get; set; }
    }
}
