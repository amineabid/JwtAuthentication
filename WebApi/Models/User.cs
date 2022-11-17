using Microsoft.AspNetCore.Identity;

namespace JwtAuthentication.Server.Models
{
    public class User : IdentityUser
    {
        public string CustomerName { get; set; }
        public string City { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
