using Microsoft.AspNetCore.Identity;

namespace FutureDocteur.API.Models
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public ApplicationUser User { get; set; }
        public Role Role { get; set; }
    }
}
