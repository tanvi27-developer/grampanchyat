using Microsoft.AspNetCore.Identity;

namespace GrampanchayatSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}