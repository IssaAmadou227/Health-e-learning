using Microsoft.AspNetCore.Identity;

namespace FutureDocteur.API.Models
{
    public class Role : IdentityRole<Guid>
    {
        public ICollection<RolePermission> RolePermissions { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public string Description {  get; set; }
    }
}
