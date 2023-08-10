using Microsoft.AspNetCore.Identity;

namespace WebPulse2023.Models
{
    public class SystemUser : IdentityUser
    {
        public bool IsAdmin { get; set; }
    }
}
