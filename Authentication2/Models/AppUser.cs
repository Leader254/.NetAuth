using Microsoft.AspNetCore.Identity;

namespace Authentication2.Models
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
    }
}
